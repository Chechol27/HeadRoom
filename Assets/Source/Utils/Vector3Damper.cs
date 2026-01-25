using UnityEngine;

public struct Vector3Damper
{
    public Vector3 CurrentValue { get; private set; }
    public Vector3 TargetValue { get; set; }
    private Vector3 currentVelocity;
    [field:SerializeField] public float SmoothingTime { get; set; }
    
    public void Update()
    {
        CurrentValue = Vector3.SmoothDamp(CurrentValue, TargetValue, ref currentVelocity, SmoothingTime);
    }
}
