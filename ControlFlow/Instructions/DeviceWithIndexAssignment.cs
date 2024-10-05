using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Instructions;
public class DeviceWithIndexAssignment : Instruction
{
    public IExpression IndexExpr;
    public IExpression? SlotIndexExpr;
    public string Member;
    public IExpression ValueExpr;

    private const string PinSetProperty = "IsSet";

    public DeviceWithIndexAssignment(IExpression indexExpression, string member, IExpression expression)
    {
        IndexExpr = indexExpression;
        Member = member;
        ValueExpr = expression;
    }

    public DeviceWithIndexAssignment(IExpression indexExpression, IExpression slotIndexExpr, string member, IExpression expression)
    {
        IndexExpr = indexExpression;
        SlotIndexExpr = slotIndexExpr;
        Member = member;
        ValueExpr = expression;
    }

    public override string Render()
    {
        return (isPinSet: Member == PinSetProperty, isSlotDefined: SlotIndexExpr is not null) switch
        {
            (isPinSet: true, isSlotDefined: true) => throw new Exception($"IsSet property is not relevant for slot access"),
            (isPinSet: true, isSlotDefined: false) => throw new Exception($"IsSet property cannot be written to"),
            (isPinSet: false, isSlotDefined: true) => $"ss d{IndexExpr.Render()} {SlotIndexExpr!.Render()} {Member} {ValueExpr.Render()}",
            (isPinSet: false, isSlotDefined: false) => $"s d{IndexExpr.Render()} {Member} {ValueExpr.Render()}",
        };
    }
}
