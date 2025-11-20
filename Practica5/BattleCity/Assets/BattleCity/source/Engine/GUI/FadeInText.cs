using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInText : Fade
{
    public System.Action OnFadeInFinish;
    public Text _text;
    public Text[] _textArr;


    protected override float CalculateTransparent()
    {
        float transparent = Mathf.Lerp(0, 1f, CurrentTime / _fadeTime);
        return transparent;
    }
    protected override void ApplyTransparent(float transparent)
    {
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, transparent);
        if(_textArr != null)
        {
            for (int i = 0; i < _textArr.Length; i++)
            {
                _textArr[i].color = new Color(_textArr[i].color.r, _textArr[i].color.g, _textArr[i].color.b, transparent);
            }
        }
    }
    protected override void ResetTransparent()
    {
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 0f);
        if (_textArr != null)
        {
            for (int i = 0; i < _textArr.Length; i++)
            {
                _textArr[i].color = new Color(_textArr[i].color.r, _textArr[i].color.g, _textArr[i].color.b, 0);
            }
        }
    }
    protected override void OnFadeFinish()
    {
        if (OnFadeInFinish != null)
            OnFadeInFinish();
    }

}
