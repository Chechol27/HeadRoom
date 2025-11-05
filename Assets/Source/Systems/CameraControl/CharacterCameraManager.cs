using System;
using UnityEngine;

public class CharacterCameraManager : MonoBehaviour, ICharacterComponent
{
    [SerializeField] private Camera camera;

    private void Awake()
    {
        CharacterData.camera = camera;
    }

    public CharacterData CharacterData { get; set; }
}
