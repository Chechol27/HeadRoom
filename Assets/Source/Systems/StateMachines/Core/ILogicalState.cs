
using UnityEngine;

public interface ILogicalState
{
    public void Execute(StateMachineRegistry globalData, Component stateMachine);
}

/// <summary>
/// A logical state that requires additional "local" data to function as well as the state machine's global data
/// </summary>
/// <typeparam name="TGlobalData">The type of global data used in the parent state machine</typeparam>
/// <typeparam name="TLocalData">The type of local data used and managed by this state</typeparam>
public interface ILogicalState<TLocalData> : ILogicalState where TLocalData : class 
{
    TLocalData Settings { get; set; }
}
