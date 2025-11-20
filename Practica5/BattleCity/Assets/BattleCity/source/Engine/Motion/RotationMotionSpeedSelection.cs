using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationMotionSpeedSelection : MotionSpeedSelectorBase
{

    public void RotateToAngleWith(string id, MotionRotation motionRotation, Vector3 destination)
    {
        if (!ContainID(id))
            Debug.LogError(id + " no está disponible como acelerador");
        motionRotation.MotionSpeed = this[id];
        motionRotation.SetRotationEnd(destination);
    }

}
