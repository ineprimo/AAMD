using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MotionDecelerationSpeed : MotionVariableSpeed
{
    [SerializeField]
    protected float decelerationTime;

    

    public float DecelerationTime
    {
        get { return decelerationTime; }
        set { decelerationTime = value; }
    }

    public void SetDecelParameters(float a_minSpeed, float a_maxSpeed, float interTime, EasyFunctionType mim)
    {
        SetMotionParameters(a_minSpeed, a_maxSpeed, interTime);
        interpolatedMethod = mim;
    }

    public void SetMotionParameters(float a_minSpeed, float a_maxSpeed, float decelTime)
    {
        minSpeed = a_minSpeed;
        maxSpeed = a_maxSpeed;
        decelerationTime = decelTime;
    }

    public override void StartAcceleration()
    {
        base.StartAcceleration();
        _interpolation = 1f;
        _currentSpeed = maxSpeed;
    }


    public virtual void CalculateCurrentSpeed(float time)
    {
        _currentSpeed = CalculateInterpolation(interpolatedMethod, minSpeed, maxSpeed, _interpolation, curve);
        _currentAccelerationTime += time;

        if (_currentAccelerationTime > decelerationTime)
        {
            _interpolation = 0f;
            _currentSpeed = minSpeed;
            FinishAccel();
        }
        else if (interpolatedMethod == EasyFunctionType.Constant)
            FinishAccel();
        else
            _interpolation = 1f-(_currentAccelerationTime / decelerationTime);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateCurrentSpeed(Time.deltaTime);
    }
}
