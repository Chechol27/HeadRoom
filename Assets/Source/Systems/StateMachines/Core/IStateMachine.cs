public interface IStateMachine
{
    public StateMachineRegistry Registry { get; }
    
    /// <summary>
    /// Intended to evaluate the current state
    /// </summary>
    public void Evaluate();

    /// <summary>
    /// Intended to do pre- and post-logic for a state switch
    /// </summary>
    public void SwitchState(int nextStateId);
}
