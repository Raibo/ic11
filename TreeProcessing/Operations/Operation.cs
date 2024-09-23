using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ic11.TreeProcessing.Operations;
public abstract class Operation
{
    public abstract string OpCode { get; }
    public string ResultName { get; set; }
}
