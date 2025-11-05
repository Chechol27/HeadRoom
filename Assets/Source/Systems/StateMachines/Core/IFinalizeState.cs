using UnityEngine;

/// <summary>
/// A logical state that requires to do release operations with the state machine's global data once the state has exited
/// </summary>
public interface IFinalizeState
{
    public void Finalize(StateMachineRegistry globalData, Component stateMachine);
}
