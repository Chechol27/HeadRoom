using System;

/// <summary>
/// Delegate container for condition evaluation in state machine evaluation time
/// </summary>
/// <typeparam name="TDelegate">The type of delegate this transition uses for evaluation</typeparam>
public interface ILogicalStateTransition<TDelegate> where TDelegate : Delegate
{
    bool Evaluate();
}
