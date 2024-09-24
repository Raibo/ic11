﻿using ic11.TreeProcessing.Results;

namespace ic11.TreeProcessing.Operations;
public class Mul : Operation
{
    public Mul(Variable destination, IValue operand1, IValue operand2)
    {
        Destination = destination;
        Operand1 = operand1;
        Operand2 = operand2;
    }

    public override string OpCode => "MUL";

    public Variable Destination;
    public IValue Operand1;
    public IValue Operand2;

    public override string ToString() => $"{Destination} = {Operand1} * {Operand2}";
}