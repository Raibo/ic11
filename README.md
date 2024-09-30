# IC11 Compiler

A tool that translates (compiles) a high-level language program to an IC10 assembly for the Stationeers game. 

The language has a C-like syntax and supports basic instructions like if/then/else, while, function calls and return values.

# Usage

Provide a path to the source code as a first argument:
```bash
ic11.exe ./source.ic11
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
