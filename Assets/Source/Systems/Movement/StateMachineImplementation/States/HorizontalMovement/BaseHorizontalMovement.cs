using UnityEngine;
using UnityEngine.InputSystem;


public class BaseHorizontalMovement :
    MonoBehaviour,
    ILogicalState, 
    IInitializeState, 
    IFinalizeState
{
    [field:SerializeField]
    [field:Expandable]
    [field:CreateScriptableObject]
    public HorizontalMovementData Settings { get; set; }
    public void Initialize(StateMachineRegistry globalData, Component stateMachine)
    {
        Settings.rb = globalData.Get<Rigidbody>("CharacterRigidbody");
        PlayerInput playerInput = stateMachine.GetComponent<PlayerInput>();
        playerInput.onActionTriggered += OnMove;
    }
    
    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.action.id != Settings.actionTriggerAsset.action.id) return;
        Vector2 inputValue = ctx.ReadValue<Vector2>();
        Settings.inputDamper.TargetValue = inputValue;
    }
    
    public void SolveMotion(StateMachineRegistry globalData)
    {
        Transform cameraTransform = globalData.Get<Transform>("CameraTransform");
        Vector3 projectVector = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up) ;
        Vector2 inputMotion = Settings.inputDamper.CurrentValue;
        Vector3 motionVector = projectVector.normalized * inputMotion.y + cameraTransform.right * inputMotion.x;
        globalData.Set("HorizontalMotion", motionVector * Settings.linearSpeed);
        Rigidbody rb = Settings.rb;
        Debug.DrawLine(rb.position, rb.position + motionVector, Color.cyan, 1.0f);
        Debug.DrawLine(rb.position, rb.position + projectVector, Color.blue);
    }
    
    public void Execute(StateMachineRegistry globalData, Component stateMachine)
    {
        Settings.inputDamper.Update();
        SolveMotion(globalData);
    }

    public void Finalize(StateMachineRegistry globalData, Component stateMachine)
    {
        PlayerInput playerInput = stateMachine.GetComponent<PlayerInput>();
        playerInput.onActionTriggered -= OnMove;
    }

}
