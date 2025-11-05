using System;
using System.Collections.Generic;
using System.Linq;
using StateMachines.Core.Update;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
public class MotionStateMachine : MonoBehaviour, IStateMachine, ICharacterComponent
{
    [SerializeField]
    [Tooltip("Only relevant at awake")]
    StateMachineUpdateMode updateMode;

    [field:SerializeField] public StateMachineRegistry Registry { get; private set; }

    private List<ILogicalState> states = new();
    private List<TransitionEvaluationDelegate> transitions = new();
    
    /// <summary>
    /// Transition table
    /// Key: int index of state from
    /// Value: tuple where:
    ///     Item1: State from
    ///     Item2: state to
    /// </summary>
    private Dictionary<int,(int, int)[]> transitionMap;

    private ILogicalState currentState;
    private int currentStateId;

    private void DiscoverStates()
    {
        states = GetComponentsInChildren<ILogicalState>().ToList();
    }
    
    public void Build()
    {
        Registry = ScriptableObject.CreateInstance<StateMachineRegistry>();
        DiscoverStates();
    }

    public void ArbitrateTransitions()
    {
    }
    
    public void SwitchState(int nextStateId)
    {
        if (nextStateId == currentStateId) return;
        ILogicalState lastState = currentState; 
        currentState = states[nextStateId];
        currentStateId = nextStateId;
        if(lastState is IFinalizeState finalizeSate) finalizeSate.Finalize(Registry,this); 
        if(currentState is IInitializeState initializeState) initializeState.Initialize(Registry, this);
    }

    public void Evaluate()
    {
        currentState.Execute(Registry, this);
    }

    private void Awake()
    {
        StateMachineUpdaterFactory.CreateStateMachineUpdater(gameObject, updateMode, Evaluate);
        StateMachineUpdaterFactory.CreateStateMachineUpdater(gameObject, updateMode, ArbitrateTransitions);
    }

    public CharacterData CharacterData { get; set; }
}
