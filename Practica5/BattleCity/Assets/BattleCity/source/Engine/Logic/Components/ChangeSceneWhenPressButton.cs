using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneWhenPressButton : MonoBehaviour
{
    private const float TIME_WAITING_TO_HIDE_SKIP_PANEL = 2f;
    public string sceneWhenVideoEnd;
    public string nextScene;

    // Start is called before the first frame update
    void Start()
    {
        ShowUI(false);
        _Start();
    }

    protected virtual void _Start()
    {

    }

    // Update is called once per frame
    protected void WhenVideoEnd()
    {
        if (GameMgr.Instance.GetServer<SceneMgr>().IsSceneLoaded("Gallery"))
        {
            GameMgr.Instance.GetServer<SceneMgr>().ReturnScene(true);
        }
        else
        {
            GameMgr.Instance.GetServer<SceneMgr>().ChangeScene(sceneWhenVideoEnd, nextScene);
        }
    }

    public virtual void ShowUI(bool b)
    {

    }

    public virtual void OnCancel()
    {

    }

    public void OnSubmit()
    {
        CheckNoCancelButton();
    }
    public void OnCopy_paste()
    {
        CheckNoCancelButton();
    }
    public void OnDelete()
    {
        CheckNoCancelButton();
    }
    public void OnApply()
    {
        CheckNoCancelButton();
    }
    public void OnChangeGallery()
    {
        CheckNoCancelButton();
    }
    public void OnButtonNorth()
    {
        CheckNoCancelButton();
    }
    public void OnShoulderTrigger()
    {
        CheckNoCancelButton();
    }

    protected virtual void _OnCancelFinish()
    {

    }

    protected virtual void CheckNoCancelButton()
    {

    }
}
