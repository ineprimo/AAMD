using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionSpeedMapped : MotionSpeed
{
    [SerializeField]
    protected float minSpeed;
    [SerializeField]
    protected EasyFunctionType interpolatedMethod;
    public AnimationCurve curve;
    public bool inverse;

    protected float _currentSpeed;
    protected IInterpolation interpolation;

    public float MinSpeed
    {
        get { return minSpeed; }
        set { minSpeed = value; }
    }

    void Awake()
    {
        Stop();
    }

    private void Start()
    {
        interpolation = GetComponent<IInterpolation>();
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

    public float CalculateInterpolation(EasyFunctionType interpolatedMethod, float min, float max, float interpolation)
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
                float c1 = EasyFunctions.CalculateInterpolation(interpolatedMethod, interpolation);
                return EasyFunctions.Scale(c1, min, max);
            case EasyFunctionType.CustomFunction:
                float c2 = CalculatedCustomFunction(interpolation);
                return EasyFunctions.Scale(c2, min, max);
            default:
                return max;
        }
    }

    public float CalculatedCustomFunction(float f)
    {
        return curve.Evaluate(inverse ? 1f-f: f);
    }
    // Update is called once per frame
    void Update()
    {
        _currentSpeed = CalculateInterpolation(interpolatedMethod, minSpeed, maxSpeed, inverse ? 1f - interpolation.Interpolated : interpolation.Interpolated);
    }

}
