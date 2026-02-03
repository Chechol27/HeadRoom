using StateMachines.Data;
using StateMachines.Decorators;
using UnityEditor;
using UnityEngine;

namespace Source.Systems.Movement
{
    public class WallCheck : StateDecorator
    {
        [SerializeField] private Transform targetTransform;

        [Header("Cast")] [SerializeField] private float casterRadius = 0.35f;
        [SerializeField] private int rayCount = 8;
        [SerializeField] private float rayLength = 0.6f;
        [SerializeField] private LayerMask castMask;

        [Header("Filter")]
        //Lo agrego por si tenemos muros que no queremos escalar con cierta inclinacion
        [SerializeField]
        private float maxUpDot = 0.2f;

        private int positiveCount;
        private Vector3 accumulatedNormal;

        public override void Execute(StateMachineRegistry registry, Component stateMachine)
        {
            EvaluateWallCollision(registry);
        }
        private void EvaluateWallCollision(StateMachineRegistry registry)
        {
            float angleStep = 360f / rayCount;

            positiveCount = 0;
            accumulatedNormal = Vector3.zero;

            Vector3 up = targetTransform.up;

            for (int i = 0; i < rayCount; i++)
            {
                Vector3 dir = Quaternion.AngleAxis(angleStep * i, up) * targetTransform.forward;
                dir = Vector3.ProjectOnPlane(dir, up).normalized;

                Vector3 origin = targetTransform.position + dir * casterRadius;
                Ray r = new Ray(origin, dir);

                if (Physics.Raycast(r, out RaycastHit hit, rayLength, castMask))
                {
                    float upDot = Mathf.Abs(Vector3.Dot(hit.normal.normalized, up));
                    if (upDot <= maxUpDot)
                    {
                        positiveCount++;
                        accumulatedNormal += hit.normal;

#if UNITY_EDITOR
                        Debug.DrawLine(r.origin, r.origin + r.direction * rayLength, Color.magenta);
#endif
                    }
#if UNITY_EDITOR
                    else
                    {
                        Debug.DrawLine(r.origin, r.origin + r.direction * rayLength, Color.gray);
                    }
#endif
                }
#if UNITY_EDITOR
                else
                {
                    Debug.DrawLine(r.origin, r.origin + r.direction * rayLength, Color.black);
                }
#endif
            }

            bool onWall = positiveCount > 0;
            registry.Set("OnWall", onWall);

            if (onWall)
            {
                Vector3 n = accumulatedNormal.normalized;
                registry.Set("WallNormal", n);
            }
            else
            {
                registry.Set("WallNormal", Vector3.zero);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            rayCount = Mathf.Max(1, rayCount);
            rayLength = Mathf.Max(0.01f, rayLength);
            casterRadius = Mathf.Max(0.01f, casterRadius);
            maxUpDot = Mathf.Clamp01(maxUpDot);
        }
        private void OnDrawGizmos()
        {
            if (targetTransform == null) return;
            Handles.color = positiveCount > 0 ? Color.magenta : Color.gray;
            Handles.DrawWireDisc(targetTransform.position, targetTransform.up, casterRadius);
        }
#endif
    }
}