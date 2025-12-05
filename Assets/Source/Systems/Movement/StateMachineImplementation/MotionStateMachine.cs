using System.Collections.Generic;
using System.Linq;
using StateMachines.Core.Update;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class MotionStateMachine : MonoBehaviour, IStateMachine, ICharacterComponent
{
    [SerializeField]
    [Tooltip("Only relevant at awake")]
    StateMachineUpdateMode updateMode;

    [field:SerializeField]
    [field:Expandable]
    [field:CreateScriptableObject]
    public StateMachineRegistry Registry { get; private set; }

    private List<ILogicalState> states = new();

    private ILogicalState currentState;
    private int currentStateId = -1;

    private void DiscoverStates()
    {
        states.Clear();
        foreach (Transform t in transform)
        {
            if (t.TryGetComponent(out ILogicalState state))
            {
                states.Add(state);
                Debug.Log(t.gameObject);
            }
        }
        
        SwitchState(0);
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

    public void SwitchState(ILogicalState targetState)
    {
        if (!states.Contains(targetState)) return;
        SwitchState(states.IndexOf(targetState));
    }

    public void Evaluate()
    {
        currentState?.Execute(Registry, this);
        Debug.Log($"Current State: {currentState == null}");
    }

    private void Awake()
    {
        DiscoverStates();
        StateMachineUpdaterFactory.CreateStateMachineUpdater(gameObject, updateMode, Evaluate);
    }

    public CharacterData CharacterData { get; set; }
}
