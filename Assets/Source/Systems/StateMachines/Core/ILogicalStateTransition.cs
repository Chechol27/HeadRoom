
public interface ILogicalStateTransition
{
    ILogicalState To { get; set; }
    
    bool Evaluate(StateMachineRegistry registry);
}