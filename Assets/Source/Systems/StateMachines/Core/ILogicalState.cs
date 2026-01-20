using UnityEngine;

public interface ILogicalState
{
    public void Execute(StateMachineRegistry globalData, Component stateMachine);
}
