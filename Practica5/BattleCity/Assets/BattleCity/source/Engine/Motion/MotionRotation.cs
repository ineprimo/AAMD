using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MotionRotation : MonoBehaviour
{
    public UnityEvent OnFinishRotation;
    [SerializeField]
    protected MotionSpeed speed;

    private Vector3 _targetRotation;
    private Vector3 _rotationDirection;
    // Start is called before the first frame update
    void Start()
    {
        enabled = false;
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

    public void SetRotationEnd(Vector3 rotationEnd)
    {
        _targetRotation = rotationEnd;
        _rotationDirection = _targetRotation - this.transform.rotation.eulerAngles;
        _rotationDirection = _rotationDirection.normalized;
        enabled = true;
    }

    public float CurrentSpeed
    {
        get { return speed.CurrentSpeed; }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newAngle = this.transform.rotation.eulerAngles + (Time.deltaTime * CurrentSpeed * _rotationDirection);
        this.transform.rotation = Quaternion.Euler(newAngle);
        if (Utils.ArrivedAtPoint(_rotationDirection,newAngle, _targetRotation,0.1f))
        {
            this.transform.rotation = Quaternion.Euler(_targetRotation);
            EndRotation();
        }

    }

    protected virtual void EndRotation()
    {
        speed.Stop();
        OnFinishRotation?.Invoke();
        enabled = false;
    }
}
