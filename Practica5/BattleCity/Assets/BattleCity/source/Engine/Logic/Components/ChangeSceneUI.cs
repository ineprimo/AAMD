using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mind;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif
/// <summary>
/// Gestiona el cambio de escena desde la UI de Unity (sólo disponible a partir de la 4.6)
/// </summary>
public class ChangeSceneUI : DemoComponent {

    public string m_nextSceneAfterTransitionScene;
    public bool m_return;
    [Tooltip("El efecto de pulsar el botón se retarda un tiempo, esto solo afecta si su valor es > 0")]
    public float deferredTimeToLoad;
    [Tooltip("Solo valido para crear demos, se ignoran en el resto de builds")]
    [Demo]
    public string m_nextSceneAfterTransitionSceneDemo;
    [Programming]
    public SetVolatile setVolatile;
    public PlayerInput playerInput;
    

    /// <summary>
    /// El método es llamado por el sistema de eventos de la UI y se le pasa por parámetro la escena que quiere cargar. Adicionalmente peude tener establecida como variable pública
    /// en el inspector una siguiente escena a cargar, en caso de que la escena de carga sea una pantalla de carga.
    /// </summary>
    /// <param name="sceneToGo"></param>
    public virtual void OnChangeScene(string sceneToGo)
    {
        if (deferredTimeToLoad > 0f)
        {
            if(playerInput != null)
                playerInput.DeactivateInput();
            GameMgr.Instance.TimeMgr.DeferredCall(this, deferredTimeToLoad, _OnChangeScene, sceneToGo);
        }
        else
            _OnChangeScene(sceneToGo);
    }

    protected virtual void _OnChangeScene(object sceneToGoObj)
    {
        string sceneToGo = (string) sceneToGoObj;

        if (setVolatile != null)
            setVolatile.Apply();
        if (GameMgr.Instance.ISDEMO)
            GameMgr.Instance.GetServer<SceneMgr>().ChangeScene(m_nextSceneAfterTransitionSceneDemo, m_nextSceneAfterTransitionScene);
        else
            GameMgr.Instance.GetServer<SceneMgr>().ChangeScene(sceneToGo, m_nextSceneAfterTransitionScene);

    }


    /// <summary>
    /// El método es llamado por el sistema de eventos de la UI para cerrar la aplciación.
    /// </summary>
    /// <param name="quit"></param>
    public void OnQuitScene(bool quit)
    {
        if (deferredTimeToLoad > 0f)
        {
            if (playerInput != null)
                playerInput.DeactivateInput();
            GameMgr.Instance.TimeMgr.DeferredCall(this, deferredTimeToLoad, _OnQuitScene, quit);
        }
        else
            _OnChangeScene(quit);
    }

    public void _OnQuitScene(object quitObj)
    {
        bool quit = (bool)quitObj;
#if UNITY_EDITOR
        if (quit)
            EditorApplication.isPlaying = false;
#else
        if (quit)
            Application.Quit();
#endif
    }
}
