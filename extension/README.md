# ic11 README

A VSCode extension for the .ic11 syntax highlighting and compilation

## Features

- Syntax highlighting for `.ic11` and `.ic10` files
- Compile IC11 code to IC10 assembly
- **Automatic compiler download**: The compiler executable is automatically downloaded from GitHub releases on first use

## Build

The extension downloads the compiler executable on-demand from GitHub releases, so no manual bundling is required:

1. Package the extension:
   ```
   npm install -g @vscode/vsce
   vsce package
   ```

The extension will be lightweight (~1MB) and will download the compiler (68MB) automatically when first used.

## Install

1. Build the extension (see above)
2. Go to the Extensions view, click the ellipsis (...), and select "Install from VSIX..."
3. On first compilation, the extension will automatically download the compiler from GitHub releases

## Development

For local development, you can manually copy the compiler:
```
npm run copy-compiler
```

This copies the compiler from `../publish/ic11.exe` to `bin/ic11.exe` for local testing.
