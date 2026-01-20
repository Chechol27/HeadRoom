using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A jump implementation for platformers
/// Features:
///     Fully controllable fall acceleration
///     Fully controllable upwards acceleration
///     Coyote time 
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
    public void Initialize(StateMachineRegistry globalData, Component stateMachine)
    {
        PlayerInput playerInput = stateMachine.GetComponent<PlayerInput>();
        playerInput.onActionTriggered += OnJump;
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
            SetJumpingState(VerticalJumpData.JumpStateId.Up);    
        }
        if (ctx.canceled)
        {
            SetJumpingState(VerticalJumpData.JumpStateId.Down);
        }
    }
    private void AnimateJump(StateMachineRegistry globalData)
    {
        bool grounded = globalData.Get<bool>("Grounded");
        Vector3 verticalMotion = Vector3.zero;
        switch (Settings.currentJumpStateId)
        {
            case VerticalJumpData.JumpStateId.None:
                verticalMotion = Vector3.up * Settings.downwardsAcceleration.Evaluate(.5f);
                if (!grounded)
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
                Settings.currentFreeFallTime += grounded ? 0 : Time.fixedDeltaTime;
                verticalMotion = Vector3.up * Settings.downwardsAcceleration.Evaluate(Settings.currentFreeFallTime / Settings.freeFallTime);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        globalData.Set("VerticalMotion", verticalMotion);
    }
    public void Execute(StateMachineRegistry globalData, Component stateMachine)
    {
        AnimateJump(globalData);
    }
    public void Finalize(StateMachineRegistry globalData, Component stateMachine)
    {
        PlayerInput playerInput = stateMachine.GetComponent<PlayerInput>();
        playerInput.onActionTriggered -= OnJump;
    }
    
}
