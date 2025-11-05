public interface IStateMachine
{
    public StateMachineRegistry Registry { get; }

    /// <summary>
    /// Intended to build:
    /// 1. States from data
    /// 2. Transitions from data
    /// 3. Correlations between states and transitions
    /// </summary>
    public void Build();
    
    /// <summary>
    /// Intended to evaluate the current relevant transitions and switch if necessary
    /// </summary>
    public void ArbitrateTransitions();
    
    /// <summary>
    /// Intended to evaluate the current state
    /// </summary>
    public void Evaluate();

    /// <summary>
    /// Intended to do pre- and post-logic for a state switch
    /// </summary>
    public void SwitchState(int nextStateId);
}
