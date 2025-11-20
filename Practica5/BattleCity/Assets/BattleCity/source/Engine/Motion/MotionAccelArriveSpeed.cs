using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MotionAccelArriveSpeed : MotionAccelArriveBase
{
    protected Vector3 destination;

    public override void SetDestination(Vector3 v)
    {
        destination = v;
    }

    public override Vector3 Destination
    {
        get { return destination; }
    }
}
