using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MotionAccelerationSpeed : MotionVariableSpeed
{

    [SerializeField]
    protected float accelerationTime;


    public float AccelerationTime
    {
        get { return accelerationTime; }
        set { accelerationTime = value; }
    }


    public void SetAccelParameters(float a_minSpeed, float a_maxSpeed, float interTime, EasyFunctionType mim)
    {
        SetMotionParameters( a_minSpeed, a_maxSpeed, interTime);
        interpolatedMethod = mim;
    }

    public void SetMotionParameters(float a_minSpeed, float a_maxSpeed, float acceltime)
    {
        minSpeed = a_minSpeed;
        maxSpeed = a_maxSpeed;
        accelerationTime = acceltime;
    }

    public override void StartAcceleration()
    {
        base.StartAcceleration();
        _interpolation = 0f;
        _currentSpeed = minSpeed;
    }

    


    public virtual void CalculateCurrentSpeed(float time)
    {
        _currentSpeed = CalculateInterpolation(interpolatedMethod,minSpeed, maxSpeed,_interpolation,curve);
        _currentAccelerationTime += time;

        if (_currentAccelerationTime > accelerationTime)
        {
            _interpolation = 1f;
            _currentSpeed = maxSpeed;
            FinishAccel();
        }
        else if(interpolatedMethod == EasyFunctionType.Constant)
            FinishAccel();
        else
            _interpolation = _currentAccelerationTime / accelerationTime;
    }
    // Start is called before the first frame update




    // Update is called once per frame
    void Update()
    {
        CalculateCurrentSpeed(Time.deltaTime);
        //Debug.Log(" Speed " + CurrentSpeed);
    }
}
