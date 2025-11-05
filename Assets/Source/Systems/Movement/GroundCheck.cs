using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class GroundCheck : MonoBehaviour, ICharacterComponent
{
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
    
    private void EvaluateGroundCollision()
    {
        float angleStep = 360.0f / (float)rayCount;
        positiveCount = 0;
        for (float i = 0; i < rayCount; i++)
        {
            Vector3 rayOrigin = Quaternion.AngleAxis(angleStep * i, transform.forward) * transform.up * casterRadius;
            Ray r = new Ray(transform.position + rayOrigin, transform.forward);
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

        CharacterData.motion.grounded = ArbitrateGroundedState();
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        rayCount = Mathf.Max(1, rayCount);
        groundedThreshold = Mathf.Clamp(groundedThreshold, 1, rayCount);
    }
#endif
    
    private void Update()
    {
        EvaluateGroundCollision();
    }

    public CharacterData CharacterData { get; set; }
    
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = positiveCount > 0 ? Color.green : Color.red;
        Handles.DrawWireDisc(transform.position, transform.forward, casterRadius);
    }
    #endif
}
