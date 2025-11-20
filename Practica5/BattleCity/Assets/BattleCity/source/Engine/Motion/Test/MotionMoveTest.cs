using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionMoveTest : MonoBehaviour
{
    public Transform pos1;
    public Transform pos2;
    public MotionMovePosition movePosition;

    private int dir;
    // Start is called before the first frame update
    void Start()
    {
        dir = -1;
        OnFinishMovement();
    }

    public void OnFinishMovement()
    {
        if(dir > 0)
        {
            movePosition.SetDestination(pos2.transform.position);
            dir = -1;
        }
        else if (dir < 0)
        {
            movePosition.SetDestination(pos1.transform.position);
            dir = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
