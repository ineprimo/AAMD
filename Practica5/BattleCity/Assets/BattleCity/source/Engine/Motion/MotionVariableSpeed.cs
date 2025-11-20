using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MotionVariableSpeed : MotionSpeed
{
    public UnityEvent OnFinishAccel;
    [SerializeField]
    protected float minSpeed;
    [SerializeField]
    protected EasyFunctionType interpolatedMethod;
    public AnimationCurve curve;

    protected float _currentSpeed;
    protected float _currentAccelerationTime;
    protected float _interpolation;

    public float MinSpeed
    {
        get { return minSpeed; }
        set { minSpeed = value; }
    }

    public override float CurrentSpeed
    {
        get { return _currentSpeed; }
    }

    public EasyFunctionType MotionInterpolatedMethod
    {
        get { return interpolatedMethod; }
        set { interpolatedMethod = value; }
    }
    public override void StartAcceleration()
    {
        base.StartAcceleration();
        _currentAccelerationTime = 0f;
    }

    public float CalculateInterpolation(EasyFunctionType interpolatedMethod, float min, float max, float interpolation, AnimationCurve a_curve)
    {
        switch (interpolatedMethod)
        {
            case EasyFunctionType.Linear:
            case EasyFunctionType.EaseInSine:
            case EasyFunctionType.EaseOutSine:
            case EasyFunctionType.EaseInOutSine:
            case EasyFunctionType.EaseInQuad:
            case EasyFunctionType.EaseOutQuad:
            case EasyFunctionType.EaseInOutQuad:
            case EasyFunctionType.EaseInCubic:
            case EasyFunctionType.EaseOutCubic:
            case EasyFunctionType.EaseInOutCubic:
            case EasyFunctionType.EaseInQuart:
            case EasyFunctionType.EaseOutQuart:
            case EasyFunctionType.EaseInOutQuart:
            case EasyFunctionType.EaseInQuint:
            case EasyFunctionType.EaseOutQuint:
            case EasyFunctionType.EaseInOutQuint:
            case EasyFunctionType.EaseInExpo:
            case EasyFunctionType.EaseOutExpo:
            case EasyFunctionType.EaseInOutExpo:
            case EasyFunctionType.EaseInCirc:
            case EasyFunctionType.EaseOutCirc:
            case EasyFunctionType.EaseInOutCirc:
            case EasyFunctionType.EaseInBack:
            case EasyFunctionType.EaseOutBack:
            case EasyFunctionType.EaseInOutBack:
            case EasyFunctionType.EaseInElastic:
            case EasyFunctionType.EaseOutElastic:
            case EasyFunctionType.EaseInOutElastic:
            case EasyFunctionType.EaseInBounce:
            case EasyFunctionType.EaseOutBounce:
            case EasyFunctionType.EaseInOutBounce:
                float inter= EasyFunctions.CalculateInterpolation(interpolatedMethod, min, max, interpolation);
                return inter;
            case EasyFunctionType.CustomFunction:
                float c = a_curve.Evaluate(interpolation);
                float scale = EasyFunctions.Scale(c, min, max);
                return scale;
            default:
                return max;
        }
    }

    void Awake()
    {
        Stop();
    }

    protected virtual void FinishAccel()
    {
        Stop();
        OnFinishAccel?.Invoke();
    }
}
