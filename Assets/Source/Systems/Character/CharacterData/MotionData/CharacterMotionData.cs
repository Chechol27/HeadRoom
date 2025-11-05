using UnityEngine;

[CreateAssetMenu(fileName = "CharacterMotionData", menuName = "Scriptable Objects/Character/MotionData")]
public class CharacterMotionData : ScriptableObject
{
    public Transform cameraTransform;
    public bool grounded;
    public Vector3 horizontalMotion;
    public Vector3 verticalMotion;
    public Vector3 motionVector;
}
