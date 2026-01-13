using ic11.ControlFlow.NodeInterfaces;
using ic11.ControlFlow.Nodes;

namespace ic11.ControlFlow.TreeProcessing;
public class RootStatementsSorter
{
    public void SortStatements(Root node)
    {
        var statementsList = node.Statements;

        // List<T>.Sort/Array.Sort are not always stable
        List<IStatement> temporaryStatements = new(statementsList.Count);
        temporaryStatements.AddRange(statementsList.OrderBy(s => s, new StatementComparer()));
        statementsList.Clear();
        statementsList.AddRange(temporaryStatements);
    }

    private class StatementComparer : IComparer<IStatement>
    {
        private static Dictionary<Type, int> _typesMap = new() {
            [typeof(PinDeclaration)] = 0,
            [typeof(ConstantDeclaration)] = 1,
            [typeof(MethodDeclaration)] = 2,
        };

        public int Compare(IStatement x, IStatement y)
        {
            var xTypeIndex = _typesMap[x.GetType()];
            var yTypeIndex = _typesMap[y.GetType()];

            if (xTypeIndex < yTypeIndex)
                return -1;

            if (xTypeIndex > yTypeIndex)
                return 1;

            if (x is PinDeclaration xx && y is PinDeclaration yy)
                return xx.Device.CompareTo(yy.Device);

            if (x is MethodDeclaration xxx && y is MethodDeclaration yyy)
            {
                if (xxx.Name == "Main")
                    return -1;

                if (yyy.Name == "Main")
                    return 1;
            }

            return 0;
        }
    }
}
