using StateMachines.Data;

[UnityEngine.DefaultExecutionOrder(-1)]
public partial class MotionStateMachineRegistry : StateMachineRegistry
{
    void Awake()
    {
        properties.Add(nameof(CharacterRigidbody), new (() => CharacterRigidbody, (obj) => CharacterRigidbody = (UnityEngine.Rigidbody)obj)); 
		properties.Add(nameof(CameraTransform), new (() => CameraTransform, (obj) => CameraTransform = (UnityEngine.Transform)obj)); 
		properties.Add(nameof(HorizontalMotion), new (() => HorizontalMotion, (obj) => HorizontalMotion = (UnityEngine.Vector3)obj)); 
		properties.Add(nameof(VerticalMotion), new (() => VerticalMotion, (obj) => VerticalMotion = (UnityEngine.Vector3)obj)); 
		properties.Add(nameof(MotionVector), new (() => MotionVector, (obj) => MotionVector = (UnityEngine.Vector3)obj)); 
		properties.Add(nameof(Grounded), new (() => Grounded, (obj) => Grounded = (System.Boolean)obj)); 
		properties.Add(nameof(OnWall), new (() => OnWall, (obj) => OnWall = (System.Boolean)obj)); 
    }
}