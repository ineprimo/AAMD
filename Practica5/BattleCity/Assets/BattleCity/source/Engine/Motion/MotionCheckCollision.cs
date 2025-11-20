using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionCheckCollision : MonoBehaviour
{

    protected float collisionRadious = -1f;
    protected int collisionLayerMask = 0;

    public void StopWhenCollision(float rad, int layerMask)
    {
        collisionRadious = rad;
        collisionLayerMask = layerMask;
    }

    protected void UpdateCollision(float time)
    {
        if (collisionRadious > 0f)
            CheckCollision(time);
    }

    protected void CheckCollision(float time)
    {
        Collider[] clliders = Physics.OverlapSphere(this.transform.position, collisionRadious, collisionLayerMask);
        if (clliders != null && clliders.Length > 0)
        {
            EndMovement();
        }
    }

    protected virtual void EndMovement()
    {
        enabled = false;
    }
}
