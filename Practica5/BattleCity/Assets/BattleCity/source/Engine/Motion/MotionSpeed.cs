using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionSpeed : MonoBehaviour
{
    [SerializeField]
    protected float maxSpeed;
    public float MaxSpeed
    {
        get { return maxSpeed; }
        set { maxSpeed = value; }
    }

    public virtual void SetDestination(Vector3 position)
    {
        Debug.LogError("El MotionSpeed " + this.GetType().Name + " no tiene implementado el método SetDestination. Quizas necesites usar otro MotionSpeed");
    }

    public virtual void SetTarget(Transform t)
    {
        Debug.LogError("El MotionSpeed " + this.GetType().Name + " no tiene implementado el método SetTarget. Quizas necesites usar otro MotionSpeed");
    }


    public virtual void StartAcceleration()
    {
        enabled = true;
    }

    public virtual float CurrentSpeed
    {
        get { return maxSpeed; }
    }
    // Start is called before the first frame update

    public void Stop()
    {
        enabled = false;
    }

}
