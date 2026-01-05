const https = require('https');
const http = require('http');
const fs = require('fs');
const path = require('path');
const vscode = require('vscode');

const GITHUB_API_BASE = 'api.github.com';
const REPO_OWNER = 'Raibo';
const REPO_NAME = 'ic11';

/**
 * Gets the extension version from package.json
 * @param {string} extensionRoot - Root directory of the extension
 * @returns {string} Extension version
 */
function getExtensionVersion(extensionRoot) {
  const packageJsonPath = path.join(extensionRoot, 'package.json');
  const packageJson = JSON.parse(fs.readFileSync(packageJsonPath, 'utf8'));
  return packageJson.version;
}

/**
 * Gets the stored compiler version
 * @param {string} binDir - Directory where compiler is stored
 * @returns {string|null} Stored version or null if not found
 */
function getStoredCompilerVersion(binDir) {
  const versionPath = path.join(binDir, '.version');
  if (fs.existsSync(versionPath)) {
    try {
      return fs.readFileSync(versionPath, 'utf8').trim();
    } catch (error) {
      return null;
    }
  }
  return null;
}

/**
 * Stores the compiler version
 * @param {string} binDir - Directory where compiler is stored
 * @param {string} version - Version to store
 */
function storeCompilerVersion(binDir, version) {
  const versionPath = path.join(binDir, '.version');
  fs.writeFileSync(versionPath, version, 'utf8');
}

/**
 * Gets a specific release by tag from GitHub API
 * @param {string} tag - Release tag (e.g., "v1.0.0" or "1.0.0")
 * @returns {Promise<Object>} Release information with assets
 */
function getReleaseByTag(tag) {
  return new Promise((resolve, reject) => {
    // Normalize tag - remove 'v' prefix if present, then add it back for API
    const normalizedTag = tag.startsWith('v') ? tag : `v${tag}`;
    
    const options = {
      hostname: GITHUB_API_BASE,
      path: `/repos/${REPO_OWNER}/${REPO_NAME}/releases/tags/${normalizedTag}`,
      method: 'GET',
      headers: {
        'User-Agent': 'ic11-vscode-extension',
        'Accept': 'application/vnd.github.v3+json'
      }
    };

    https.get(options, (res) => {
      let data = '';

      res.on('data', (chunk) => {
        data += chunk;
      });

      res.on('end', () => {
        if (res.statusCode !== 200) {
          reject(new Error(`GitHub API error: ${res.statusCode} - ${data}. Release tag: ${normalizedTag}`));
          return;
        }

        try {
          const release = JSON.parse(data);
          resolve(release);
        } catch (error) {
          reject(new Error(`Failed to parse GitHub API response: ${error.message}`));
        }
      });
    }).on('error', (error) => {
      reject(new Error(`Failed to fetch release info: ${error.message}`));
    });
  });
}

/**
 * Finds the appropriate asset for the current platform
 * @param {Array} assets - Array of release assets
 * @returns {Object|null} The matching asset or null
 */
function findPlatformAsset(assets) {
  let platformLabel;
  let platformName;
  
  if (process.platform === 'win32') {
    platformLabel = 'Win-x64';
    platformName = 'ic11.exe';
  } else if (process.platform === 'linux') {
    platformLabel = 'Linux-x64';
    platformName = 'ic11-linux-x64';
  } else if (process.platform === 'darwin') {
    platformLabel = 'macOS-x64';
    platformName = 'ic11-osx-x64';
  } else {
    // Unknown platform, try to find any executable
    return assets.find(asset => 
      asset.name === 'ic11.exe' || 
      asset.name.startsWith('ic11-') ||
      asset.label
    );
  }
  
  // Try to find by label first (most reliable)
  let asset = assets.find(a => a.label === platformLabel);
  
  // Fallback to filename matching
  if (!asset) {
    asset = assets.find(a => 
      a.name === platformName ||
      (process.platform === 'win32' && a.name.toLowerCase().endsWith('.exe')) ||
      (process.platform === 'linux' && a.name.includes('linux')) ||
      (process.platform === 'darwin' && (a.name.includes('osx') || a.name.includes('macos')))
    );
  }
  
  return asset || null;
}

/**
 * Downloads a file from a URL
 * @param {string} url - URL to download from
 * @param {string} filePath - Path to save the file
 * @param {Function} progressCallback - Callback for download progress
 * @returns {Promise<void>}
 */
function downloadFile(url, filePath, progressCallback) {
  return new Promise((resolve, reject) => {
    const file = fs.createWriteStream(filePath);
    const urlObj = new URL(url);
    const client = urlObj.protocol === 'https:' ? https : http;

    client.get(url, {
      headers: {
        'User-Agent': 'ic11-vscode-extension'
      }
    }, (response) => {
      // Handle redirects
      if (response.statusCode === 301 || response.statusCode === 302) {
        file.close();
        fs.unlinkSync(filePath);
        return downloadFile(response.headers.location, filePath, progressCallback)
          .then(resolve)
          .catch(reject);
      }

      if (response.statusCode !== 200) {
        file.close();
        fs.unlinkSync(filePath);
        reject(new Error(`Download failed: ${response.statusCode} ${response.statusMessage}`));
        return;
      }

      const totalSize = parseInt(response.headers['content-length'], 10);
      let downloadedSize = 0;

      response.on('data', (chunk) => {
        downloadedSize += chunk.length;
        if (progressCallback && totalSize) {
          progressCallback(downloadedSize, totalSize);
        }
      });

      response.pipe(file);

      file.on('finish', () => {
        file.close();
        // Make executable on Unix systems
        if (process.platform !== 'win32') {
          fs.chmodSync(filePath, 0o755);
        }
        resolve();
      });
    }).on('error', (error) => {
      file.close();
      if (fs.existsSync(filePath)) {
        fs.unlinkSync(filePath);
      }
      reject(error);
    });
  });
}

/**
 * Downloads the compiler executable from GitHub releases
 * @param {string} extensionRoot - Root directory of the extension
 * @param {string} version - Extension version to download compiler for
 * @param {Function} progressCallback - Callback for download progress
 * @returns {Promise<string>} Path to the downloaded executable
 */
async function downloadCompiler(extensionRoot, version, progressCallback) {
  const binDir = path.join(extensionRoot, 'bin');
  const executableName = process.platform === 'win32' ? 'ic11.exe' : 'ic11';
  const executablePath = path.join(binDir, executableName);
  
  // Ensure bin directory exists
  if (!fs.existsSync(binDir)) {
    fs.mkdirSync(binDir, { recursive: true });
  }

  // Get release info for the specific version
  const release = await getReleaseByTag(version);
  
  // Find the appropriate asset for this platform
  const asset = findPlatformAsset(release.assets);
  
  if (!asset) {
    throw new Error(`No compatible compiler found for platform: ${process.platform} in release ${version}. Available assets: ${release.assets.map(a => a.name).join(', ')}`);
  }

  // Download to a temporary file first (asset might have a different name)
  const tempDownloadPath = path.join(binDir, `temp-${asset.name}`);
  
  try {
    // Download the asset
    await downloadFile(asset.browser_download_url, tempDownloadPath, progressCallback);
    
    // Remove existing executable if it exists (for upgrade scenario)
    if (fs.existsSync(executablePath)) {
      fs.unlinkSync(executablePath);
    }
    
    // Rename to the expected executable name
    if (tempDownloadPath !== executablePath) {
      fs.renameSync(tempDownloadPath, executablePath);
    }
    
    // Store the version we downloaded
    storeCompilerVersion(binDir, version);
  } catch (error) {
    // Clean up temp file on error
    if (fs.existsSync(tempDownloadPath)) {
      fs.unlinkSync(tempDownloadPath);
    }
    throw error;
  }
  
  return executablePath;
}

/**
 * Gets the compiler path, downloading if necessary
 * @param {string} extensionRoot - Root directory of the extension
 * @returns {Promise<string>} Path to the compiler executable
 */
async function ensureCompiler(extensionRoot) {
  const binDir = path.join(extensionRoot, 'bin');
  const executableName = process.platform === 'win32' ? 'ic11.exe' : 'ic11';
  const executablePath = path.join(binDir, executableName);
  
  // Get current extension version
  const extensionVersion = getExtensionVersion(extensionRoot);
  
  // Skip snapshot versions (local development)
  if (extensionVersion.includes('-snapshot')) {
    // For snapshot versions, check if compiler exists locally
    if (fs.existsSync(executablePath)) {
      return executablePath;
    }
    // Don't try to download for snapshot versions
    throw new Error('Compiler not found locally. For snapshot versions, use "npm run copy-compiler" to copy from local build.');
  }

  // Get stored compiler version
  const storedVersion = getStoredCompilerVersion(binDir);
  const needsDownload = !fs.existsSync(executablePath) || storedVersion !== extensionVersion;

  if (!needsDownload) {
    // Compiler exists and version matches
    return executablePath;
  }

  // Download with progress
  const progressOptions = {
    location: vscode.ProgressLocation.Notification,
    title: storedVersion 
      ? `Updating IC11 compiler to ${extensionVersion}...`
      : `Downloading IC11 compiler ${extensionVersion}...`,
    cancellable: false
  };

  return vscode.window.withProgress(progressOptions, async (progress) => {
    return downloadCompiler(extensionRoot, extensionVersion, (downloaded, total) => {
      const percent = Math.round((downloaded / total) * 100);
      progress.report({ increment: percent, message: `${percent}%` });
    });
  });
}

module.exports = { ensureCompiler, downloadCompiler };

