using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public delegate void OnSceneEventy();

public delegate void OnAsyncLoadingProgress(float progress, bool finish);

/// <summary>
/// Scene mgr: Es el encargado de manejar la carga y descarga de escenas...
/// </summary>
public class SceneMgr : MonoBehaviour
{
    //NEXT_SCENE y SCENE_SECTION alamcenan en la memoria volatil entre escenas
    //la inormación de la siguiente escena a cargar. Esto es util si queremos construir un
    //una pantalla de carga.
    public const string NEXT_SCENE = "next_scene";
    public const string FROM_SCENE = "from";
    public const string FROM_SCENE_BOSS = "boss";
    public const string SCENE_SECTION = "scene";
    public const float LOAD_READY_PERCENTAGE = 0.9f;
    public const string FUSE_COLOR = "fuse_color";
    public const string FUSE_TIME = "fuse_time";
    public const string COLLECTABLE_SECTION = "COLLECTABLES";
    public const string COLLECTABLE_ID = "ID";
    public const string CURRENT_COLLECTED = "COLLECTED";
    public const string CURRENT_COLLECTED_ID = "ID";
    public const string LOAD_SUBLEVEL_DEBUG = "load_sublevel_debug";

    protected struct TSceneInfo
	{
		public string name;
		public Dictionary<string,SubSceneInfo> subScenes;
	}

    //Delegados para gestión de eventos dle SceneMgr. OnSceneDestroy y OnAsyncLoadingProgress

    public void RegisterDestroyScene(OnSceneEventy destroy)
	{
		m_sceneDestroyCallbacks += destroy;
	}
	
	public void UnRegisterDestroyScene(OnSceneEventy destroy)
	{
		m_sceneDestroyCallbacks -= destroy;
	}

    public void RegisterAllSubSceneLoaded(OnSceneEventy destroy)
    {
        m_allSubSceneLoaded += destroy;
    }

    public void UnRegisterAllSubSceneLoaded(OnSceneEventy destroy)
    {
        m_allSubSceneLoaded -= destroy;
    }


    public void AllSubsecenesHaveBeenLoaded()
    {
        if (m_allSubSceneLoaded != null)
            m_allSubSceneLoaded();
    }
    

    public void RegisterLoadingSceneProgressCallback(OnAsyncLoadingProgress callback)
    {
        m_AsyncLoadingSceneProgress += callback;
    }

    public void UnRegisterLoadingSceneProgressCallback(OnAsyncLoadingProgress callback)
    {
        m_AsyncLoadingSceneProgress -= callback;
    }

    public void RegisterLoadingAdditiveProgressCallback(string subScene, OnAsyncLoadingProgress callback)
    {
        if (!m_AsyncLoadingAditiveProgress.ContainsKey(subScene))
        {
            m_AsyncLoadingAditiveProgress.Add(subScene, callback);
        }
        else
        {
            m_AsyncLoadingAditiveProgress[subScene] += callback;
        }
    }

    public void UnRegisterLoadingAdditiveProgressCallback(string subScene, OnAsyncLoadingProgress callback)
    {
        if (m_AsyncLoadingAditiveProgress.ContainsKey(subScene))
        {
            OnAsyncLoadingProgress progress = m_AsyncLoadingAditiveProgress[subScene] ;
            progress -= callback;
            if (progress == null)
            {
                m_AsyncLoadingAditiveProgress.Remove(subScene);
            }
        }
    }

    //Método para cambiar la escena de foram asincrona.
    public void ChangeAsyncScene(string sceneToLoad, float waitingTimeToTheEnd = 0)
    {
        if (m_numSubSceneLoading == 0)
        {
            m_justAsyncLoader = true;
            StartCoroutine(LoadingAsync(sceneToLoad, waitingTimeToTheEnd));
        }
        else
            m_deferredSceneChange = sceneToLoad;
    }

    //Método para cambiar la escena de foram asincrona.
    public AsyncOperation ChangeAsyncSceneConfirmation(string sceneToLoad, System.Action onSceneLoadingFinish)
    {
        if (m_numSubSceneLoading == 0)
        {
            m_justAsyncLoader = true;
            AsyncOperation op=LoadingAsyncConfirmationInit(sceneToLoad);
            if(op == null)
            {
                Debug.LogError("La escena " + sceneToLoad + " no ha podido ser encontrada, comprueba que este asignada en el buidlSetting y que esté activada ");
                return op;
            }
            StartCoroutine(LoadingAsyncConfirmationProgress(op, onSceneLoadingFinish,sceneToLoad));
            return op;
        }
        return null;
    }

    /// <summary>
    /// Changes the scene: Cambia la escena actual por otra nueva. La escena anterior se elimina.
    /// </summary>
    /// <param name='scene'>
    /// SceneName: El nombre de la escena a cargar
    /// </param>
    /// <param name='next'>
    /// Next. Nombre de la siguiente escena a cargar.... Si queremos hacer una pantalla de carga, por ejemplo
    /// </param>
    public void ChangeScene(string sceneName, string next = "")
	{
		//Eliminamos la pila de escenas ya que vamos a renovar completamente toda la escena.
		m_stackScenes.Clear();

		//TODO 4 Si next es distinto de "" lo guardamos en el amacenamiento volatil para que la siguiente escena pueda leerla.
		if(next != "" || next == null)
		{
			GameMgr.Instance.GetStorageMgr().SetVolatile(SceneMgr.SCENE_SECTION, SceneMgr.NEXT_SCENE, next);
		}
		//Avisamos de que vamso a destruir la escena...
        //TODO 5 avisamos a los delegados de destrucción de escena.
        if(m_sceneDestroyCallbacks != null)
			m_sceneDestroyCallbacks();
		GameMgr.Instance.GetServer<MemoryMgr>().GarbageRecolect(true);
        //Cargamso la escena
        
		//TODO 6: cargamso la escena y la almacenamos en la cima de la pila usando StoreLevelInfoInStack
        SceneManager.LoadScene(sceneName);
        m_desactiveGameObject.Clear();

        StoreLevelInfoInStack(sceneName);
	}
	
	/// <summary>
	/// Pushs the scene: Permite apilar escenas unas sobre otras. Es util para mantener todo el contenido en una misma escena y poder
	/// volver rapido a la escena anterior sin necesidad de volver a cargarla.
	/// </summary>
	/// <returns>
	/// La AsyncOperation que nos permite saber si dicha escena esta siendo cargada de forma asincrona
	/// </returns>
	/// <param name='sceneName'>
	/// Scene name: Nombre de la escena que queremos apilar.
	/// </param>
	/// <param name='asyn'>
	/// Asyn: parametro que determina si la carga sera sincrona o asincrona...
	/// </param>
	public void PushScene(string sceneName,bool asyn = false, System.Action onScenePushed = null)
	{
		//Suspendemos la escena actual.
		// Si no hay escena que suspender (es decir apilamos la primera escena, no la suspendemos.
		if(m_stackScenes.Count > 0)
		{
			TSceneInfo current = m_stackScenes.Peek();
			SuspendScene(current);
		}
		
		//Si ya tenemos la escena cargada de antes, pero esta desactivada, no la cargamos, simplemente la activamos...
		if(m_desactiveGameObject.ContainsKey(sceneName))
		{
            //TODO 1: leemos del diccionario m_desactiveGameObject el nombre de la escena  para obtenerla y apilarla de nuevo usando StoreLevelInfoInStack
            //Activamos la escena y eliminamos la escena de las escenas desactivadas m_desactiveGameObject
			List<GameObject> goScenes = m_desactiveGameObject[sceneName];
			StoreLevelInfoInStack(sceneName);
            for(int i = 0; i < goScenes.Count; i++)
            {
                if (goScenes[i] != null)
                    goScenes[i].SetActive(true);
            }

			m_desactiveGameObject.Remove(sceneName);
            if (onScenePushed != null)
                onScenePushed();
        }
		else
		{
			//Carga de la escena
			if(asyn)
			{
                m_numSubSceneLoading++;
				//TODO 2: lanzamos la corrutina LoadingAdditiveAsync pasándole como parámetro LoadingAdditiveAsyncParam
                StartCoroutine("LoadingAdditiveAsync", new LoadingAdditiveAsyncParam( sceneName, true, onScenePushed));
            }
			else
			{
                SceneManager.LoadScene(sceneName,LoadSceneMode.Additive);
				StoreLevelInfoInStack(sceneName);
			}
		}
	}

    protected struct LoadingAdditiveAsyncParam
    {
        public string sceneName;
        public bool inStack;
        public System.Action onSceneLoaded;

        public LoadingAdditiveAsyncParam(string a_s, bool a_i, System.Action action)
        {
            sceneName = a_s;
            inStack = a_i;
            onSceneLoaded = action;
        }
    }

    protected IEnumerator LoadingAsync(string sceneName, float waitingTime)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        //Como vamso a hacer un cambio de escena y las escenas actuales va na ser destruidas
        //no hace falta esperar a cambiar de escena para reescribir nuestra estructura de datos
        //de la escena. Así evitamos que se inicie antes el start de la siguiente escena que 
        //la actualización de los métodos de las corrutinas. "Recordad el flujo de llamadas!!!!!"
        m_justAsyncLoader = true;
        m_stackScenes.Clear();
        StoreLevelInfoInStack(sceneName);
        op.allowSceneActivation = false;
        do
        {
            if (m_AsyncLoadingSceneProgress != null)
                m_AsyncLoadingSceneProgress(op.progress, op.isDone);

            yield return null;
        } while (op.progress < LOAD_READY_PERCENTAGE);

        if (m_AsyncLoadingSceneProgress != null)
            m_AsyncLoadingSceneProgress(1, op.isDone);
        if(waitingTime > 0)
            yield return new WaitForSeconds(waitingTime);
        op.allowSceneActivation = true;
        m_justAsyncLoader = false;
        yield return null;
    }

    protected AsyncOperation LoadingAsyncConfirmationInit(string sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        return op;
    }

    protected IEnumerator LoadingAsyncConfirmationProgress(AsyncOperation op, System.Action onComplete, string sceneName)
    {
        //Como vamso a hacer un cambio de escena y las escenas actuales va na ser destruidas
        //no hace falta esperar a cambiar de escena para reescribir nuestra estructura de datos
        //de la escena. Así evitamos que se inicie antes el start de la siguiente escena que 
        //la actualización de los métodos de las corrutinas. "Recordad el flujo de llamadas!!!!!"
        m_justAsyncLoader = true;
        m_stackScenes.Clear();
        StoreLevelInfoInStack(sceneName);
        op.allowSceneActivation = false;
        do
        {
            if (m_AsyncLoadingSceneProgress != null)
                m_AsyncLoadingSceneProgress(op.progress, op.isDone);

            yield return null;
        } while (op.progress < LOAD_READY_PERCENTAGE);

        if (m_AsyncLoadingSceneProgress != null)
            m_AsyncLoadingSceneProgress(1, op.isDone);
        if (onComplete != null)
            onComplete();

        yield return null;
    }

    public void LoadingAsingConfirmationComplete(AsyncOperation op)
    {
        op.allowSceneActivation = true;
        m_justAsyncLoader = false;
    }

    protected IEnumerator LoadingAdditiveAsync(LoadingAdditiveAsyncParam param)
    {
    	//TODO 3.a hacemos la carga aditiva sobre la variale op. (solución 005.a)
        AsyncOperation op = SceneManager.LoadSceneAsync(param.sceneName,LoadSceneMode.Additive);
        do
        {
            if (m_AsyncLoadingAditiveProgress.ContainsKey(param.sceneName))
            {
                OnAsyncLoadingProgress progress = m_AsyncLoadingAditiveProgress[param.sceneName];
                if (progress != null)
                    progress(op.progress, op.isDone);
            }
            //TODO 3.b yield return WaitForEndOfFrame bloquea la corrutina hasta el siguiente frame (solución 005.b)
            yield return new WaitForEndOfFrame();
        } while (!op.isDone);

        if (param.inStack)
            StoreLevelInfoInStack(param.sceneName);
        else
            StoreSubSceneInCurrentScene(param.sceneName);
        m_numSubSceneLoading--;

        if (param.onSceneLoaded != null)
            param.onSceneLoaded();
        yield return null;
    }

    /// <summary>
    /// Returns the scene. Vuelve a la escena anterior.
    /// </summary>
    /// <param name='clearCurrentScene'>
    /// Clear current scene.
    /// </param>
    public void ReturnScene(bool clearCurrentScene)
	{
		Assert.AbortIfNot(m_stackScenes.Count > 1,"Error, No hay escena a la cual volver");
		
		TSceneInfo current = m_stackScenes.Pop();
		
		//NOTA: Puede querer destruirla? en principio si...
		if(clearCurrentScene)
			DestroyScene(current);
		else
			DesactiveScene(current);
		
		TSceneInfo previousScene = m_stackScenes.Peek();
		BackToLifeScene(previousScene);
	}

    /// <summary>
    /// Método permite convertir un Gameobject raiz en una subscena.
    /// </summary>
    /// <param name="subSceneRoot"></param>
    public void AddSubScene(GameObject subSceneRoot)
    {
        Assert.AbortIfNot(subSceneRoot.activeInHierarchy, "No podemos añadir gameobjects desactivados como subscenas "+ subSceneRoot.name);

        TSceneInfo info = m_stackScenes.Peek();
        if(info.subScenes.ContainsKey(subSceneRoot.name))
        {
            //Si ya tenemos la escena miramos si esta desactivada y si lo está la activamos.
            if (m_desactiveGameObject.ContainsKey(subSceneRoot.name))
            {
                StoreSubSceneInCurrentScene(subSceneRoot.name);
                List<GameObject> goSubScenes = m_desactiveGameObject[subSceneRoot.name];
                for(int i = 0; i < goSubScenes.Count; i++)
                {
                    goSubScenes[i].SetActive(true);
                }
                m_desactiveGameObject.Remove(subSceneRoot.name);
            }
            // Si ya estab activada no hacemos nada.
        }
        else
        {
            // Si estaba añadida, la añadimos.
            Scene existingScene = SceneManager.GetSceneByName(subSceneRoot.name);

            //Si no existe, la creamos y añadimos el GO a la escena nueva.
            if (!existingScene.IsValid())
            {
                existingScene = SceneManager.CreateScene(subSceneRoot.name);
                SceneManager.MoveGameObjectToScene(subSceneRoot, existingScene);
            }

            //Se guarda la escena en cache. 
            StoreSubSceneInCurrentScene(subSceneRoot.name);
        }
        
    }

    public void SetSubScene(string sceneName)
    {
        Scene s = SceneManager.GetSceneByName(sceneName);
        if(s.IsValid())
        {
            StoreSubSceneInCurrentScene(sceneName);
        }
    }
    public bool IsSceneLoaded(string sceneName)
    {
        Scene s = SceneManager.GetSceneByName(sceneName);
        return s.IsValid();
        
    }

    /// <summary>
    /// Loads the sub scene. Cargo un fragmento de la escena.
    /// </summary>
    /// <param name='subScene'>
    /// Sub scene. El nombre de la subscena que deseamos cargar
    /// </param>
    /// <param name='asyn'>
    /// Asyn. Parametro que determina si la carga de la escena es sincrona o asincrona.
    /// </param>
    public void LoadSubScene(string subScene, bool asyn = false, System.Action onLoadingFinish = null)
	{
        //si voy a cambiar de escena no cargo subescena porque la escena va a ser destruida.
        if (!m_justAsyncLoader)
        {
            if (!m_stackScenes.Peek().subScenes.ContainsKey(subScene))
            {
                if (m_desactiveGameObject.ContainsKey(subScene))
                {
                	//TODO 7: leemos de las escenas de m_desactiveGameObject y la guardamos en StoreSubSceneInCurrentScene, activamos la escena
                    
                    StoreSubSceneInCurrentScene(subScene);
                    List<GameObject> goSubScenes = m_desactiveGameObject[subScene];
                    for(int i = 0; i > goSubScenes.Count; i++)
                    {
                        goSubScenes[i].SetActive(true);
                    }
                    m_desactiveGameObject.Remove(subScene);
                    if (onLoadingFinish != null)
                        onLoadingFinish();
                }
                else
                {
                    //Carga de la subscena
                    if (asyn)
                    {
                        m_numSubSceneLoading++;
                        //TODO 8 lanzar la corrutina LoadingAdditiveAsync
                        StartCoroutine("LoadingAdditiveAsync", new LoadingAdditiveAsyncParam( subScene, false, onLoadingFinish));
                    }
                    else
                    {
                        SceneManager.LoadScene(subScene, LoadSceneMode.Additive);
                        StoreSubSceneInCurrentScene(subScene);
                        if(onLoadingFinish != null)
                            onLoadingFinish();
                    }
                }
            }
            else
            {
                Debug.LogWarning("La subscena " + subScene + " Ya esta activa en la actual escene: " + m_stackScenes.Peek().name);
            }
        }
	}

    /// <summary>
    /// Unloads the sub scene. Descarga una subscena.
    /// </summary>
    /// <param name='subScene'>
    /// Sub scene. La subscena a descargar
    /// </param>
    /// <param name='clearSubScene'>
    /// Clear sub scene. Si deseo eliminar la subscena por completo o simplemente desactivarla.
    /// </param>
    public void UnloadSubScene(string subScene, bool clearSubScene, System.Action onUnloadFinish = null)
	{
        //si voy a cambiar de escena no cargo subescena porque la escena va a ser destruida.
        if (!m_justAsyncLoader)
        {
            Assert.AbortIfNot(m_stackScenes.Count > 0, "Error no hay ninguna escena cargada");
            Assert.AbortIfNot(subScene != null, "la subescena es null");
            //Obtengo la escena actual.
            TSceneInfo current = m_stackScenes.Peek();

            //Busco la subscena
            //Assert.AbortIfNot(current.subScenes.ContainsKey(subScene), "Error: la subscena a eliminar/desactivar no existe: " + subScene);
            if (!current.subScenes.ContainsKey(subScene) && !subScene.EndsWith(Globals.SCENE_SUFIX))
                subScene = subScene + Globals.SCENE_SUFIX;
            if (current.subScenes.ContainsKey(subScene))
            {
            	//TODO 10: buscamos en las subscenas la subscena a descargar,
            	//buscamos el gameobject, si eliminamos la escena clearSubScene, la destruimos y si no simplemente la desactivamos y la metemos en la lista
            	//de escenas desactivadas.
                SubSceneInfo subSceneInfo = current.subScenes[subScene];
                string debugSubScene = "";
                GameObject subSceneRoot = GetsubSceneRootBySubSceneID(subScene, out debugSubScene);

                if (subSceneRoot == null)
                {
                    Debug.LogError("No hemos podido encontrar la subscena " + subScene +" Para descargar. Las subscenas disponibles son estas "+ debugSubScene);
                }
                else
                {
                    if (clearSubScene)
                    {
                        GameObject.Destroy(subSceneRoot);
                        StartCoroutine(UnloadSceneAsync(subScene, onUnloadFinish));
                    }
                    else
                    {
                        subSceneRoot.SetActive(false);
                        m_desactiveGameObject.Add(subScene, new List<GameObject>() { subSceneRoot });
                        if (onUnloadFinish != null)
                            onUnloadFinish();
                    }
                    current.subScenes.Remove(subScene);
                }
                
            }
            else
            {
                Debug.LogError("No se puede eliminar la subscena " + subScene + " que no esta activa ");
            }
        }
	}

    private GameObject GetsubSceneRootBySubSceneID(string subScene, out string debugSubScene)
    {
        GameObject subSceneRoot = null;
        SubSceneID[] subSceneIDs = GameObject.FindObjectsOfType<SubSceneID>();

        //Buscamos los GameObjects que tengan el nombre igual al nivel (quizá es lento, pero se hace sólo una vez al descargar nivel)
        debugSubScene = "";
        for (int i = 0; i < subSceneIDs.Length; i++)
        {
            if (i < (subSceneIDs.Length - 1))
                debugSubScene += debugSubScene + subSceneIDs[i].ID + ",";
            else
                debugSubScene += debugSubScene + subSceneIDs[i].ID;
            if (subSceneIDs[i].ID == subScene)
            {
                subSceneRoot = subSceneIDs[i].gameObject;
                break;
            }
        }
        return subSceneRoot;
    }


    private IEnumerator UnloadSceneAsync(string sceneName, System.Action onUnloadfinish)
    {
        AsyncOperation op = SceneManager.UnloadSceneAsync(sceneName);
        while(op != null && !op.isDone)
            yield return null;

        if (onUnloadfinish != null)
            onUnloadfinish();
    }
	
	/// <summary>
	/// Gets the name of the current scene.
	/// </summary>
	/// <returns>
	/// The current scene name.
	/// </returns>
	
	public string GetCurrentSceneName()
	{
		return m_stackScenes.Peek().name;
	}

    public GameObject GetCurrentSceneRoot()
    {
        if (m_stackScenes.Count > 0)
        { 
            string rootName = m_stackScenes.Peek().name;
            if (m_cacheSceneRoot == null)
            {
                m_cacheSceneRoot = GameObject.Find(rootName);
            }
            else if (m_cacheSceneRoot.name != rootName)
            {
                m_cacheSceneRoot = GameObject.Find(rootName);
            }
        }
		return m_cacheSceneRoot;
	}
	
	/// <summary>
	/// Gets the number scenes stacked.
	/// </summary>
	/// <returns>
	/// The number scenes stacked.
	/// </returns>
	public int GetNumScenesStacked()
	{
		return m_stackScenes.Count;
	}
	
	public bool IsLoadingFinish()
	{
		return !m_justAsyncLoader;
	}
	
	
	void Awake()
	{
		//Para evitar que se destruya entre escenas.
		DontDestroyOnLoad(this);
		StoreLevelInfoInStack(SceneManager.GetActiveScene().name);
	}
	
	
	protected void DestroyScene(TSceneInfo sceneInfo)
	{
		foreach( SubSceneInfo ssinfo in sceneInfo.subScenes.Values)
		{
            string debugSubScene = "";
            GameObject subSceneRoot = GetsubSceneRootBySubSceneID(ssinfo.m_name, out debugSubScene);    
            m_desactiveGameObject.Remove(ssinfo.m_name);
            Destroy(subSceneRoot);
		}
		GameObject root = GameObject.Find(sceneInfo.name);
        if(m_sceneDestroyCallbacks != null)
		    m_sceneDestroyCallbacks();
		Destroy(root);
        SceneManager.UnloadSceneAsync(sceneInfo.name);
        

    }
	
	protected void BackToLifeScene(TSceneInfo sceneInfo)
	{
        foreach ( SubSceneInfo ssinfo in sceneInfo.subScenes.Values)
		{
			if(!ssinfo.m_active)
			{
				List<GameObject> subScenesRoot = m_desactiveGameObject[ssinfo.m_name];
                for(int i = 0; i < subScenesRoot.Count; i++)
                    subScenesRoot[i].SetActive(true);
				m_desactiveGameObject.Remove(ssinfo.m_name);
				ssinfo.m_active = true;
            }
		}
		List<GameObject> root = m_desactiveGameObject[sceneInfo.name];
        for (int i = 0; i < root.Count; i++)
            root[i].SetActive(true);
        m_desactiveGameObject.Remove(sceneInfo.name);
		

    }
	
	protected void SuspendScene(TSceneInfo sceneInfo)
	{
		foreach( SubSceneInfo ssinfo in sceneInfo.subScenes.Values)
		{
			if(ssinfo.m_active)
			{
                string debugSubScene = "";
                GameObject subSceneRoot = GetsubSceneRootBySubSceneID(ssinfo.m_name, out debugSubScene);
                subSceneRoot.SetActive(false);
				m_desactiveGameObject.Add(ssinfo.m_name,new List<GameObject>() { subSceneRoot });
				ssinfo.m_active = false;
			}
		}
        SceneToSuspendWhenPush[] scenestoSuspend = GameObject.FindObjectsByType<SceneToSuspendWhenPush>(FindObjectsSortMode.None);
        List<GameObject> roots = new List<GameObject>();
        for(int i = 0; i < scenestoSuspend.Length; i++)
        {
            roots.Add(scenestoSuspend[i].gameObject);
            scenestoSuspend[i].gameObject.SetActive(false);
        }
        if (!m_desactiveGameObject.ContainsKey(sceneInfo.name))
            m_desactiveGameObject.Add(sceneInfo.name, roots);
        else
            m_desactiveGameObject[sceneInfo.name] = roots;
        /*List<GameObject> roots = GameObject.Find(sceneInfo.name);
		if(!m_desactiveGameObject.ContainsKey(sceneInfo.name))
			m_desactiveGameObject.Add(sceneInfo.name,root);
		else
			m_desactiveGameObject[sceneInfo.name]=root;
		root.SetActive(false);*/
	}
	
	protected void DesactiveScene(TSceneInfo sceneInfo)
	{
		foreach( SubSceneInfo ssinfo in sceneInfo.subScenes.Values)
		{
            string debugSubScene = "";
            GameObject subSceneRoot = GetsubSceneRootBySubSceneID(ssinfo.m_name, out debugSubScene);
            Destroy(subSceneRoot);
		}
		sceneInfo.subScenes.Clear();
        SceneToSuspendWhenPush[] scenestoSuspend = GameObject.FindObjectsByType<SceneToSuspendWhenPush>(FindObjectsSortMode.None);
        List<GameObject> roots = new List<GameObject>();
        for (int i = 0; i < scenestoSuspend.Length; i++)
        {
            roots.Add(scenestoSuspend[i].gameObject);
            scenestoSuspend[i].gameObject.SetActive(false);
        }
        //GameObject root = GameObject.Find(sceneInfo.name);
        m_desactiveGameObject.Add(sceneInfo.name, roots);
		//root.SetActive(false);
	}
	
	protected SubSceneInfo StoreSubSceneInCurrentScene(string subSceneName)
	{
		TSceneInfo info = m_stackScenes.Peek();
		SubSceneInfo subSceneInfo = new SubSceneInfo();
		subSceneInfo.m_name = subSceneName;
		subSceneInfo.m_active = true;
		//TODO 9: añadir a la scena que está en la cima m_stackScenes.Peek(); la subscena subSceneName (solución 007)
		if(info.subScenes.ContainsKey(subSceneInfo.m_name))
			info.subScenes[subSceneInfo.m_name] = subSceneInfo;
		else
			info.subScenes.Add(subSceneInfo.m_name,subSceneInfo);
		return subSceneInfo;
	}
	
	protected TSceneInfo StoreLevelInfoInStack(string levelName)
	{
		TSceneInfo info = new TSceneInfo();
		info.name = levelName;
		info.subScenes = new Dictionary<string, SubSceneInfo>();
		m_stackScenes.Push(info);
		return info;
	}
	
	
	// Update is called once per frame
	void Update()
	{

        if(m_numSubSceneLoading == 0)
        {
            if (m_deferredSceneChange != null && m_deferredSceneChange != "")
            {
                StartCoroutine(LoadingAsync( m_deferredSceneChange, 0f));
                m_deferredSceneChange = "";
            }
        }
    }
	
	private GameObject m_cacheSceneRoot = null;
	private Stack<TSceneInfo> m_stackScenes = new Stack<TSceneInfo>();
	private Dictionary<string,List<GameObject>> m_desactiveGameObject = new Dictionary<string, List<GameObject>>();
	
	private OnSceneEventy m_sceneDestroyCallbacks;
    private OnSceneEventy m_allSubSceneLoaded;
    private bool m_justAsyncLoader = false;
    private OnAsyncLoadingProgress m_AsyncLoadingSceneProgress = null;
    private int m_numSubSceneLoading = 0;
    private string m_deferredSceneChange = "";
    private Dictionary<string, OnAsyncLoadingProgress> m_AsyncLoadingAditiveProgress = new Dictionary<string, OnAsyncLoadingProgress>();

}
