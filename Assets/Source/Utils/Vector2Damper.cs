using System;
using UnityEngine;


[Serializable]
public struct Vector2Damper
{
    public Vector2 CurrentValue { get; private set; }
    public Vector2 TargetValue { get; set; }
    private Vector2 currentVelocity;
    [field:SerializeField] public float SmoothingTime { get; set; }
    
    public void Update()
    {
        CurrentValue = Vector2.SmoothDamp(CurrentValue, TargetValue, ref currentVelocity, SmoothingTime);
    }
}
