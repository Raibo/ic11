# Setup

```
pip install antlr4-tools
```

# Debug GUI

```
cd grammars # this dir
antlr4-parse Ic11.g4 program -gui ../examples/test.ic11
```

# Generate 

```
cd grammars # this dir
antlr4 -o ../src/ic11/generated -visitor Ic11.g4
```
