const vscode = require('vscode');
const path = require('path');
const fs = require('fs');

const https = require('https');
const http = require('http'); 

const { spawn } = require('child_process');

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
    } catch {
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

    if (process.platform !== 'win32') {
      fs.chmodSync(executablePath, 0o755);
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


function findProjectCompiler() {
  const folders = vscode.workspace.workspaceFolders;
  if (!folders || folders.length === 0) {
    return null;
  }

  for (const folder of folders) {
    const publishDir = path.join(folder.uri.fsPath, 'publish');
    let executableNames;
    if (process.platform === 'win32') {
      executableNames = ['ic11.exe'];
    } else if (process.platform === 'linux') {
      executableNames = ['ic11-linux-x64', 'ic11'];
    } else if (process.platform === 'darwin') {
      executableNames = ['ic11-osx-x64', 'ic11'];
    } else {
      executableNames = ['ic11', 'ic11.exe'];
    }

    for (const name of executableNames) {
      const candidate = path.join(publishDir, name);
      if (fs.existsSync(candidate)) {
        return candidate;
      }
    }
  }

  return null;
}


async function ensureCompiler(extensionRoot) {
  // 1. Prefer local compiler from workspace
  const localCompiler = findProjectCompiler();
  if (localCompiler) {
    return localCompiler;
  }

  // 2. Fallback to extension-managed compiler
  const binDir = path.join(extensionRoot, 'bin');
  const executableName = process.platform === 'win32' ? 'ic11.exe' : 'ic11';
  const executablePath = path.join(binDir, executableName);

  const extensionVersion = getExtensionVersion(extensionRoot);
  const storedVersion = getStoredCompilerVersion(binDir);
  const needsDownload =
    !fs.existsSync(executablePath) ||
    storedVersion !== extensionVersion;

  if (!needsDownload) {
    return executablePath;
  }

  const progressOptions = {
    location: vscode.ProgressLocation.Notification,
    title: storedVersion
      ? `Updating IC11 compiler to ${extensionVersion}...`
      : `Downloading IC11 compiler ${extensionVersion}...`,
    cancellable: false
  };

  return vscode.window.withProgress(progressOptions, async (progress) => {
    let lastPercent = 0;

    return downloadCompiler(extensionRoot, extensionVersion, (downloaded, total) => {
      const percent = Math.round((downloaded / total) * 100);
      progress.report({
        increment: percent - lastPercent,
        message: `${percent}%`
      });
      lastPercent = percent;
    });
  });
}





/**
 * Compiles IC11 code to IC10 assembly
 * @param {string} code - IC11 source code
 * @param {string} extensionRoot - Root directory of the extension
 * @param {Object} outputChannel - Output channel for logging (optional)
 * @param {Function} callback - Callback function (result, error)
 */
async function compileIC11Code(code, extensionRoot, outputChannel, callback) {
  const log = (message) => {
    if (outputChannel) {
      outputChannel.appendLine(message);
    }
  };
  
  try {
    log('[INFO] Ensuring compiler is available...');
    // Ensure compiler is available (download if necessary)
    const compilerPath = await ensureCompiler(extensionRoot);
    log(`[INFO] Using compiler: ${compilerPath}`);
    
    // Create a temporary file in the OS temp folder to store the code
    const tempDir = process.env.TEMP || process.env.TMPDIR || '/tmp';
    const tempFilePath = path.join(
      tempDir,
      `ic11-${process.pid}-${Date.now()}.ic11`
    );
    log(`[INFO] Writing temporary file: ${tempFilePath}`);
    fs.writeFileSync(tempFilePath, code);

    // Use the compiler executable
    const child = spawn(compilerPath, [tempFilePath], {
      windowsHide: true
    });
    
    let stdout = '';
    let stderr = '';
    
    child.stdout.on('data', (data) => {
      stdout += data.toString();
    });
    
    child.stderr.on('data', (data) => {
      stderr += data.toString();
    });
    
    child.on('close', (code) => {
      try {
        fs.unlinkSync(tempFilePath);
        log('[INFO] Temporary file cleaned up');
      } catch {}
    
      if (code !== 0) {
        const errorMessage = stderr || `Compiler exited with code ${code}`;
        log(`[ERROR] Compilation failed: ${errorMessage}`);
        callback(null, errorMessage);
        return;
      }
    
      log('[SUCCESS] Compilation completed successfully');
      callback(stdout, null);
    });

  } catch (error) {
    const errorMessage = `Failed to get compiler: ${error.message}`;
    log(`[ERROR] ${errorMessage}`);
    callback(null, errorMessage);
  }
}



// Create output channel for compiler messages
let outputChannel;

/**
 * Parses compiler error messages and creates diagnostics
 * @param {string} errorMessage - Error message from compiler
 * @param {vscode.TextDocument} document - The source document
 * @returns {vscode.Diagnostic[]} Array of diagnostics
 */
function parseCompilerErrors(errorMessage, document) {
  const diagnostics = [];
  const lines = errorMessage.split('\n');
  
  // Common error patterns (adjust based on actual compiler output format)
  // Example: "error: file.ic11:5:10: message"
  // Example: "file.ic11(5,10): error: message"
  const errorPatterns = [
    /(?:error|warning):\s*(?:.*?):(\d+):(\d+):\s*(.+)/i,
    /(?:.*?)\((\d+),(\d+)\):\s*(?:error|warning):\s*(.+)/i,
    /line\s+(\d+)[,\s]+column\s+(\d+)[:\s]+(?:error|warning):\s*(.+)/i,
  ];
  
  for (const line of lines) {
    for (const pattern of errorPatterns) {
      const match = line.match(pattern);
      if (match) {
        const lineNum = parseInt(match[1], 10) - 1; // VSCode uses 0-based line numbers
        const colNum = parseInt(match[2], 10) - 1; // VSCode uses 0-based column numbers
        const message = match[3].trim();
        
        if (lineNum >= 0 && lineNum < document.lineCount) {
          const range = new vscode.Range(
            lineNum,
            Math.max(0, colNum),
            lineNum,
            Math.max(0, colNum)
          );
          
          const severity = line.toLowerCase().includes('warning') 
            ? vscode.DiagnosticSeverity.Warning 
            : vscode.DiagnosticSeverity.Error;
          
          diagnostics.push(new vscode.Diagnostic(range, message, severity));
        }
      }
    }
  }
  
  // If no structured errors found, create a general diagnostic at the start
  if (diagnostics.length === 0 && errorMessage.trim()) {
    const range = new vscode.Range(0, 0, 0, 0);
    diagnostics.push(new vscode.Diagnostic(range, errorMessage, vscode.DiagnosticSeverity.Error));
  }
  
  return diagnostics;
}

async function activate(context) {
  // Create output channel for compiler messages
  outputChannel = vscode.window.createOutputChannel('IC11 Compiler');
  context.subscriptions.push(outputChannel);
  
  let diagnosticCollection = vscode.languages.createDiagnosticCollection('ic11');
  
  // Ensure compiler is available on activation (download if needed)
  // This happens in the background so activation doesn't block
  // Use context.extensionPath to get the extension's installation directory
  const extensionRoot = context.extensionPath;
  ensureCompiler(extensionRoot).catch((error) => {
    // Log error but don't block activation
    outputChannel.appendLine(`[ERROR] Failed to ensure compiler availability: ${error.message}`);
    outputChannel.show(true);
  });
  
  let disposable = vscode.commands.registerCommand('ic11.compile', async function () {
    const editor = vscode.window.activeTextEditor;
    if (!editor) {
      vscode.window.showWarningMessage('No active editor found');
      return;
    }
    
    const document = editor.document;
    if (document.languageId !== 'ic11') {
      vscode.window.showWarningMessage('Active file is not an IC11 file');
      return;
    }
    
    const code = document.getText();
    const uri = document.uri;
    const fileName = path.basename(uri.fsPath);

    // Clear previous diagnostics
    diagnosticCollection.clear();
    
    // Clear and show output channel
    outputChannel.clear();
    outputChannel.appendLine(`[${new Date().toLocaleTimeString()}] Compiling ${fileName}...`);
    outputChannel.appendLine('');

    compileIC11Code(code, extensionRoot, outputChannel, (result, error) => {
      if (error) {
        outputChannel.appendLine(`[ERROR] Compilation failed:`);
        outputChannel.appendLine(error);
        outputChannel.appendLine('');
        outputChannel.show(true); // Show the output channel
        
        // Parse errors and show as diagnostics
        const diagnostics = parseCompilerErrors(error, document);
        if (diagnostics.length > 0) {
          diagnosticCollection.set(uri, diagnostics);
        }
        
        // Show a brief notification with option to view details
        vscode.window.showErrorMessage(
          `Compilation failed: ${fileName}`,
          'View Details'
        ).then(selection => {
          if (selection === 'View Details') {
            outputChannel.show(true);
          }
        });
        return;
      }
      
      outputChannel.appendLine(`[SUCCESS] Compilation succeeded`);
      outputChannel.appendLine(`Output length: ${result.length} characters`);
      outputChannel.appendLine('');
      
      // Open a new tab with the compiled code
      const compiledFilePath = uri.fsPath.replace(path.extname(uri.fsPath), '.ic10');
      try {
        fs.writeFileSync(compiledFilePath, result);
        outputChannel.appendLine(`[INFO] Compiled output written to: ${path.basename(compiledFilePath)}`);
        
        vscode.workspace.openTextDocument(vscode.Uri.file(compiledFilePath)).then((doc) => {
          vscode.window.showTextDocument(doc);
        });
        
        // Show brief success notification
        vscode.window.showInformationMessage(
          `Compilation succeeded: ${fileName}`,
          'View Output'
        ).then(selection => {
          if (selection === 'View Output') {
            outputChannel.show(true);
          }
        });
      } catch (writeError) {
        outputChannel.appendLine(`[ERROR] Failed to write output file: ${writeError.message}`);
        outputChannel.show(true);
        vscode.window.showErrorMessage(`Failed to write compiled output: ${writeError.message}`);
      }
    });
  });

  context.subscriptions.push(diagnosticCollection);
  context.subscriptions.push(disposable);
}

exports.activate = activate;