using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionMoveInterpolationCController : MotionMoveInterpolation
{
    public CharacterController characterController;
    /*public override float XAxis
    {
        set
        {
            if (xAxisMapping == AxisMapping.X)
                characterController.Move(new Vector3(value, 0f, 0f));
            else if (xAxisMapping == AxisMapping.Y)
                characterController.Move(new Vector3(0f, value,0f));
            else
                characterController.Move(new Vector3(0f, 0f, value));
        }

    }*/

    /*public override float YAxis
    {
        set
        {
            if (yAxisMapping == AxisMapping.X)
                characterController.Move( new Vector3(value, 0f, 0f));
            else if (yAxisMapping == AxisMapping.Y)
                characterController.Move( new Vector3(0f, value, 0f));
            else
                characterController.Move( new Vector3(0f, 0f, value));
        }

    }*/
}
