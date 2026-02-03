using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Systems.Movement.StateMachineImplementation.States.WallMovement
{
    [CreateAssetMenu(menuName = "Movement/Wall Movement Data", fileName = "WallMovementData")]
    public class WallMovementData : ScriptableObject
    {
        public InputActionReference moveAction;
        public Vector2Damper inputDamper;
        public float linearSpeed=6f;
        public float stickForce = 8f;
        public float maxDownwardSpeed = 0f;
        [HideInInspector] public Rigidbody rb;
    }
}