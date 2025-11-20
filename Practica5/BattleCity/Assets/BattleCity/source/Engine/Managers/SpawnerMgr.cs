using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Spawner mgr. Manager que gestiona la creacion de entidades dle juego dinamicas. Es altamente recomendable siempre utilizar el
/// SpawnerMng y no crear objetos a mano directamente. De esta forma podemos gestionar de ofrma global la instanciacion dew entidades.
/// </summary>
public class SpawnerMgr
{
	public SpawnerMgr(SceneMgr sceneMgr)
	{
		m_sceneMgr = sceneMgr;
		Assert.AbortIfNot(m_sceneMgr != null, "Error: el Scene mgr debe ser distinto de null");
		//Queremos que el manager de escena nos avise cuando haya temrinado la escena para poder destruir nuestra cache de recursos.
		//TODO 1 Registarnos al calback de fin de escena con OnDestroyCurrentScene.
		m_sceneMgr.RegisterDestroyScene(OnDestroyCurrentScene);
        _hackingTeleportPosition = new List<Transform>();
		_hackingExitPositions = new List<GameObject>();
		_visited = new HashSet<Transform>();
	}
	
	protected void OnDestroyCurrentScene()
	{
		Debug.Log("OnDestroyCurrentScene");
		ClearCache();
        ClearHackingPoint();
	}
	//Crea un gameobject a partir de un prefab y otro gameobject, haciendo un clon del mismo. Permite ademas posicionarlo
	//y orientarlo.
	public GameObject CreateNewGameObject(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
	{
		GameObject instance = null;
		//Si lo tenemos en la cache, lo reciclamos. (OJO al crear los componentes, estos deben estar pensados para ser reiniciados)
		//TODO 2 Si está en cache, lo recoperamos, lo activamos y lo ponemos y lo sacamos de a lista.
		if(m_cache.ContainsKey(prefab.name))
		{
			HashSet<GameObject> set = m_cache[prefab.name];
			if(set.Count > 0)
			{
				foreach (GameObject i in set)
                {
					instance = i;
					set.Remove(instance);
					if (instance != null)
                    {
						instance.SetActive(true);
						instance.transform.position = position;
						instance.transform.rotation = rotation;
					}
					break;
				}

			}
		}
		//si no lo teniamos en la cache, lo creamos.
		if(instance == null)
		{
			//No tenemos una instancia creado...
			//TODO 3: instanciamos el objeto.
			instance = Object.Instantiate(prefab,position,rotation) as GameObject;
			//Los objetos estan nombrados con le nombre del prefab original, seguido de @ y un numero.
			//Esto es asi para poder obtener luego el prefab original que los instancio.
			SetAsSpawneableGO(instance, prefab);
			SpawnerRoot root = SpawnerRoot.Get();
			if(root != null)
				instance.transform.parent = root.transform;
		}
		if (instance != null && parent != null)
			instance.transform.parent = parent;
		return instance;
	}

	public void SetAsSpawneableGO(GameObject instance, GameObject prefab)
    {
		instance.name = prefab.name + "@" + m_staticIDs++;
	}

	public string GetOriginlPrefabName(GameObject prefab)
    {
		string originalPrefabName = prefab.name;
		//obtenemos el nombre del prefab original...
		if (prefab.name.IndexOf("@") >= 0)
		{
			//Obtengo el nombre del prefab original
			originalPrefabName = prefab.name.Split('@')[0];
		}
		return originalPrefabName;
	}
	//"destruimos" un objeto. Un objeto puede estar destruido o desactivado, dependiendo de lo que decidamos.
	//Si esta desactivado, lo meteremos en la cache.
	public void DestroyGameObject(GameObject prefab, bool clear = false)
	{
		if(clear)
			GameObject.Destroy(prefab);
		else
		{
			prefab.SetActive(false);
			string originalPrefabName  = GetOriginlPrefabName(prefab);

			//Miro en la cache is el prefab original esta cacheado.
			if (!m_cache.ContainsKey(originalPrefabName))
			{
				//Si no existe lo creo y lo añado
				HashSet<GameObject> set = new HashSet<GameObject>();
				set.Add(prefab);
				m_cache.Add(originalPrefabName, set);
			}
			else
			{
				//Si existe lo añado.
				HashSet<GameObject> list = m_cache[originalPrefabName];
				list.Add(prefab);
			}
		}
	}
	
	public void ClearCache()
	{
		foreach(HashSet<GameObject> a_list in m_cache.Values)
		{
			foreach( GameObject go in a_list)
			{
				GameObject.Destroy(go);
			}
			a_list.Clear();
		}
		m_cache.Clear();
	}
	
	//Instanciamos los objetos precargados iniciales de la escena.
	public void InstanciateInitialObjects(PrebuildGameObjects gameObjects, bool clearCache)
	{
		if(clearCache)
			ClearCache();
		foreach(PrebuildGameObjects.CacheData cd in gameObjects.m_objectCache)
		{
			//Creo los objetos y los almaceno en la cache...
			HashSet<GameObject> set = new HashSet<GameObject>();
			for(int i = 0; i < cd.cacheSize; ++i)
			{
				GameObject newObject = Object.Instantiate(cd.prefab,Vector3.zero,Quaternion.identity) as GameObject;
				SetAsSpawneableGO(newObject, cd.prefab);
				newObject.SetActive(false);
				set.Add(newObject);
				SpawnerRoot root = SpawnerRoot.Get();
                if(cd.parent == null)
				    newObject.transform.parent = root.transform;
                else
                    newObject.transform.parent = cd.parent;
            }
			m_cache.Add(cd.prefab.name, set);
		}
	}
	
	//obtenemos le ultimo punto de spawn del player.
	public Transform GetPlayerSpawnerPoint()
	{
		return m_lastRespawnPoint;
	}
	//Alacenamos el ultimo punto de respawn del player.
	public void ChangeSpawnPoint(Transform spawnPoint)
	{
		m_lastRespawnPoint = spawnPoint;
	}

    public void AddHackingTeleportPosition(Transform position)
    {
        _hackingTeleportPosition.Add(position);
	}

	public void AddHackingExitPositions(GameObject go)
    {
		_hackingExitPositions.Add(go);
	}

	public void RemoveHackingExitPositions(GameObject go)
	{
		_hackingExitPositions.Remove(go);
	}

	public void RemoveHackingTeleportPosition(Transform position)
	{
		_hackingTeleportPosition.Remove(position);
		_visited.Remove(position);
	}

	public void AddVisitedHackingPoint(Transform position)
	{
		_visited.Add(position);
	}

	

	

	public List<GameObject> HackingExitPositions
    {
        get
        {
			return _hackingExitPositions;

		}
    }


	public void ClearHackingPoint()
    {
        _hackingTeleportPosition.Clear();
		_visited.Clear();
		_hackingExitPositions.Clear();

	}



	private List<Transform> _hackingTeleportPosition;
	private List<GameObject> _hackingExitPositions;
	private HashSet<Transform> _visited;
	private Transform m_lastRespawnPoint;
	private Dictionary<string,HashSet<GameObject>> m_cache = new Dictionary<string, HashSet<GameObject>>();
	private SceneMgr m_sceneMgr;
	private static int m_staticIDs = 0;
}
