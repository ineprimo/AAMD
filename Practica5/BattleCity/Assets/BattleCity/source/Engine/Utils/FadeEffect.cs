using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class FadeEffect 
{
    public System.Action OnFadeIn;
    public System.Action OnFadeOut;

    private float _time;
    private float fadeTime;

    public FadeEffect(float t)
    {
        fadeTime = t;
    }



    public void FadeIn()
    {
        float fade = Mathf.Lerp(0f, 1f, _time / fadeTime);
        SetFade(fade);
        if (_time > fadeTime)
        {
            _time = 0f;
            if (OnFadeIn != null)
                OnFadeIn();
        }
    }

    public void SpendTime(float deltaTime)
    {
        _time += deltaTime;
    }

    public void SetTime(float time)
    {
        _time = time;
    }

    public void FadeOut()
    {

        float fade = Mathf.Lerp(1f, 0f, _time / fadeTime);
        SetFade(fade);
        if (_time > fadeTime)
        {
            if (OnFadeOut != null)
                OnFadeOut();
            _time = 0f;
        }

    }

    public abstract void SetFade(float fade);

    public abstract void ChangeLayer(int layer, out int original);
}


public class MaterialFadeEffect : FadeEffect
{
    FadeMaterialController _fadeMaterialController;

    public MaterialFadeEffect(float t, FadeMaterialController FadeEffect) : base(t)
    {
        _fadeMaterialController = FadeEffect;
    }

    public override void ChangeLayer(int layer, out int original)
    {
        _fadeMaterialController.ChangeLayer(layer,out original);
    }

    public override void SetFade(float fade)
    {
        _fadeMaterialController.Fade = fade;
    }
}

public class UIFadeEffect<T> : FadeEffect where T: MaskableGraphic
{
    private List<T> _fadeMaterialController;

    public UIFadeEffect(float t, T[] FadeEffect) : base(t)
    {
        _fadeMaterialController = new List<T>(FadeEffect);
    }


    public void AddImagenes(T[] FadeEffect)
    {
        for (int i = 0; i < FadeEffect.Length; i++)
        {
            _fadeMaterialController.Add(FadeEffect[i]);
        }
    }

    public override void ChangeLayer(int layer, out int original)
    {
        original = 0;
        for (int i = 0; i < _fadeMaterialController.Count; i++)
        {
            original = _fadeMaterialController[i].gameObject.layer;
            _fadeMaterialController[i].gameObject.layer = layer;
        }
    }
    public override void SetFade(float fade)
    {
        for(int i = 0; i < _fadeMaterialController.Count; i++)
        {
            if(_fadeMaterialController[i] != null)
            {
                _fadeMaterialController[i].color = new Color(_fadeMaterialController[i].color.r, _fadeMaterialController[i].color.g,
                _fadeMaterialController[i].color.b, fade);
            }

        }
    }

    public void SetTransparent()
    {
        for (int i = 0; i < _fadeMaterialController.Count; i++)
        {
            if (_fadeMaterialController[i] != null)
            {
                _fadeMaterialController[i].color = new Color(_fadeMaterialController[i].color.r, _fadeMaterialController[i].color.g,
                _fadeMaterialController[i].color.b, 0f);
            }
        }

    }
}

