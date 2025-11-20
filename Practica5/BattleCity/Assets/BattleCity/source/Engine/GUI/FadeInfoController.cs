using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(FadeIn))]
[RequireComponent(typeof(FadeOut))]
[RequireComponent(typeof(FadeInTextPro))]
[RequireComponent(typeof(FadeOutTextPro))]
public class FadeInfoController : AutoTickableUpdate
{
    public float timeToShow;

    protected FadeIn _fadeInImg;
    protected FadeOut _fadeOutImg;
    protected FadeInTextPro _fadeInTxt;
    protected FadeOutTextPro _fadeOutTxt;
    protected Timer _timer;
    // Start is called before the first frame update

    private void Awake()
    {
        _fadeInImg = GetComponent<FadeIn>();
        _fadeOutImg = GetComponent<FadeOut>();
        _fadeInTxt = GetComponent<FadeInTextPro>();
        _fadeOutTxt = GetComponent<FadeOutTextPro>();
        _timer = new Timer(timeToShow, OnTimerFinish, this);
        _Awake();
    }

    protected virtual void _Awake()
    {

    }

    protected void OnTimerFinish(float time)
    {
        _fadeOutImg.Init();
        _fadeOutTxt.Init();
        _fadeOutTxt.OnFadeOutFinish = OnFadeOutFinish;
    }

    protected virtual void OnFadeOutFinish()
    {

    }


    public virtual void Show()
    {
        _fadeInImg.Init();
        _fadeInTxt.Init();
        _timer.Start();
    }
}
