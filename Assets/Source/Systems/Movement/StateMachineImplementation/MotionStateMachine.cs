using System.Collections.Generic;
using StateMachines.Core;
using StateMachines.Core.Update;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(MotionStateMachineRegistry))]
public class MotionStateMachine : MonoBehaviour, IStateMachine, ICharacterComponent
{
    [SerializeField]
    [Tooltip("Only relevant at awake")]
    StateMachineUpdateMode updateMode;
    
    public MotionStateMachineRegistry Registry { get; private set; }

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
                if (state is IInitializeState initializeState)
                {
                    initializeState.RegisterDecorators(t);
                }
                state.RegisterDecorators(t);
                if (state is IFinalizeState finalizeState)
                {
                    finalizeState.RegisterDecorators(t);
                    
                }
                states.Add(state);
                Debug.Log(t.gameObject);
            }
        }
        
        SwitchState(0);
    }

    private void ApplyMotion()
    {
        Rigidbody characterRb = Registry.CharacterRigidbody;
        Vector3 horizontalMotion = Registry.HorizontalMotion;
        Vector3 verticalMotion = Registry.VerticalMotion;
        Vector3 motionVector = new Vector3(horizontalMotion.x, verticalMotion.y,
            horizontalMotion.z);
        Registry.MotionVector = motionVector;

        characterRb.linearVelocity = motionVector;
        
        Vector3 lookVector = Vector3.ProjectOnPlane(motionVector, Vector3.up);
        Quaternion q = Quaternion.FromToRotation(characterRb.transform.forward, lookVector.normalized);
        characterRb.rotation *= Quaternion.Slerp(Quaternion.identity, q, lookVector.magnitude * Time.fixedDeltaTime * 10.0f);
    }
    
    public void SwitchState(int nextStateId)
    {
        if (nextStateId == currentStateId) return;
        ILogicalState lastState = currentState;
        currentState = states[nextStateId];
        currentStateId = nextStateId;
        if (lastState is IFinalizeState finalizeSate)
        {
            finalizeSate.Finalize(Registry,this);
        }

        if (currentState is IInitializeState initializeState)
        {
            initializeState.Initialize(Registry, this);
        }
    }

    public void SwitchState(ILogicalState targetState)
    {
        if (!states.Contains(targetState)) return;
        SwitchState(states.IndexOf(targetState));
    }

    public void Evaluate()
    {
        currentState?.PreExecute?.Invoke(Registry, this);
        currentState?.Execute(Registry, this);
        currentState?.PostExecute?.Invoke(Registry, this);
        ApplyMotion();
    }

    private void Awake()
    {
        Registry = GetComponent<MotionStateMachineRegistry>();
        DiscoverStates();
        StateMachineUpdaterFactory.CreateStateMachineUpdater(gameObject, updateMode, Evaluate);
    }

    public CharacterData CharacterData { get; set; }
}
