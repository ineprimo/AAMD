using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementDisplay : SecureAnimation
{
    public Image icon;
    public TMP_Text title;
    public float time;

    private void Start()
    {
        this.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show(string t, Sprite i)
    {
        this.gameObject.SetActive(true);
        title.text = t;
        icon.sprite = i;
        Play("AchievementIn",null);
        GameMgr.Instance.TimeMgr.DeferredCall(this, time, HideAnim);
    }
    public void HideAnim()
    {
        Play("AchievementOut", OnAchievementOut);
    }

    public void OnAchievementOut(string n)
    {
        this.gameObject.SetActive(false);
    }

}
