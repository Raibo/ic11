# IC11 Compiler

A tool that translates (compiles) a high-level language program to an IC10 assembly for the Stationeers game. 

The language has a C-like syntax and supports basic instructions like if/then/else, while, function calls and return values.

Use the [Wiki](https://github.com/Raibo/ic11/wiki) for the ic11 language reference.

# Usage

`ic11.exe <path> [-w]`  
Provide a path to the source code as a first argument.  
If the path is a file, then this file will be compiled.  
If the path is a directory, then all `*.ic11` files in this directory will be compiled.  
Optional second argument `-w` will write compiled code to new `*.ic10` files next to the sources.  
```bash
ic11.exe source.ic11
ic11.exe ./examples
ic11.exe source.ic11 -w
ic11.exe ./examples -w
```

The compiled code is provided in the Stdout.

# Building

Download/update dependencies:
```bash
dotnet restore
```

Build binaries:
```bash
dotnet build src/ic11/ic11.csproj -c Release --no-restore
```

Build a single exe:
```bash
dotnet publish src/ic11/ic11.csproj -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -o ./publish
```
