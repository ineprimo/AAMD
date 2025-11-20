using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : Fade
{
    public Image _image;
    public Image[] _imageArr;

    public System.Action OnFadeOutFinish;

    protected override float CalculateTransparent()
    {
        float transparent = Mathf.Lerp(1, 0f, CurrentTime / _fadeTime);
        return transparent;
    }
    protected override void ApplyTransparent(float transparent)
    {
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, transparent);
        if (_imageArr != null)
        {
            for (int i = 0; i < _imageArr.Length; i++)
            {
                _imageArr[i].color = new Color(_imageArr[i].color.r, _imageArr[i].color.g, _imageArr[i].color.b, transparent);
            }
        }
    }
    protected override void ResetTransparent()
    {
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 1f);
        if (_imageArr != null)
        {
            for (int i = 0; i < _imageArr.Length; i++)
            {
                _imageArr[i].color = new Color(_imageArr[i].color.r, _imageArr[i].color.g, _imageArr[i].color.b, 1f);
            }
        }
    }
    protected override void OnFadeFinish()
    {
        if (OnFadeOutFinish != null)
            OnFadeOutFinish();
    }



}
