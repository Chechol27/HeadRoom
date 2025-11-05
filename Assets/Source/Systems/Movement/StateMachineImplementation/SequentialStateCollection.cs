using System.Collections.Generic;
using UnityEngine;

public class SequentialStateCollection : ILogicalState, IInitializeState, IFinalizeState
{
    private List<ILogicalState> subStates = new List<ILogicalState>();

    public void AddState(ILogicalState state)
    {
        subStates.Add(state);
    }
    
    public void Initialize(StateMachineRegistry globalData, Component stateMachine)
    {
        foreach (ILogicalState logicalState in subStates)
        {
            if (logicalState is IInitializeState initState)
            {
                initState.Initialize(globalData, stateMachine);
            }
        }
    }
    
    public void Execute(StateMachineRegistry globalData, Component stateMachine)
    {
        foreach (ILogicalState logicalState in subStates)
        {
            logicalState.Execute(globalData, stateMachine);
        }
    }
    
    public void Finalize(StateMachineRegistry globalData, Component stateMachine)
    {
        foreach (ILogicalState logicalState in subStates)
        {
            if (logicalState is IFinalizeState finalizeState)
            {
                finalizeState.Finalize(globalData, stateMachine);
            }
        }
    }
}
