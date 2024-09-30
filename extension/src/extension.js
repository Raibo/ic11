const vscode = require('vscode');
const { compileIC11Code } = require('./compiler');
const path = require('path');
const fs = require('fs');

function activate(context) {
  let diagnosticCollection = vscode.languages.createDiagnosticCollection('ic11');
  
  let disposable = vscode.commands.registerCommand('ic11.compile', function () {
    const editor = vscode.window.activeTextEditor;
    if (editor) {
      const document = editor.document;
      const code = document.getText();
      const uri = document.uri;

      diagnosticCollection.clear();

      compileIC11Code(code, (result, error) => {
        if (error) {
          vscode.window.showErrorMessage('Compilation failed');
          vscode.window.showErrorMessage(error);
          return;
        }
        vscode.window.showInformationMessage('Compilation succeeded');
        //vscode.window.showInformationMessage(result);
        // open a new tab with the compiled code
        const compiledFilePath = uri.fsPath.replace(path.extname(uri.fsPath), '.ic10');
        fs.writeFileSync(compiledFilePath, result);
        vscode.workspace.openTextDocument(vscode.Uri.file(compiledFilePath)).then((doc) => {
          vscode.window.showTextDocument(doc);
        });
      });
    }
  });

  context.subscriptions.push(diagnosticCollection);
  context.subscriptions.push(disposable);
}

exports.activate = activate;