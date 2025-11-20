using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionSeekAccelArriveSpeed : MotionAccelArriveBase
{
    [SerializeField]
    protected Transform target;
    public override Vector3 Destination
    {
        get { return target.transform.position; }
    }

    public override void SetTarget(Transform t)
    {
        target = t;
    }
}
