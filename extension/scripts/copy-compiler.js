const fs = require('fs');
const path = require('path');

const extensionRoot = __dirname.replace(/[\\/]scripts$/, '');
const srcPath = path.join(extensionRoot, '..', 'publish', 'ic11.exe');
const destDir = path.join(extensionRoot, 'bin');
const destPath = path.join(destDir, 'ic11.exe');

if (!fs.existsSync(srcPath)) {
  console.error(`Error: Source compiler not found at ${srcPath}`);
  console.error('Please build the compiler first (publish/ic11.exe must exist)');
  process.exit(1);
}

if (!fs.existsSync(destDir)) {
  fs.mkdirSync(destDir, { recursive: true });
}

fs.copyFileSync(srcPath, destPath);
console.log(`Compiler copied successfully from ${srcPath} to ${destPath}`);

