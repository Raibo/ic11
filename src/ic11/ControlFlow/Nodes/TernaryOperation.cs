using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;
using ic11.ControlFlow.TreeProcessing;

namespace ic11.ControlFlow.Nodes;
public class TernaryOperation : Node, INodeExpression, IExpressionContainer
{
    public INodeExpression OperandA;
    public INodeExpression OperandB;
    public INodeExpression OperandC;
    public string Operation;
    public Variable? Variable { get; set; }

    public decimal? CtKnownValue
    {
        get
        {
            if (!OperandA.CtKnownValue.HasValue || !OperandB.CtKnownValue.HasValue || !OperandC.CtKnownValue.HasValue)
                return null;

            return CtCalculate(OperandA.CtKnownValue.Value, OperandB.CtKnownValue.Value, OperandC.CtKnownValue.Value);
        }
    }

    public TernaryOperation(INodeExpression operandA, INodeExpression operandB, INodeExpression operandC, string operation)
    {
        OperandA = operandA;
        OperandB = operandB;
        OperandC = operandC;
        Operation = GetOperation(operation ?? throw new ArgumentNullException(nameof(operation)));
        operandA.Parent = this;
        operandB.Parent = this;
        operandC.Parent = this;
    }

    public IEnumerable<INodeExpression> Expressions
    {
        get
        {
            yield return OperandA;
            yield return OperandB;
            yield return OperandC;
        }
    }

    private string GetOperation(string input)
    {
        return OperationHelper.SymbolsTernaryOpMap.TryGetValue(input, out var symbolOp)
            ? symbolOp
            : input;
    }

    private decimal CtCalculate(decimal a, decimal b, decimal c)
    {
        return Operation switch
        {
            "select" => a != 0m ? b : c,
            "sap" => Math.Abs((double)a - (double)b) <= Math.Max((double)c * Math.Max(Math.Abs((double)a), Math.Abs((double)b)), float.Epsilon * 8d) ? 1 : 0,
            "sna" => Math.Abs((double)a - (double)b) >  Math.Max((double)c * Math.Max(Math.Abs((double)a), Math.Abs((double)b)), float.Epsilon * 8d) ? 1 : 0,
            _ => throw new Exception("Unexpected ternary operation type"),
        };
    }
}
