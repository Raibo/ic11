using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ic11.TreeProcessing.Operations;

namespace ic11.TreeProcessing;
public class ProgramContext
{
    public Dictionary<string, string> PinAliases = new();
    public Dictionary<string, string> Variables = new();
    public int TempVarIndex = 0;

    public List<Operation> Operations = new(); 

    public Stack<int> WhileLabels = new();
    public int WhileCount = 0;


    public string ClaimTempVar(string type = "real")
    {   
        var tempVar = $"t{TempVarIndex++}";
        Variables.Add(tempVar, type);
        return tempVar;
    }

    public void InsertOperation(Operation operation)
    {
        Operations.Add(operation);
    }
}
