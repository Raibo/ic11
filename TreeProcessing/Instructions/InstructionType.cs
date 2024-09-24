namespace ic11.TreeProcessing.Instructions;
public enum InstructionType
{
    None = 0,
    PinName,
    Yield,
    Label,
    Jump,
    OperationBinary,
    OperationUnary,
    DeviceRead,
    DeviceWrite,
}
