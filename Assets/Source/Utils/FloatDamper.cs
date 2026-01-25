using UnityEngine;

public struct FloatDamper
{
    public float CurrentValue { get; private set; }
    public float TargetValue { get; set; }
    private float currentVelocity;
    [field:SerializeField] public float SmoothingTime { get; set; }
    
    public void Update()
    {
        CurrentValue = Mathf.SmoothDamp(CurrentValue, TargetValue, ref currentVelocity, SmoothingTime);
    }
}