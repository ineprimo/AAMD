using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecureAnimation : MonoBehaviour
{
    [SerializeField]
    private Animation _animation;

    private string _animationName;
    private bool _active;
    private System.Action<string> OnAnimationFinished;
    private DefferedCallID defferedCallID;

    public Animation Animation
    {
        get
        {
            if (_animation == null)
                _animation = GetComponent<Animation>();
            return _animation;
        }
    }
    public void Play(string name, System.Action<string> onAnimationFinished)
    {
        _animationName = name;
        OnAnimationFinished = onAnimationFinished;
        AnimationClip clip = Animation.GetClip(name);
        defferedCallID = GameMgr.Instance.TimeMgr.DeferredCall(this, clip.length, InterlOnAnimationFinished);
        Animation.Play(name);
        _active = true;
    }
    protected void InterlOnAnimationFinished()
    {
        _active = false;
        OnAnimationFinished?.Invoke(_animationName);
    }
    private void OnEnable()
    {
        if (_active)
        {
            //repetimos la animación.
            GameMgr.Instance.TimeMgr.CancelDeferredCall(defferedCallID);
            Play(_animationName, OnAnimationFinished);
        }
        _OnEnable();
    }

    protected virtual void _OnEnable()
    {

    }
}
