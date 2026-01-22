using System;
using StateMachines.Data;
using UnityEngine;

public partial class MotionStateMachineRegistry : StateMachineRegistry
{
    [field:SerializeField][StateMachineProperty] public Rigidbody CharacterRigidbody { get; set; }
    [field:SerializeField][StateMachineProperty] public Transform CameraTransform { get; set; }
    [field:SerializeField][StateMachineProperty] public Vector3 HorizontalMotion { get; set; }
    [field:SerializeField][StateMachineProperty] public Vector3 VerticalMotion { get; set; }
    [field:SerializeField][StateMachineProperty] public Vector3 MotionVector { get; set; }
    [field:SerializeField][StateMachineProperty] public bool Grounded { get; set; }
    [field:SerializeField][StateMachineProperty] public bool OnWall { get; set; }
}
