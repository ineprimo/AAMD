using UnityEngine;
using System.Collections;

/// <summary>
/// Prebuild game objects. Clase que podemos colocar en la raiz de la escena y que informa al SpawnerMgr de la lista
/// de objetos precargados que necesita la escena.
/// </summary>
public class PrebuildGameObjects : MonoBehaviour
{
	
	[System.Serializable]
	public class CacheData
	{
		public GameObject prefab;
		public int cacheSize;
        public Transform parent;
	}

	public CacheData[] m_objectCache;
    protected bool m_isInstanciate = false;
    public bool _clearCache = true;
    public bool deferredInit = false;


    void Start()
    {
        if (!deferredInit)
        {
            SceneMgr sceneMaganer = GameMgr.Instance.GetServer<SceneMgr>();
            if (sceneMaganer.IsLoadingFinish())
            {
                GameMgr.Instance.GetSpawnerMgr().InstanciateInitialObjects(this, _clearCache);
                m_isInstanciate = true;
            }
        }
	}

    void Update()
    {
        if(!m_isInstanciate)
        {
            GameMgr.Instance.GetSpawnerMgr().InstanciateInitialObjects(this, _clearCache);
            m_isInstanciate = true;
        }
    }

}
