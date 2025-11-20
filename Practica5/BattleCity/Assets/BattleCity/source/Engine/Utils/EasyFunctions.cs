using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EasyFunctionType
{
    Constant, Linear, EaseInSine, EaseOutSine, EaseInOutSine,
    EaseInQuad, EaseOutQuad, EaseInOutQuad, EaseInCubic, EaseOutCubic, EaseInOutCubic,
    EaseInQuart, EaseOutQuart, EaseInOutQuart, EaseInQuint, EaseOutQuint, EaseInOutQuint,
    EaseInExpo, EaseOutExpo, EaseInOutExpo, EaseInCirc, EaseOutCirc, EaseInOutCirc,
    EaseInBack, EaseOutBack, EaseInOutBack, EaseInElastic, EaseOutElastic, EaseInOutElastic,
    EaseInBounce, EaseOutBounce, EaseInOutBounce, CustomFunction
}

public class EasyFunctions
{
    public static float Transform(float f, float min, float max)
    {
        return (f + min) * (max - min);
    }
    public static float Scale(float f, float min, float max)
    {
        return Mathf.Lerp(min, max, f);
    }
    // Sin functions
    public static float EaseInSine(float x)
    {
        return 1 - Mathf.Cos((x* Mathf.PI) / 2);
    }

    public static float CalculateInterpolation(EasyFunctionType interpolatedMethod,float interpolation)
    {
        float y = 0f;
        switch (interpolatedMethod)
        {
            case EasyFunctionType.Linear:
                y = Mathf.Lerp(0f, 1f, interpolation);
                break;
            case EasyFunctionType.EaseInSine:
                y = EaseInSine(interpolation);
                break;
            case EasyFunctionType.EaseOutSine:
                y = EaseOutSine(interpolation);
                break;
            case EasyFunctionType.EaseInOutSine:
                y = EaseInOutSine(interpolation);
                break;
            case EasyFunctionType.EaseInQuad:
                y = EaseInQuad(interpolation);
                break;
            case EasyFunctionType.EaseOutQuad:
                y = EaseOutQuad(interpolation);
                break;
            case EasyFunctionType.EaseInOutQuad:
                y = EaseInOutQuad(interpolation);
                break;
            case EasyFunctionType.EaseInCubic:
                y = EaseInCubic(interpolation);
                break;
            case EasyFunctionType.EaseOutCubic:
                y = EaseOutCubic(interpolation);
                break;
            case EasyFunctionType.EaseInOutCubic:
                y = EaseInOutCubic(interpolation);
                break;
            case EasyFunctionType.EaseInQuart:
                y = EaseInQuart(interpolation);
                break;
            case EasyFunctionType.EaseOutQuart:
                y = EaseOutQuart(interpolation);
                break;
            case EasyFunctionType.EaseInOutQuart:
                y = EaseInOutQuart(interpolation);
                break;
            case EasyFunctionType.EaseInQuint:
                y = EaseInQuint(interpolation);
                break;
            case EasyFunctionType.EaseOutQuint:
                y = EaseOutQuint(interpolation);
                break;
            case EasyFunctionType.EaseInOutQuint:
                y = EaseInOutQuint(interpolation);
                break;
            case EasyFunctionType.EaseInExpo:
                y = EaseInExpo(interpolation);
                break;
            case EasyFunctionType.EaseOutExpo:
                y = EaseOutExpo(interpolation);
                break;
            case EasyFunctionType.EaseInOutExpo:
                y = EaseInOutExpo(interpolation);
                break;
            case EasyFunctionType.EaseInCirc:
                y = EaseInCirc(interpolation);
                break;
            case EasyFunctionType.EaseOutCirc:
                y = EaseOutCirc(interpolation);
                break;
            case EasyFunctionType.EaseInOutCirc:
                y = EaseInOutCirc(interpolation);
                break;
            case EasyFunctionType.EaseInBack:
                y = EaseInBack(interpolation);
                break;
            case EasyFunctionType.EaseOutBack:
                y = EaseOutBack(interpolation);
                break;
            case EasyFunctionType.EaseInOutBack:
                y = EaseInOutBack(interpolation);
                break;
            case EasyFunctionType.EaseInElastic:
                y = EaseInElastic(interpolation);
                break;
            case EasyFunctionType.EaseOutElastic:
                y = EaseOutElastic(interpolation);
                break;
            case EasyFunctionType.EaseInOutElastic:
                y = EaseInOutElastic(interpolation);
                break;
            case EasyFunctionType.EaseInBounce:
                y = EaseInBounce(interpolation);
                break;
            case EasyFunctionType.EaseOutBounce:
                y = EaseOutBounce(interpolation);
                break;
            case EasyFunctionType.EaseInOutBounce:
                y = EaseInOutBounce(interpolation);
                break;
            default:
                y = 1f;
                break;
        }
        return y;
    }
    public static float CalculateInterpolation(EasyFunctionType interpolatedMethod, float min, float max, float interpolation)
    {
        float y = CalculateInterpolation(interpolatedMethod, interpolation);
        return Scale(y, min, max);
    }



    public static float EaseOutSine(float x)
    {
        return Mathf.Sin((x* Mathf.PI) / 2);
    }

    public static float EaseInOutSine(float x){
        return -(Mathf.Cos(Mathf.PI* x) - 1) / 2;
    }


    public static float EaseInQuad(float x){
        return x* x;    
    }

    public static float EaseOutQuad(float x){
        return 1 - (1 - x) * (1 - x);
    }

    public static float EaseInOutQuad(float x) {
        return x< 0.5 ? 2 * x* x : 1 - Mathf.Pow(-2 * x + 2, 2) / 2;
    }

    public static float EaseInCubic(float x){
        return x* x * x;
    }

    public static float EaseOutCubic(float x)
    {
        return 1 - Mathf.Pow(1 - x, 3);
    }

    public static float EaseInOutCubic(float x)
    {
        return x < 0.5 ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;
    }

    public static float EaseInQuart(float x)
    {
        return x * x * x * x;
    }

    public static float EaseOutQuart(float x)
    {
        return 1 - Mathf.Pow(1 - x, 4);
    }

    public static float EaseInOutQuart(float x)
    {
        return x < 0.5 ? 8 * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 4) / 2;
    }
    public static float EaseInQuint(float x)
    {
        return x * x * x * x * x;
    }
    public static float EaseOutQuint(float x)
    {
        return 1 - Mathf.Pow(1 - x, 5);
    }

    public static float EaseInOutQuint(float x)
    {   
        return x < 0.5 ? 16 * x * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 5) / 2;
    }

    public static float EaseInExpo(float x)
    {
        return x == 0 ? 0 : Mathf.Pow(2, 10 * x - 10);
    }

    public static float EaseOutExpo(float x)
    {
        return x == 1 ? 1 : 1 - Mathf.Pow(2, -10 * x);
    }

    public static float EaseInOutExpo(float x)
    {
        return x == 0
        ? 0
        : x == 1
        ? 1
        : x < 0.5 ? Mathf.Pow(2, 20 * x - 10) / 2
        : (2 - Mathf.Pow(2, -20 * x + 10)) / 2;
    }

    public static float EaseInCirc(float x)
    {
        return 1 - Mathf.Sqrt(1 - Mathf.Pow(x, 2));
    }

    public static float EaseOutCirc(float x)
    {
        return Mathf.Sqrt(1 - Mathf.Pow(x - 1, 2));
    }

    public static float EaseInOutCirc(float x)
    {
        return x < 0.5
        ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * x, 2))) / 2
        : (Mathf.Sqrt(1 - Mathf.Pow(-2 * x + 2, 2)) + 1) / 2;
    }

    public static float EaseInBack(float x)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1;

        return c3 * x * x * x - c1 * x * x;
    }

    public static float EaseOutBack(float x)
    {
        float c1 = 1.70158F;
        float c3 = c1 + 1;

        return 1 + c3 * Mathf.Pow(x - 1, 3) + c1 * Mathf.Pow(x - 1, 2);
    }

    public static float EaseInOutBack(float x)
    {
        float c1 = 1.70158f;
        float c2 = c1 * 1.525f;

        return x < 0.5
          ? (Mathf.Pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2)) / 2
          : (Mathf.Pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 2) / 2;
    }

    public static float EaseInElastic(float x)
    {
        float c4 = (2 * Mathf.PI) / 3;

        return x == 0
          ? 0
          : x == 1
          ? 1
          : -Mathf.Pow(2, 10 * x - 10) * Mathf.Sin((x * 10 - 10.75F) * c4);
    }

    public static float EaseOutElastic(float x)
    {
        float c4 = (2 * Mathf.PI) / 3;

        return x == 0
          ? 0
          : x == 1
          ? 1
          : Mathf.Pow(2, -10 * x) * Mathf.Sin((x * 10 - 0.75f) * c4) + 1;
    }

    public static float EaseInOutElastic(float x)
    {
        float c5 = (2 * Mathf.PI) / 4.5f;

        return x == 0
          ? 0
          : x == 1
          ? 1
          : x < 0.5
          ? -(Mathf.Pow(2, 20 * x - 10) * Mathf.Sin((20 * x - 11.125f) * c5)) / 2
          : (Mathf.Pow(2, -20 * x + 10) * Mathf.Sin((20 * x - 11.125f) * c5)) / 2 + 1;
    }

    public static float EaseInBounce(float x)
    {
        return 1 - EaseOutBounce(1 - x);
    }

    public static float EaseOutBounce(float x)
    {
        float n1 = 7.5625f;
        float d1 = 2.75f;

        if (x < 1 / d1)
        {
            return n1 * x * x;
        }
        else if (x < 2 / d1)
        {
            return n1 * (x -= 1.5f / d1) * x + 0.75f;
        }
        else if (x < 2.5 / d1)
        {
            return n1 * (x -= 2.25f / d1) * x + 0.9375f;
        }
        else
        {
            return n1 * (x -= 2.625f / d1) * x + 0.984375f;
        }
    }

    public static float EaseInOutBounce(float x)
    {
        return x < 0.5
          ? (1 - EaseOutBounce(1 - 2 * x)) / 2
          : (1 + EaseOutBounce(2 * x - 1)) / 2;
    }


}
