using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionMoveMappedBase : MotionMovePosition, IInterpolation
{
    public AnimationCurve curve;
    public bool inverse;
    [SerializeField]
    public AnimationCurve inverseCurve;
    public Rigidbody _rigidbody;


    protected float moveDirection;
    protected Vector3 _origin;
    protected Vector3 _directionRotation90;
    protected Vector3 _directionUnit;
    protected Vector3 _displacement;

    public float MoveDirection
    {
        get
        {
            return moveDirection;
        }
    }
    public float Interpolated
    {
        get
        {
            return _displacement.magnitude / _direction.magnitude;
        }
    }


    public void MoveToPosition(Vector3 newPosition)
    {
        if (_rigidbody == null)
            this.transform.position = newPosition;
        else
            _rigidbody.MovePosition(newPosition);
    }


}
