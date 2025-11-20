using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Este componente mueve un objeto entre dos puntos, pero puede utilizar una función de interpolación
/// entre ambos puntos. La función de interpolación puede estar o no escalada y puede afectar al eje X o al eje y
/// por defecto X seria el dominio e Y el recorrido, pero dependerá del mapeo final
/// </summary>
public class MotionMoveInterpolation : MotionMovePosition, IInterpolation
{
    public enum AxisMapping {  X, Y, Z}
    [SerializeField]
    protected AxisMapping xAxisMapping = AxisMapping.X;
    [SerializeField]
    protected AxisMapping yAxisMapping = AxisMapping.Y;
    [SerializeField]
    protected EasyFunctionType easyFunctionType;
    public AnimationCurve curve;
    public bool inverse;
    [SerializeField]
    protected EasyFunctionType inverseEasyFunctionType;
    public AnimationCurve inverseCurve;

    protected float xScale;
    protected float yScale;
    protected float xInit;
    protected float yInit;
    protected float moveDirection;
    protected float _interpolation;

    public EasyFunctionType EasyFunctionType
    {
        get { return easyFunctionType; }
        set { easyFunctionType = value; }
    }
    protected void SetDestination(float xs, float ys, float md = 1f)
    {
        moveDirection = md;
        xScale = xs;
        yScale = ys;
        enabled = true;

        if (md < 0f)
        {
            xInit = XAxis - xScale;
            yInit = YAxis - yScale;
        }
        else
        {
            xInit = XAxis;
            yInit = YAxis;
        }
        if (speed is MotionAccelArriveSpeed)
            ((MotionAccelArriveSpeed)speed).SetDestination(Utils.IncPosition(this.transform.position, xs, ys, 0f));
        speed.StartAcceleration();
    }

    /// <summary>
    /// Siempre tiene los dos puntos entre los que se mueve el movil, y en función del a dirección, empezara en el primer punto o en el segundo
    /// Si punto 1 es menor que punto 2 en el eje y mirará hacia arriba si no mirará hacia abajo (estará invertido)
    /// </summary>
    /// <param name="a_origin"></param>
    /// <param name="a_destination"></param>
    /// <param name="dir"></param>

    public override void SetPositions(Vector3 a_origin, Vector3 a_destination, float dir)
    {
        _interpolation = 0f;
        float xs = a_destination.x - a_origin.x;
        float ys = a_destination.y - a_origin.y;
        if (dir > 0)
            this.transform.position = a_origin;
        else
            this.transform.position = a_destination;
        SetDestination(xs, ys, dir);
    }

    /// <summary>
    /// Pone como foco el transform actual, por lo que la interpolación la hace desde un punto de vista local a ese punto
    /// </summary>
    /// <param name="a_destination"></param>
    public override void SetDestination(Vector3 a_destination)
    {
        float xs = a_destination.x - this.transform.position.x;
        float ys = a_destination.y - this.transform.position.y;
        SetDestination(xs, ys);
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
        return inverse ? inverseCurve.Evaluate(f) : curve.Evaluate(f);
    }
    public override void Move(float time)
    {
        float x = XAxis;
        bool finish = false;
        if (moveDirection < 0f)
            x += speed.CurrentSpeed * time * Mathf.Sign(xScale) * -1;
        else
            x += speed.CurrentSpeed * time * Mathf.Sign(xScale);

        float distance = Mathf.Abs(x - xInit);
        float distanceToMax = Mathf.Abs(x - (xInit + xScale));
        Vector3 currentPosition = this.transform.position;
        if (moveDirection > 0f && distance < Mathf.Abs(xScale) || moveDirection < 0f && distanceToMax < Mathf.Abs(xScale))
        {
            _interpolation = distance / xScale;
            float y = CalculateInterpolation(inverse ? inverseEasyFunctionType : easyFunctionType, yInit, yInit + yScale, Mathf.Abs(_interpolation));
            YAxis = y;
        }
        else
        {
            _interpolation = 1f;
            float y = CalculateInterpolation(inverse ? inverseEasyFunctionType : easyFunctionType, yInit, yInit + yScale, _interpolation);
            YAxis = y;
            finish = true;
        }
        XAxis = x;
        _lastVelocity = this.transform.position - currentPosition;
        if (finish)
        {
            EndMovement();
        }
    }

    public void OldMove(float time)
    {

    }


    protected float CalcInverse(float f)
    {
        if (inverse)
            f = 1f - f;
        return f;
    }

    public virtual float XAxis
    {
        get
        {
            if (xAxisMapping == AxisMapping.X)
                return this.transform.position.x;
            else if (xAxisMapping == AxisMapping.Y)
                return this.transform.position.y;
            else
                return this.transform.position.z;
        }

        set
        {
            if (xAxisMapping == AxisMapping.X)
                this.transform.position = new Vector3(value, this.transform.position.y, this.transform.position.z);
            else if (xAxisMapping == AxisMapping.Y)
                this.transform.position = new Vector3(this.transform.position.x, value, this.transform.position.z);
            else
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, value);
        }

    }

    public virtual float YAxis
    {
        get
        {
            if (yAxisMapping == AxisMapping.X)
                return this.transform.position.x;
            else if (yAxisMapping == AxisMapping.Y)
                return this.transform.position.y;
            else
                return this.transform.position.z;
        }

        set
        {
            if (yAxisMapping == AxisMapping.X)
                this.transform.position = new Vector3(value, this.transform.position.y, this.transform.position.z);
            else if (yAxisMapping == AxisMapping.Y)
                this.transform.position = new Vector3(this.transform.position.x, value, this.transform.position.z);
            else
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, value);
        }

    }

    public virtual float Interpolated
    {
        get { return _interpolation; }
    }
}
