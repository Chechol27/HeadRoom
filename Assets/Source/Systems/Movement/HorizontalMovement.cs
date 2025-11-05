using UnityEngine;
using UnityEngine.InputSystem;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
public class HorizontalMovement : MonoBehaviour, ICharacterComponent
{
    private Rigidbody rb;

    [SerializeField] private Vector2Damper inputDamper;
    [SerializeField] private float linearSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(CallbackContext ctx)
    {
        Vector2 inputValue = ctx.ReadValue<Vector2>();
        inputDamper.TargetValue = inputValue;
    }

    public void SolveMotion()
    {
        Transform cameraTransform = CharacterData.camera.transform;
        Vector3 projectVector =Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up) ;
        Vector2 inputMotion = inputDamper.CurrentValue;
        Vector3 motionVector = projectVector.normalized * inputMotion.y + cameraTransform.right * inputMotion.x;
        CharacterData.motion.horizontalMotion = motionVector * linearSpeed;
        Debug.DrawLine(rb.position, rb.position + motionVector, Color.cyan, 1.0f);
        Debug.DrawLine(rb.position, rb.position + projectVector, Color.blue);
    }
    
    private void FixedUpdate()
    {
        inputDamper.Update();
        SolveMotion();
    }

    public CharacterData CharacterData { get; set; }
}
