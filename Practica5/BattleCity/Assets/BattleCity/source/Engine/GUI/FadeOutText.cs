using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutText : Fade
{
    public System.Action OnFadeOutFinish;
    public Text _text;
    public Text[] _textArr;

    protected override float CalculateTransparent()
    {
        float transparent = Mathf.Lerp(1f, 0f, CurrentTime / _fadeTime);
        return transparent;
    }
    protected override void ApplyTransparent(float transparent)
    {
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, transparent);
        if (_textArr != null)
        {
            for (int i = 0; i < _textArr.Length; i++)
            {
                _textArr[i].color = new Color(_textArr[i].color.r, _textArr[i].color.g, _textArr[i].color.b, transparent);
            }
        }
    }
    protected override void ResetTransparent()
    {
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 1f);
        if (_textArr != null)
        {
            for (int i = 0; i < _textArr.Length; i++)
            {
                _textArr[i].color = new Color(_textArr[i].color.r, _textArr[i].color.g, _textArr[i].color.b, 1);
            }
        }

    }
    protected override void OnFadeFinish()
    {
        if (OnFadeOutFinish != null)
            OnFadeOutFinish();
    }

}
