using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MotionSeek : MonoBehaviour
{
    public UnityEvent OnFinishMovement;
    public Transform target;
    public MotionSpeed speed;
    public float stoppingDistance = 0.1f;


    public void SetTarget(Transform t)
    {
        target = t;
        speed.SetTarget(t);
        Init();
    }

    public void Init()
    {
        enabled = true;
        speed.StartAcceleration();
    }
    public float CurrentSpeed
    {
        get { return speed.CurrentSpeed; }
    }
    public virtual void Move(float time)
    {
        Vector3 direction = target.position - this.transform.position;
        direction = direction.normalized;
        float s = CurrentSpeed;
        this.transform.position = this.transform.position + direction * s * time;
        if (Utils.ArrivedAtPoint(direction, this.transform.position, target.position, stoppingDistance))
        {
            this.transform.position = target.position;
            enabled = false;
            OnFinishMovement?.Invoke();
            speed.Stop();
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Move(Time.deltaTime);
    }
}
