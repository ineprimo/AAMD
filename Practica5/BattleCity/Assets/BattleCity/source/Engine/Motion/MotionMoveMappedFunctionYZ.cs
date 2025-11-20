using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionMoveMappedFunctionYZ : MotionMoveMappedBase
{
    public float _yScale = 1f;
    /// <summary>
    /// Siempre tiene los dos puntos entre los que se mueve el movil, y en función del a dirección, empezara en el primer punto o en el segundo
    /// Si punto 1 es menor que punto 2 en el eje y mirará hacia arriba si no mirará hacia abajo (estará invertido)
    /// </summary>
    /// <param name="a_origin"></param>
    /// <param name="a_destination"></param>
    /// <param name="dir"></param>

    public override void SetPositions(Vector3 a_origin, Vector3 a_destination, float dir)
    {
        _direction = a_destination - a_origin;
        destination = a_destination;
        _origin = a_origin;
        _directionRotation90 = Vector3.zero;
        _directionRotation90 = Utils.RotateVector2ExeYZ(_direction, _direction.z >= 0 ? -90 : 90);
        _directionRotation90 = _directionRotation90.normalized;
        _directionUnit = _direction.normalized;
        moveDirection = dir;
        _displacement = Vector3.zero;
        enabled = true;
        if (speed is MotionAccelArriveSpeed)
            ((MotionAccelArriveSpeed)speed).SetDestination(Utils.IncPosition(this.transform.position, 0f, _direction.y, _direction.z));
        speed.StartAcceleration();
    }

    public float CalculatedCustomFunction(float f)
    {
        return inverse ? inverseCurve.Evaluate(f) : curve.Evaluate(f);
    }

    public override void Move(float time)
    {

        bool finish = false;
        if (moveDirection < 0f)
            _displacement = _displacement + _directionUnit * speed.CurrentSpeed * time*-1f;
        else
            _displacement = _displacement + _directionUnit * speed.CurrentSpeed * time;

        float distance = Mathf.Abs(_displacement.z);
        float distanceToMax = Mathf.Abs(destination.z - (_origin + _displacement).z);
        Vector3 currentPosition = this.transform.position;
        if (moveDirection > 0f && distance < Mathf.Abs(_direction.z) || moveDirection < 0f && distanceToMax < Mathf.Abs(_direction.z))
        {
            float interpolation = _displacement.magnitude / _direction.magnitude;
            float y = CalculatedCustomFunction(Mathf.Abs(interpolation));
            Debug.DrawRay(_origin, _displacement, Color.green);
            Debug.DrawRay(_origin, _directionRotation90 * y * _yScale, Color.blue);
            Debug.DrawRay(_origin, _displacement + _directionRotation90 * y * _yScale, Color.red);
            MoveToPosition((_origin + _displacement) + _directionRotation90 * y * _yScale);
        }
        else
        {
            MoveToPosition(destination);
            finish = true;
        }
        _lastVelocity = this.transform.position - currentPosition;
        if (finish)
        {
            EndMovement();
        }
    }
}
