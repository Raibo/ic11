const { exec } = require('child_process');
const fs = require('fs');
const path = require('path');
const { ensureCompiler } = require('./download-compiler');

/**
 * Compiles IC11 code to IC10 assembly
 * @param {string} code - IC11 source code
 * @param {Function} callback - Callback function (result, error)
 */
async function compileIC11Code(code, callback) {
  try {
    // Ensure compiler is available (download if necessary)
    const extensionRoot = path.join(__dirname, '..');
    const compilerPath = await ensureCompiler(extensionRoot);
    
    // Create a temporary file in the OS temp folder to store the code
    const tempDir = process.env.TEMP || process.env.TMPDIR || '/tmp';
    const tempFilePath = path.join(tempDir, `temp-${Date.now()}.ic11`);
    fs.writeFileSync(tempFilePath, code);

    // Use the compiler executable
    const command = `"${compilerPath}" "${tempFilePath}"`;
    exec(command, (error, stdout, stderr) => {
      // Clean up temp file
      try {
        if (fs.existsSync(tempFilePath)) {
          fs.unlinkSync(tempFilePath);
        }
      } catch (cleanupError) {
        // Ignore cleanup errors
      }

      if (error) {
        callback(null, stderr || error.message);
        return;
      }
      callback(stdout, null);
    });
  } catch (error) {
    callback(null, `Failed to get compiler: ${error.message}`);
  }
}

module.exports = { compileIC11Code };