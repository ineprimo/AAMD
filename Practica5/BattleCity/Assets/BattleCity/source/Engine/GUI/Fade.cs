using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fade : MonoBehaviour
{
    public float _fadeTime;
    public bool _enable = true;

    private float _time = 0;
    // Use this for initialization
    void Start()
    {
        if (_enable)
            ResetFade();
    }

    protected abstract float CalculateTransparent();
    protected abstract void ApplyTransparent(float transparent);
    protected abstract void ResetTransparent();
    protected abstract void OnFadeFinish();
    // Update is called once per frame
    void Update()
    {
        if (_enable)
        {
            _time += Time.deltaTime;
            float transparent = CalculateTransparent();// Mathf.Lerp(0, 1f, _time / _fadeTime);
            ApplyTransparent(transparent);
            if (_time >= _fadeTime)
            {
                End();
                OnFadeFinish();
            }
        }
    }

    protected float CurrentTime
    {
        get
        {
            return _time;
        }
    }

    public void ResetFade()
    {
        _time = 0f;
        ResetTransparent();
    }

    public void Init()
    {
        _enable = true;
        ResetFade();
    }

    public void End()
    {
        _enable = false;
    }
}
