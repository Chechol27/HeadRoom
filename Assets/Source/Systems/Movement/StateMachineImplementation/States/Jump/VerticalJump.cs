using System;
using StateMachines.Core;
using StateMachines.Data;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A jump implementation for platformers
/// Features:
///     Fully controllable fall acceleration
///     Fully controllable upwards acceleration
/// </summary>
public class VerticalJump : 
    MonoBehaviour,
    ILogicalState, 
    IInitializeState, 
    IFinalizeState
{
    [field:SerializeField]
    [field:Expandable]
    [field:CreateScriptableObject]
    public VerticalJumpData Settings { get; set; }

    public Action<StateMachineRegistry, Component> PreInitialize { get; set; }
    public Action<StateMachineRegistry, Component> PostInitialize { get; set; }

    private PropertyCache grounded;

    public void Initialize(StateMachineRegistry globalData, Component stateMachine)
    {
        PlayerInput playerInput = stateMachine.GetComponent<PlayerInput>();
        playerInput.onActionTriggered += OnJump;
        grounded = globalData.CacheProperty("Grounded");
    }
    void SetJumpingState(VerticalJumpData.JumpStateId jumpState)
    {
        if (Settings.currentJumpStateId == jumpState) return;
        switch (jumpState)
        {
            case VerticalJumpData.JumpStateId.Up:
                Settings.currentAirTime = 0;
                Settings.currentJumpStateId = VerticalJumpData.JumpStateId.Up;
                break;
            case VerticalJumpData.JumpStateId.Down:
                Settings.currentFreeFallTime = 0;
                Settings.currentJumpStateId = VerticalJumpData.JumpStateId.Down;
                break;
            default:
                Settings.currentJumpStateId = VerticalJumpData.JumpStateId.None;
                break;
        }
    }
    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.action.id != Settings.actionTrigger.action.id) return;
        if (ctx.started)
        {
            if (!grounded.Get<bool>()) return;
            SetJumpingState(VerticalJumpData.JumpStateId.Up);    
        }
        if (ctx.canceled)
        {
            SetJumpingState(VerticalJumpData.JumpStateId.Down);
        }
    }
    private void AnimateJump(StateMachineRegistry globalData)
    {
        Vector3 verticalMotion = Vector3.zero;
        switch (Settings.currentJumpStateId)
        {
            case VerticalJumpData.JumpStateId.None:
                verticalMotion = Vector3.up * Settings.downwardsAcceleration.Evaluate(.5f);
                if (!grounded.Get<bool>())
                {
                    SetJumpingState(VerticalJumpData.JumpStateId.Down);
                }
                break;
            case VerticalJumpData.JumpStateId.Up:
                Settings.currentAirTime += Time.fixedDeltaTime;
                verticalMotion = Vector3.up * Settings.upwardsAcceleration.Evaluate(Settings.currentAirTime / Settings.airTime);
                if (Settings.currentAirTime > Settings.airTime)
                {
                    SetJumpingState(VerticalJumpData.JumpStateId.Down);
                    Settings.currentAirTime = 0;
                }
                break;
            case VerticalJumpData.JumpStateId.Down:
                Settings.currentFreeFallTime += grounded.Get<bool>() ? 0 : Time.fixedDeltaTime;
                verticalMotion = Vector3.up * Settings.downwardsAcceleration.Evaluate(Settings.currentFreeFallTime / Settings.freeFallTime);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        globalData.Set("VerticalMotion", verticalMotion);
    }

    public Action<StateMachineRegistry, Component> PreExecute { get; set; }
    public Action<StateMachineRegistry, Component> PostExecute { get; set; }

    public void Execute(StateMachineRegistry globalData, Component stateMachine)
    {
        AnimateJump(globalData);
    }

    public Action<StateMachineRegistry, Component> PreFinalize { get; set; }
    public Action<StateMachineRegistry, Component> PostFinalize { get; set; }

    public void Finalize(StateMachineRegistry globalData, Component stateMachine)
    {
        PlayerInput playerInput = stateMachine.GetComponent<PlayerInput>();
        playerInput.onActionTriggered -= OnJump;
    }
    
}
