using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineInputAxisController))]
[RequireComponent(typeof(CinemachineOrbitalFollow))]
public class AdditionalCameraController : MonoBehaviour, ICharacterComponent
{
    private CinemachineInputAxisController input;
    private CinemachineOrbitalFollow orbitalFollow;
    void OnFocus()
    {
    }
    public CharacterData CharacterData { get; set; }
}
