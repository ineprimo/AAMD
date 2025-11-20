using UnityEngine;
using System.Collections;
using UnityEngine.Events;

/// <summary>
/// Componente que permite cargar el siguiente nivel desde una pantalla de carga dinámica de forma asíncrona. Se presupone que el nivel que ha puesto la pantalla de carga debe
/// haber establecido previamente la variable SCENE_SECTION/NEXT_SCENE con el nombre de la escena a cargar.
/// </summary>
public class AsyncLoader : MonoBehaviour
{
    public UnityEvent OnLoadFinishEvent;
    public float _waitingTimeToEndLoading;
    public bool _confirmation;
    private bool loadingInit = false;
    private AsyncOperation _op;

    void Update () {

        if (!loadingInit)
        {
            loadingInit = true;
            
            Load();
        }
    }
    
    protected void Load()
    {
        //QualitySettings.asyncUploadTimeSlice = 4;
        //QualitySettings.asyncUploadBufferSize = 4;
        string sceneToLoad = GameMgr.Instance.GetStorageMgr().GetVolatile<string>(SceneMgr.SCENE_SECTION, SceneMgr.NEXT_SCENE);
        SceneMgr sceneMgr = GameMgr.Instance.GetServer<SceneMgr>();
        if (!_confirmation)
            sceneMgr.ChangeAsyncScene(sceneToLoad, _waitingTimeToEndLoading);
        else
            _op = sceneMgr.ChangeAsyncSceneConfirmation(sceneToLoad, OnLoadFinish);
    }

    public void OnLoadFinish()
    {
        if (OnLoadFinishEvent != null)
            OnLoadFinishEvent.Invoke();
    }

    public void EndLoading()
    {
        SceneMgr sceneMgr = GameMgr.Instance.GetServer<SceneMgr>();
        sceneMgr.LoadingAsingConfirmationComplete(_op);
    }

}
