using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionMoveInterpolationTest : MonoBehaviour
{
    public MotionMoveInterpolation motionInterpolation;
    public Transform pos1;
    public Transform pos2;
    // Start is called before the first frame update
    private int dir;
    // Start is called before the first frame update
    void Start()
    {
        dir = -1;
        OnFinishMovement();
    }

    public void OnFinishMovement()
    {
        if (dir > 0)
        {
            //motionInterpolation.SetPositions(pos2.transform.position, pos1.transform.position, -1f);
            motionInterpolation.SetPositions(pos1.transform.position,pos2.transform.position, -1f);
            //motionInterpolation.SetDestination(pos1.transform.position.x - this.transform.position.x, pos1.transform.position.y - this.transform.position.y,-1f);
            //motionInterpolation.SetDestination(pos1.transform.position);
            dir = -1;
        }
        else if (dir < 0)
        {
            //motionInterpolation.SetPositions(pos2.transform.position, pos1.transform.position, 1f);
            motionInterpolation.SetPositions(pos1.transform.position, pos2.transform.position, 1f);
            //motionInterpolation.SetDestination(pos2.transform.position);
            dir = 1;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
