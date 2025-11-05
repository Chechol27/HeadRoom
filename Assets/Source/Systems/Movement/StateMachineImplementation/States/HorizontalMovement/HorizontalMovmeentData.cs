using UnityEngine;
using UnityEngine.InputSystem;

public class HorizontalMovementData : ScriptableObject
{
    public InputActionReference actionTriggerAsset;
    [HideInInspector] public Rigidbody rb;
    public Vector2Damper inputDamper;
    public float linearSpeed;
}
