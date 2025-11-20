using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SubSceneLoaderWhenAwaking : MonoBehaviour {

    public string[] _subscenesToLoad;
	// Use this for initialization
	void Awake () {
        SceneMgr sceneMgr = GameMgr.Instance.GetServer<SceneMgr>();
        for (int i = 0; i < _subscenesToLoad.Length; ++i)
        {
            if(!sceneMgr.IsSceneLoaded(_subscenesToLoad[i]))
            {
                sceneMgr.LoadSubScene(_subscenesToLoad[i], false);
            }
            else
            {
                sceneMgr.SetSubScene(_subscenesToLoad[i]);
            }
            
        }
       
    }


    private void Start()
    {
        GameMgr.Instance.GetServer<SceneMgr>().AllSubsecenesHaveBeenLoaded();
    }

    private void OnDestroy()
    {
        SceneMgr sceneMgr = GameMgr.Instance.GetServer<SceneMgr>();
        if (sceneMgr != null)
        {
            for (int i = 0; i < _subscenesToLoad.Length; ++i)
            {
                sceneMgr.UnloadSubScene(_subscenesToLoad[i], true);
            }
        }
    }
}
