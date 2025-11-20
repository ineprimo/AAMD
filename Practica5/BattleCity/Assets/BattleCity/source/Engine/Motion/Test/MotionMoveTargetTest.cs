using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionMoveTargetTest : MonoBehaviour
{
    public Transform target;
    public MotionSeek moveSeek;
    public float distanceToMove;

    private bool stop = false;
    // Start is called before the first frame update
    void Start()
    {
        moveSeek.SetTarget(target);
        stop = false;
    }

    public void OnFinishMovement()
    {
        stop = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(stop)
        {
            Vector3 direction = target.position - this.transform.position;
            if(direction.sqrMagnitude > distanceToMove* distanceToMove)
            {
                moveSeek.Init();
                stop = false;
            }
        }
    }
}
