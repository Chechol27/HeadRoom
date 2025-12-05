using System.Collections.Generic;
using UnityEngine;

public class SequentialStateCollection : 
    MonoBehaviour, 
    ILogicalState, 
    IInitializeState, 
    IFinalizeState,
    ITransitionalState
{
    [field:SerializeField] public TransitionEvaluator TransitionEvaluator { get; set; }
    private List<ILogicalState> subStates = new(); 
    
    public void Initialize(StateMachineRegistry globalData, Component stateMachine)
    {
        subStates.Clear();
        foreach (Transform t in transform)
        {
            if (t.TryGetComponent(out ILogicalState state))
            {
                subStates.Add(state);
                if (state is IInitializeState initState)
                {
                    initState.Initialize(globalData, stateMachine);
                }
            }
        }
    }
    
    public void Execute(StateMachineRegistry globalData, Component stateMachine)
    {
        foreach (ILogicalState logicalState in subStates)
        {
            ((ILogicalState)logicalState).Execute(globalData, stateMachine);
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
