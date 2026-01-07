# ic11 README

A VSCode extension for the .ic11 syntax highlighting and compilation

## Features

- Syntax highlighting for `.ic11` and `.ic10` files
- Compile IC11 code to IC10 assembly
- The compiler executable is automatically downloaded from GitHub releases on first use

## Install

1. Build the extension (see below) or download from the GitHub releases
2. Go to View/Command Palette, and type "Install from VSIX...", select the extension file
3. The extension will automatically download the compiler from GitHub releases

## Build Extension

**Source code:** [https://github.com/Raibo/ic11](https://github.com/Raibo/ic11)

The extension downloads the compiler executable on-demand from GitHub releases, so no manual bundling is required:

1. Package the extension:
   ```
   npm install -g @vscode/vsce
   vsce package
   ```

2. Install the the vsix file (see "Install")