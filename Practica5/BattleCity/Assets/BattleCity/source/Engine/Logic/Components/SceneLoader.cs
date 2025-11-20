using UnityEngine;
using System.Collections;

//Cargador de subescenas... Podemos utilizarlo para componer una escena por piezas.
public class SceneLoader : MonoBehaviour
{

	public string[] m_subScenesInitialLoaded;
	
	void Start()
	{
		foreach(string ssinfo in m_subScenesInitialLoaded)
		{
			GameMgr.Instance.GetServer<SceneMgr>().LoadSubScene(ssinfo,false);
		}
	}
}
