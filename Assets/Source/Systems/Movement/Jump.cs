using System;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(PlayerInput))]
public class Jump : MonoBehaviour, ICharacterComponent
{
    private enum JumpState
    {
        None,
        Up,
        Down
    };
    
    [SerializeField] float airTime;
    [SerializeField] float freeFallTime;
    [SerializeField] private AnimationCurve upwardsAcceleration;
    [SerializeField] private AnimationCurve downwardsAcceleration;

    [SerializeField] private JumpState currentJumpState = JumpState.None;
    private float currentAirTime;
    private float currentFreeFallTime;
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (!gameObject.activeInHierarchy) return;
        if (ctx.started)
        {
            SetJumpingState(JumpState.Up);    
        }
        if (ctx.canceled)
        {
            SetJumpingState(JumpState.Down);
        }
    }

    void SetJumpingState(JumpState jumpState)
    {
        if (currentJumpState == jumpState) return;
        switch (jumpState)
        {
            case JumpState.Up:
                if (!Motion.grounded) return;
                currentAirTime = 0;
                currentJumpState = JumpState.Up;
                break;
            case JumpState.Down:
                currentFreeFallTime = 0;
                currentJumpState = JumpState.Down;
                break;
            default:
                currentJumpState = JumpState.None;
                break;
        }
    }

    private void AnimateJump()
    {
        switch (currentJumpState)
        {
            case JumpState.None:
                Motion.verticalMotion = Vector3.up * downwardsAcceleration.Evaluate(.5f);
                if (!Motion.grounded)
                {
                    SetJumpingState(JumpState.Down);
                }
                break;
            case JumpState.Up:
                currentAirTime += Time.fixedDeltaTime;
                Motion.verticalMotion = Vector3.up * upwardsAcceleration.Evaluate(currentAirTime / airTime);
                if (currentAirTime > airTime)
                {
                    SetJumpingState(JumpState.Down);
                    currentAirTime = 0;
                }
                break;
            case JumpState.Down:
                currentFreeFallTime += Motion.grounded ? 0 : Time.fixedDeltaTime;
                Motion.verticalMotion = Vector3.up * downwardsAcceleration.Evaluate(currentFreeFallTime / freeFallTime);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void FixedUpdate()
    {
        AnimateJump();
    }

    public CharacterData CharacterData { get; set; }
    private CharacterMotionData Motion => CharacterData.motion;
}
