using UnityEngine;
using UnityEngine.InputSystem;

public class VerticalJumpData : ScriptableObject
{
    public enum JumpStateId
    {
        None,
        Up,
        Down
    };
    public InputActionReference actionTrigger;
    public float airTime;
    public float freeFallTime;
    public AnimationCurve upwardsAcceleration;
    public AnimationCurve downwardsAcceleration;
    [HideInInspector]public JumpStateId currentJumpStateId = JumpStateId.None;
    [HideInInspector]public float currentAirTime;
    [HideInInspector]public float currentFreeFallTime;
}