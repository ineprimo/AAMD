using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    public delegate void OnTimerFinish(float time);

    private float _currenTime;
    private float _maxTime;
    private float _randomRange;
    private float _currentRandomRange;
    private OnTimerFinish _event;
    private bool _started = false;
    private bool _pause = false;
    private bool _loop;

    private float _timeToAlert;
    private System.Action _onAlertAtTime;
    private bool _onAlertAtTimeThrowed;
    // Start is called before the first frame update

    public Timer(float maxTime, OnTimerFinish ev, AutoTickable tick = null, bool loop = false, float randomRange = -1f)
    {
        _maxTime = maxTime;
        _event = ev;
        _started = false;
        _randomRange = randomRange;
        _onAlertAtTimeThrowed = false;
        _loop = loop;
        _pause = false;
        if (tick != null)
            tick.AddUpdate(this.Update);
    }

    public void Start(float t)
    {
        _maxTime = t;
        Start();
    }

    public void Start()
    {
        _currenTime = 0;
        _started = true;
        RecalculateRandomRange();
    }

    public float Time
    {
        get
        {
            return _currenTime;
        }
    }

    public float MaxTime
    {
        get
        {
            return _maxTime + _currentRandomRange;
        }
    }

    public void Stop()
    {
        _currenTime = 0;
        _started = false;
    }

    public void Pause()
    {
        _pause = true;
    }

    public void Resume()
    {
        _pause = false;
    }

    // Update is called once per frame
    public void Update(float deltaTime)
    {
        if (!_started)
            return;

        if (_pause)
            return;

        _currenTime += deltaTime;
        if(_onAlertAtTime != null && !_onAlertAtTimeThrowed && _currenTime >= _timeToAlert)
        {
            _onAlertAtTime();
            _onAlertAtTimeThrowed = true;
        }

        if (_currenTime > MaxTime && _event != null)
        {
            
            _currenTime = _currenTime- MaxTime;
            RecalculateRandomRange();
            if (!_loop)
                _started = false;
            _onAlertAtTimeThrowed = false;
            _event(_currenTime);
        }
    }

    public void AlertAt(float t, System.Action action)
    {
        Debug.Assert(t > 0 && t <= _maxTime);
        _timeToAlert = _maxTime * t;
        _onAlertAtTime = action;
        _onAlertAtTimeThrowed = false;
    }

    private void RecalculateRandomRange()
    {
        if (_randomRange > 0f)
            _currentRandomRange = Random.Range(-_randomRange, _randomRange);
        else
            _currentRandomRange = 0f;
    }
}
