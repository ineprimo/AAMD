using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MotionSpeedSelector : MotionSpeedSelectorBase
{
    public void MoveToPositionWith(string id, MotionMovePosition motionMovePosition, Vector3 destination)
    {
        if (!ContainID(id))
            Debug.LogError(id+" no está disponible como acelerador");
        motionMovePosition.MotionSpeed = this[id];
        motionMovePosition.SetDestination(destination);
    }

}
