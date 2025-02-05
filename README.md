# IC11 Compiler

A tool that translates (compiles) a high-level language program to an IC10 assembly for the Stationeers game. 

The language has a C-like syntax and supports basic instructions like if/then/else, while, function calls and return values.

# Usage

Provide a path to the source code as a first argument.
If the path is a directory, then all `*.ic11` files in this directory will be compiled.
Optional second argument `-w` will save compiled code to new `*.ic10` files next to the sources.
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
dotnet build -c Release --no-restore
```

Build a single exe:
```bash
dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -o ./publish
```
