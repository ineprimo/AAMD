using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : Fade
{
    public Image _image;
    public Image[] _imageArr;
    public System.Action OnFadeInFinish;

    protected override float CalculateTransparent()
    {
        float transparent = Mathf.Lerp(0, 1f, CurrentTime / _fadeTime);
        return transparent;
    }
    protected override void ApplyTransparent(float transparent)
    {
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, transparent);
        if(_imageArr != null)
        {
            for (int i = 0; i < _imageArr.Length; i++)
            {
                _imageArr[i].color = new Color(_imageArr[i].color.r, _imageArr[i].color.g, _imageArr[i].color.b, transparent);
            }
        }
    }
    protected override void ResetTransparent()
    {
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0f);
        if (_imageArr != null)
        {
            for (int i = 0; i < _imageArr.Length; i++)
            {
                _imageArr[i].color = new Color(_imageArr[i].color.r, _imageArr[i].color.g, _imageArr[i].color.b, 0f);
            }
        }
    }
    protected override void OnFadeFinish()
    {
        if (OnFadeInFinish != null)
            OnFadeInFinish();
    }

}
