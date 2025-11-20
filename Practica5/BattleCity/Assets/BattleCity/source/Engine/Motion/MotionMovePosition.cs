using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MotionMovePosition : MotionCheckCollision
{
    public UnityEvent OnFinishMovement;
    
    [SerializeField]
    protected MotionSpeed speed;


    protected Vector3 _direction;
    protected Vector3 destination;

    protected Vector3 _lastVelocity;
    protected bool _init = false;

    public Vector3 Destination
    {
        get{return destination;}
        set{destination = value;}
    }
    public MotionSpeed MotionSpeed
    {
        get { return speed; }
        set { speed = value; }
    }

    public bool IsStopped()
    {
        return !enabled;
    }

    public Vector3 LastDirection
    {
        get
        {
            return _direction;
        }
    }

    public Vector3 LastVelocity
    {
        get
        {
            return _lastVelocity;
        }
    }


    public void Stop()
    {
        base.EndMovement();
        speed.Stop();
    }
    public Vector3 Direction
    {
        get { return _direction; }
        set { _direction = value; }
    }


    public virtual void SetPositions(Vector3 a_origin,Vector3 a_destination, float dir)
    {
        this.transform.position = dir > 0 ? a_origin : a_destination;
        SetDestination(dir > 0 ? a_destination : a_origin);
    }

    public virtual void SetDestination(Vector3 a_destination)
    {
        _direction = a_destination - this.transform.position;
        _direction = _direction.normalized;
        destination = a_destination;
        enabled = true;
        if (speed is MotionAccelArriveSpeed)
            ((MotionAccelArriveSpeed)speed).SetDestination(a_destination);
        speed.StartAcceleration();
        collisionRadious = -1f;
        _lastVelocity = Vector3.zero;
        _init = true;
    }

    public float CurrentSpeed
    {
        get{return speed.CurrentSpeed; }
    }

    public virtual void Move(float time)
    {
        this.transform.position = this.transform.position + (time * CurrentSpeed *_direction);
        _lastVelocity = CurrentSpeed * time * _direction;
        if (Utils.ArrivedAtPoint(_direction, this.transform.position, destination, 0.1f))
        {
            this.transform.position = destination;
            EndMovement();
        }
    }

    protected override void EndMovement()
    {
        base.EndMovement();
        speed.Stop();
        OnFinishMovement?.Invoke();
    }

    void Awake()
    {
        if(!_init)
            enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Move(Time.deltaTime);
        UpdateCollision(Time.deltaTime);
    }
}
