using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ic11.Operations;
public class Add : IBinaryOperation
{
    public void Write(StringBuilder sb, string regResult, string regA, string regB)
    {
        sb.AppendLine($"sum {regResult} {regA} {regB}");
    }
}
