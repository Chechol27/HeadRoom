using System;
using StateMachines.Core;
using StateMachines.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Systems.Movement.StateMachineImplementation.States.WallMovement
{
    public class WallMovement : MonoBehaviour,ILogicalState,IInitializeState,IFinalizeState
    {
        [field: SerializeField]
        [field: Expandable]
        [field: CreateScriptableObject]
        public WallMovementData Settings { get; set; }
        
        private PropertyCache onWall;
        private PropertyCache wallNormal;
        
        public Action<StateMachineRegistry, Component> PreInitialize { get; set; }
        public Action<StateMachineRegistry, Component> PostInitialize { get; set; }
        public void Initialize(StateMachineRegistry globalData, Component stateMachine)
        {
            Settings.rb = globalData.Get<Rigidbody>("CharacterRigidbody");

            onWall = globalData.CacheProperty("OnWall");
            wallNormal = globalData.CacheProperty("WallNormal");

            PlayerInput playerInput = stateMachine.GetComponent<PlayerInput>();
            playerInput.onActionTriggered += OnMove;
        }
        private void OnMove(InputAction.CallbackContext ctx)
        {
            if (ctx.action.id != Settings.moveAction.action.id) return;
            Vector2 inputValue = ctx.ReadValue<Vector2>();
            Settings.inputDamper.TargetValue = inputValue;
        }
        
        public Action<StateMachineRegistry, Component> PreExecute { get; set; }
        public Action<StateMachineRegistry, Component> PostExecute { get; set; }
        public void Execute(StateMachineRegistry globalData, Component stateMachine)
        {
            Settings.inputDamper.Update();

            if (!onWall.Get<bool>())
                return; 

            Vector3 n = wallNormal.Get<Vector3>();
            if (n == Vector3.zero) return;

            Transform cam = globalData.Get<Transform>("CameraTransform");

            Vector2 input = Settings.inputDamper.CurrentValue;
            Vector3 forward = Vector3.ProjectOnPlane(cam.forward, Vector3.up).normalized;
            Vector3 right = cam.right;

            Vector3 desired = (forward * input.y + right * input.x);
            desired = Vector3.ProjectOnPlane(desired, Vector3.up);

            Vector3 wallTangentMove = Vector3.ProjectOnPlane(desired, n).normalized;
            Vector3 wallHorizontal = wallTangentMove * (desired.magnitude * Settings.linearSpeed);

            Vector3 stick = -n * Settings.stickForce;

            Vector3 finalHorizontal = wallHorizontal + stick;
            finalHorizontal.y = 0f;
            globalData.Set("HorizontalMotion", finalHorizontal);

            Vector3 v = globalData.Get<Vector3>("VerticalMotion");
            if (v.y < -Mathf.Abs(Settings.maxDownwardSpeed))
                v.y = -Mathf.Abs(Settings.maxDownwardSpeed);
            globalData.Set("VerticalMotion", v);

#if UNITY_EDITOR
            Rigidbody rb = Settings.rb;
            Debug.DrawLine(rb.position, rb.position + (-n), Color.red, 0.05f);
            Debug.DrawLine(rb.position, rb.position + wallTangentMove, Color.yellow, 0.05f);
#endif
        }

        public Action<StateMachineRegistry, Component> PreFinalize { get; set; }
        public Action<StateMachineRegistry, Component> PostFinalize { get; set; }
        public void Finalize(StateMachineRegistry globalData, Component stateMachine)
        {
            PlayerInput playerInput = stateMachine.GetComponent<PlayerInput>();
            playerInput.onActionTriggered -= OnMove;
        }
    }
}