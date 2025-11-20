using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SavingInfo : FadeInfoController
{
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    protected override void OnFadeOutFinish()
    {
        this.gameObject.SetActive(false);
    }

    public override void Show()
    {
        this.gameObject.SetActive(true);
        base.Show();
    }
}
