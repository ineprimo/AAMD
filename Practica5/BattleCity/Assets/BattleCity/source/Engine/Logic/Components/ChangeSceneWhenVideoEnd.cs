using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(VideoController))]
public class ChangeSceneWhenVideoEnd : ChangeSceneWhenPressButton
{

    private VideoController _videoController;



    // Start is called before the first frame update
    protected override void _Start()
    {
        _videoController = GetComponent<VideoController>();

        if (_videoController._videoPlayer.clip == null)
        {
            Debug.LogWarning("No tenemos el video asignado, saltamos la intro");
            GameMgr.Instance.GetServer<SceneMgr>().ChangeScene(sceneWhenVideoEnd, nextScene);
        }
        else
        {
            _videoController.Register(WhenVideoEnd);
            //GameMgr.Instance.GetServer<InputMgr>().LockGameplayEvents(true);
            //GameMgr.Instance.GetServer<InputMgr>().RegisterUIAccept(OnCancel);
        }
    }



    protected override void _OnCancelFinish()
    {
        _videoController.Pause();
    }



    private void OnDestroy()
    {
        if (_videoController != null)
            _videoController.Unregister(WhenVideoEnd);
    }


}
