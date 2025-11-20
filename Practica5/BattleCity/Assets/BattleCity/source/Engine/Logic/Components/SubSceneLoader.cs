using UnityEngine;
using System.Collections;

//Carga y descarga escena. PErmite activar otros subsceneloaders.
public class SubSceneLoader : MonoBehaviour
{

	// Use this for initialization
	public string[] m_subScenesToLoad;
	public string[] m_subScenesToUnload;
	public bool m_destroyingSubScenes;
	public GameObject m_subSceneLoaderActivate;
    public string m_playerTag = "Player";


    void OnTriggerEnter(Collider other) {
        //Destroyo las subscenas
        if (other.gameObject.tag == m_playerTag)
        {
            SceneMgr sceneMgr = GameMgr.Instance.GetServer<SceneMgr>();
            foreach (string subScene in m_subScenesToUnload)
            {
                sceneMgr.UnloadSubScene(subScene, m_destroyingSubScenes);
            }
            //Cargo las nuevas...
            StartCoroutine(Loading());
		}
	}
	
	
	IEnumerator Loading()
	{
		SceneMgr sceneMgr = GameMgr.Instance.GetServer<SceneMgr>();
		foreach (string subScene in m_subScenesToLoad)
		{
			
			if(sceneMgr.IsLoadingFinish())
			{
				Debug.Log("Loading");
				sceneMgr.LoadSubScene(subScene,true);
			}
			else
			{
				Debug.Log("IsLoadingFinish waiting");
			}
			yield return null; 
		}
		if(m_subSceneLoaderActivate != null)
		{
			m_subSceneLoaderActivate.SetActive(true);
			this.gameObject.SetActive(false);
		}
	}
	
	
}
