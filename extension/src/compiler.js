const { exec } = require('child_process');
const fs = require('fs');

function compileIC11Code(code, callback) {
  // create a temporary file in the OS temp folder to store the code
  const tempFilePath = `${process.env.TEMP}/temp.ic11`;
  fs.writeFileSync(tempFilePath, code);

  exec(`ic11 ${tempFilePath}`, (error, stdout, stderr) => {
    if (error) {
      callback(null, stderr);
      return;
    }
    callback(stdout, null);
  });
}

module.exports = { compileIC11Code };