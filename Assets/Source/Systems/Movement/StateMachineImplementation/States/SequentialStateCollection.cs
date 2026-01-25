using System;
using System.Collections.Generic;
using StateMachines.Core;
using StateMachines.Data;
using StateMachines.Transitions;
using UnityEngine;


/// <summary>
/// Utility state for executing multiple sub-states sequentially, think of it as a state group
/// </summary>
public class SequentialStateCollection : 
    MonoBehaviour, 
    ILogicalState, 
    IInitializeState, 
    IFinalizeState,
    ITransitionalState
{
    [field:SerializeField] public TransitionEvaluator TransitionEvaluator { get; set; }
    private List<ILogicalState> subStates = new();

    public Action<StateMachineRegistry, Component> PreInitialize { get; set; }
    public Action<StateMachineRegistry, Component> PostInitialize { get; set; }

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

    public Action<StateMachineRegistry, Component> PreExecute { get; set; }
    public Action<StateMachineRegistry, Component> PostExecute { get; set; }

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

    public Action<StateMachineRegistry, Component> PreFinalize { get; set; }
    public Action<StateMachineRegistry, Component> PostFinalize { get; set; }

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
