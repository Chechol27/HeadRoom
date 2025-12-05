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
            logicalState.Execute(globalData, stateMachine);
        }

        if (TransitionEvaluator.Evaluate(globalData, out ILogicalStateTransition hit))
        {
            ((IStateMachine)stateMachine).SwitchState(hit.To);
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
