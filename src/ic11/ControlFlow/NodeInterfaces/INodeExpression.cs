namespace ic11.ControlFlow.NodeInterfaces;

public interface INodeExpression: INode, IExpression
{
    public int FirstIndexInTree => GetFirstExpressionIndex(this);

    private int GetFirstExpressionIndex(INodeExpression ex)
    {
        var index = ex.IndexInScope;

        if (ex is IExpressionContainer ec)
        {
            foreach (var item in ec.Expressions)
                index = Math.Min(index, GetFirstExpressionIndex(item));
        }

        return index;
    }
}