using System;
using StateMachines.Data;
using StateMachines.Decorators;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class GroundCheck : StateDecorator
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float casterRadius;
    [SerializeField] private float rayCount;
    [SerializeField] private float groundedThreshold;
    [SerializeField] private float rayLength;
    [SerializeField] private LayerMask castMask;
    private int positiveCount;

    private bool ArbitrateGroundedState()
    {
        return positiveCount > groundedThreshold;
    }
    
    private void EvaluateGroundCollision(StateMachineRegistry registry)
    {
        float angleStep = 360.0f / (float)rayCount;
        positiveCount = 0;
        for (float i = 0; i < rayCount; i++)
        {
            Vector3 rayOrigin = Quaternion.AngleAxis(angleStep * i, -targetTransform.up) * targetTransform.forward * casterRadius;
            Ray r = new Ray(targetTransform.position + rayOrigin, -targetTransform.up);
            if (Physics.Raycast(r, out RaycastHit hit, rayLength, castMask))
            {
#if UNITY_EDITOR
                Debug.DrawLine(r.origin, r.origin + r.direction * rayLength, Color.green);
#endif
                positiveCount++;
            }
            else
            {
#if UNITY_EDITOR
                Debug.DrawLine(r.origin, r.origin + r.direction * rayLength, Color.red);
#endif
            }
        }

        registry.Set("Grounded", ArbitrateGroundedState());
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        rayCount = Mathf.Max(1, rayCount);
        groundedThreshold = Mathf.Clamp(groundedThreshold, 1, rayCount);
    }
#endif

    public CharacterData CharacterData { get; set; }
    
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = positiveCount > 0 ? Color.green : Color.red;
        Handles.DrawWireDisc(targetTransform.position, -targetTransform.up, casterRadius);
    }
    #endif
    public override void Execute(StateMachineRegistry registry, Component stateMachine)
    {
        EvaluateGroundCollision(registry);
    }
}
