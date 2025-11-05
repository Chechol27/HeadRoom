using UnityEngine;

public class CharacterDataHolder : MonoBehaviour
{
    [SerializeField] private CharacterData characterData;

    private void Awake()
    {
        characterData = ScriptableObject.CreateInstance<CharacterData>();
        foreach (ICharacterComponent charaComponent in GetComponentsInChildren<ICharacterComponent>())
        {
            charaComponent.CharacterData = characterData;
        }
    }
}
