using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ic11.TreeProcessing;
public class TreeContext
{
    public Dictionary<string, string> PinAliases = new();
    public Dictionary<string, string> Variables = new();

    public List<Operation> Operations = new(); 

    public Stack<int> WhileLabels = new();
    public int WhileCount = 0;
}
