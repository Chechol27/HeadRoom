using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MotionSolver : MonoBehaviour, ICharacterComponent
{
    private Rigidbody rb;
    private Vector3 motionVector;
    void SolveMotionVector()
    {
        CharacterMotionData motion = CharacterData.motion;
        motionVector = new Vector3(motion.horizontalMotion.x, motion.verticalMotion.y,
            motion.horizontalMotion.z);
        motion.motionVector = motionVector;

        rb.linearVelocity = motionVector;
    }

    void SolveRotation()
    {
        Vector3 lookVector = Vector3.ProjectOnPlane(motionVector, Vector3.up);
        Quaternion q = Quaternion.FromToRotation(transform.forward, lookVector.normalized);
        rb.rotation *= Quaternion.Slerp(Quaternion.identity, q, lookVector.magnitude * Time.deltaTime * 10.0f);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        SolveMotionVector();
        SolveRotation();
    }

    public CharacterData CharacterData { get; set; }
}
