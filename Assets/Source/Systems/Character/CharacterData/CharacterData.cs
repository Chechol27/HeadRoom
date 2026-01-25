using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    public Camera camera;
    public CharacterMotionData motion;

    private void Awake()
    {
        motion = ScriptableObject.CreateInstance<CharacterMotionData>();
    }
}
