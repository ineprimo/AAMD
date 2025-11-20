using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionMovePositionCController : MotionMovePosition
{
    public CharacterController characterController;
    public float threshold = 0.1f;
    public override void Move(float time)
    {
        characterController.Move(_direction * CurrentSpeed * time);
        if (Utils.ArrivedAtPoint(_direction, this.transform.position, destination, threshold))
        {
            this.transform.position = destination;
            EndMovement();
        }
    }
}
