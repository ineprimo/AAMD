using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class MotionAccelArriveBase : MotionAccelerationSpeed
{
    public UnityEvent OnStartDeccel;
    [SerializeField]
    protected float arriveDistance;
    [SerializeField]
    protected float threshold;

    [SerializeField]
    protected EasyFunctionType interpolatedMethodArrive;
    public AnimationCurve curveArrive;

    private bool isDecell = false;
    private bool startNear = false;
    private float maxSpeedReach;
    private float minSpeedReach;


    public EasyFunctionType InterpolatedMethodArrive
    {
        get
        {
            return interpolatedMethodArrive;
        }

        set
        {
            interpolatedMethodArrive = value;
        }
    }

    public float ArriveDistance
    {
        get { return arriveDistance; }
        set
        {
            arriveDistance = value;
        }
    }

    public abstract Vector3 Destination
    {
        get;
    }
    public override void CalculateCurrentSpeed(float time)
    {
        float distance = Vector3.Distance(Destination, this.transform.position);
        // estamos en frenada
        if (distance <= arriveDistance)
        {
            if (!isDecell)
                StartDeccel();
                
            isDecell = true;
            _interpolation = 1f - distance / arriveDistance;
            if (distance < threshold)
                Stop();
        }
        else if (_currentAccelerationTime < accelerationTime) // Estamos acelerando
        {
            if (isDecell) // estabamos decelerando, pero hemos superado la distancia de deceleración (por ejemplo el objeto es movil, asi que partimos desde la velocidad mínima alcanzada
            {
                minSpeedReach = _currentSpeed;
            }
            isDecell = false;
            _interpolation = _currentAccelerationTime / accelerationTime;
            _currentAccelerationTime += time;
            if (_currentAccelerationTime >= accelerationTime)
                FinishAccel();

        }
        else // estamos con la velocidad Máxima pero aún no hemos llegado a la distancia de llegada
        {
            isDecell = false;
            _interpolation = 1f;
        }

        if ((_interpolation < 1f && !isDecell))
        {
            _currentSpeed = CalculateInterpolation(interpolatedMethod, minSpeedReach, maxSpeed, _interpolation, curve); 
            // no partimos siempre de la velocidad mínima si no de la minima alcanzada. Si estamos parados será la mínima pero si por lo que sea estabamos decelerando será la mínima alcanzada en la deceleración
        }
        else if (_interpolation < 1f && isDecell)
        {
            //no partimos de la velocidad máxima si no de la máxima alcanzada.
            _currentSpeed = CalculateInterpolation(interpolatedMethodArrive, maxSpeedReach, minSpeed, _interpolation, curveArrive);
        }
        else
            _currentSpeed = maxSpeed;
    }

    public override void StartAcceleration()
    {
        base.StartAcceleration();
        isDecell = false;
        float distance = Vector3.Distance(Destination, this.transform.position);
        startNear = distance < arriveDistance;
        if (startNear) // si arrancamos muy cerca del objetivo, no aceleramos porque no tiene sentido
        { // pero tampoco nos ponemos a velocidad máxima, simplemente matenemos una velocidad media constante
            _currentSpeed = maxSpeed - minSpeed;
            Stop();
        }
        minSpeedReach = minSpeed;

    }


    public bool IsMaximunSpeed
    {
        get { return _interpolation == 1f; }
    }

    protected override void FinishAccel()
    {
        OnFinishAccel?.Invoke();
    }


    private void StartDeccel()
    {
        //nos guardamos la velocidad máxima a la que hayamos llegado.
        maxSpeedReach = _currentSpeed;
        OnStartDeccel?.Invoke();
    }
}
