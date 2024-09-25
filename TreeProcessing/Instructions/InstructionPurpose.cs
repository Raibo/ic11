namespace ic11.TreeProcessing.Instructions;
public enum InstructionPurpose
{
    Unspecified = 0,
    SaveVariableBeforeMethodCall,
    SaveParameterBeforeMethodCall,
    RestoreVariableAfterMethodCall,
}
