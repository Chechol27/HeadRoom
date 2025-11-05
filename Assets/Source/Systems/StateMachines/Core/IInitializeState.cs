using UnityEngine;

/// <summary>
/// A logical state that requires to be initialized with the state machine's global data
/// </summary>
public interface IInitializeState
{
    public void Initialize(StateMachineRegistry globalData, Component stateMachine);
}
