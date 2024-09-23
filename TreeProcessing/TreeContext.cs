using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ic11.TreeProcessing.Operations;

namespace ic11.TreeProcessing;
public class TreeContext
{
    public Dictionary<string, string> PinAliases = new();
    public Dictionary<string, string> Variables = new();
    public int TempVarIndex = 0;

    public List<Operation> Operations = new(); 

    public Stack<int> WhileLabels = new();
    public int WhileCount = 0;
}
