using Antlr4.Runtime.Misc;
using ic11.TreeProcessing.Context;
using ic11.TreeProcessing.Results;

namespace ic11.TreeProcessing;
public class MethodFindVisitor : Ic11BaseVisitor<IValue>
{
    public CompileContext CompileContext;

    public MethodFindVisitor(CompileContext compileContext)
    {
        CompileContext = compileContext;
    }

    public override IValue VisitFunction([NotNull] Ic11Parser.FunctionContext context)
    {
        var identifiers = context.IDENTIFIER();

        var name = identifiers[0].GetText();
        var retType = context.retType.Text;

        var paramCount = context.IDENTIFIER().Length - 1;

        var returnsValue = retType switch
        {
            "void" => false,
            "real" => true,
            _ => throw new Exception($"Unrecognized return type {retType}. Supported: void, real."),
        };

        if (name == "Main" && paramCount > 0)
            throw new Exception("Main cannot have parameters");

        if (name == "Main" && returnsValue)
            throw new Exception("Main cannot return value");

        var method = new DeclaredMethod(name, returnsValue, paramCount);

        if (CompileContext.DeclaredMethods.ContainsKey(name))
            throw new Exception($"Method {name} is already defined");

        CompileContext.DeclaredMethods[name] = method;

        return null;
    }
}
