using UnityEngine;
using System.Collections;
using System;
using System.Runtime.CompilerServices;
/// <summary>
/// Game mgr. Singleton que gestiona todos los managers del juego.
/// </summary>
/// 
public class GameMgr 
{
    public const string CONFIGURATION_FOLDER = "Config";
    public const string CONFIGURATION_FILE = "BaseConfig";
    public const string LOCALIZATION_FILE = "LocalizationConfig";
    public const string SOUND_RESOURCES_FILE = "SoundResources";
    /// <summary>
    /// Clase de la que hay que heredar para añadir managers specíficos del juego que desarrollemos.
    /// </summary>

    public bool ServerIntegrityOk
    {
        get
        {
            return m_servers != null && m_servers.activeInHierarchy;
        }
    }
    public static object Deserializer(string type, string data)
    {

        object obj = null;
        if (type == typeof(bool).ToString())
        {
            obj = System.Convert.ToBoolean(data);
        }
        else if (type == typeof(int).ToString())
        {
            obj = System.Convert.ToInt32(data);
        }
        else if (type == typeof(string).ToString())
        {
            obj = data;
        }
        else if (type == typeof(float).ToString())
        {
            obj = System.Convert.ToSingle(data);
        }
        else if ((type == typeof(Vector2).ToString()) || (type == typeof(Vector3).ToString()) || (type == typeof(Quaternion).ToString()))
        {
            //procesamos la cadena...
            string vector2Str = data.Substring(1);
            vector2Str = vector2Str.Substring(0, vector2Str.Length - 1);
            string[] vectorComponent = vector2Str.Split(',');
            Assert.AbortIfNot(vectorComponent.Length >= 2 && vectorComponent.Length <= 4, "Incorrect Format");
            string xStr = vectorComponent[0].Trim();
            string yStr = vectorComponent[1].Trim();
            if (type == typeof(Vector2).ToString())
            {
                obj = new Vector2(System.Convert.ToSingle(xStr), System.Convert.ToSingle(yStr));
            }
            else
            {
                string zStr = vectorComponent[2].Trim();
                if (type == typeof(Vector3).ToString())
                {
                    obj = new Vector3(System.Convert.ToSingle(xStr), System.Convert.ToSingle(yStr), System.Convert.ToSingle(zStr));
                }
                else
                {
                    string wStr = vectorComponent[3].Trim();
                    obj = new Quaternion(System.Convert.ToSingle(xStr), System.Convert.ToSingle(yStr), System.Convert.ToSingle(zStr), System.Convert.ToSingle(wStr));
                }
            }

        }
        else
        {
            Assert.AbortIfNot(false, "Incompatible Format: " + type);
        }
        return obj;
    }



    //Implementacion del storageMng que nos permite declarar los tipos de datos que permitimos almacenar en nuestros objetos.
    //TODO AllowedType
    [AllowedTypeToStorage(typeof(bool))]
    [AllowedTypeToStorage(typeof(int))]
    [AllowedTypeToStorage(typeof(string))]
    [AllowedTypeToStorage(typeof(float))]
    [AllowedTypeToStorage(typeof(Vector2))]
    [AllowedTypeToStorage(typeof(Vector3))]
    [AllowedTypeToStorage(typeof(Vector4))]
    [AllowedTypeToStorage(typeof(Quaternion))]
	private class StorageMgrImp : StorageMgr
	{
        public StorageMgrImp() : base(GameMgr.Deserializer) { }
	}
	
	/// <summary>
	/// Gets the instance of the GameMgr.
	/// </summary>
	/// <returns>
	/// The instance.
	/// </returns>
    
	public static GameMgr Instance
	{
        get
        {
            if (_instance == null)
            {
                _instance = new GameMgr();
            }
            /*if (!_instance.CheckIntegrity())
                _instance.Start();*/
            return _instance;
        }
	}

    public GameMgrConfig GameMgrConfigData
    {
        get
        {
            return m_gameMgrConfig;
        }
    }

    public static bool InstanceIsNull
    {
        get
        {
            return _instance == null;
        }
    }
	
	/// <summary>
	/// Initializes a new instance of the <see cref="GameMgr"/> class.
	/// </summary>
    private GameMgr()
	{
        Start();
    }

    public bool ServersAreNull
    {
        get { return m_servers == null; }
    }

    public void Destroy()
    {
        if (m_servers != null)
        {
            UnityEngine.Object.DestroyImmediate(m_servers);
            m_servers = null;
        }
    }

    public void Start()
    {
        ProcessBaseConfiguration();
        m_storageMgr = new StorageMgrImp();
        m_achievements = new AchievementsMgr(m_gameMgrConfig.m_achievements);
        //Inicializamos el servidorPrincipal y le registramos todos los servidores...

        if (m_servers == null)
        {
            try
            {
                m_servers = GameObject.Find("Servers");
                if (m_servers != null)
                {
                    GameObject.DestroyImmediate(m_servers);
                    Debug.LogError("En la escena ya exite un servidor previamente, borralo manualmente");
                }
                if (m_servers == null)
                {
                    m_servers = new GameObject("Servers");
                }

                DetectDirtyGameMgr detectDirtyGameMgr = AddServer<DetectDirtyGameMgr>();
                detectDirtyGameMgr.Init();
                //Registramos todos los servidores...
                //InputServer: servidor de entrada.
                //TimeServer: Gestion de llamadas a eventos de tiempo. Metodos que se llaman cada cierto tiempo (periodoco) o alarmas de un solo uso.
                //SceneMgr: Gestiona la carga de escenas... 
                AddServer<SceneMgr>();
                BaseQualityMgr q =AddServer<BaseQualityMgr>();
                q.PlatformVariant = m_gameMgrConfig.GetPlatformVariant();
                q.useQS = m_gameMgrConfig.m_useQualitySetting;
                AddServer<CoroutineMgr>();
                /*SceneMgr sm = m_servers.GetComponent<SceneMgr>();
                if(sm != null)
                    Component.DestroyImmediate(sm);
                m_servers.AddComponent<SceneMgr>();*/
                _cachedTimeMgr = AddServer<TimeMgr>();

                MemoryMgr memoryMgr = AddServer<MemoryMgr>();
                memoryMgr.Configure(m_MM_Active, m_MM_maxFramerateToRecolect, m_MM_maxTimeToRecoect, m_MM_RecolectUnityAssets);

                _cachedSoundMgr = AddServer<SoundMgr>();
                _cachedSoundMgr.Configure(_soundResources);
                AddServer<ScreenShootMgr>();

                _cachedRumbleMgr = AddServer<RumbleMgr>();

                LocalizationManager localizationMgr = AddServer<LocalizationManager>();
                localizationMgr.Configure(_localizationConfig);
            }
            catch(Exception e)
            {
                Debug.Log("Error al intentar cargar los servers");
            }

        }




        //m_localization.CurrentLangguaje = Localization.Langgage.SPANISH;
        //Debug.Log(Localization.Translate("TITLE"));
        SceneMgr smAux = m_servers.GetComponent<SceneMgr>();
        m_spawnerMgr = new SpawnerMgr(smAux);
        
    }

    public StorageMgr GetStorageMgr()
	{
		return m_storageMgr;
	}

    public AchievementsMgr GetAchievementsMgr()
    {
        return m_achievements;
    }

    public GUIControllerBase GetGUIController()
    {
        return m_gUIControllerBase;
    }

    public SpawnerMgr GetSpawnerMgr()
	{
		return m_spawnerMgr;
	}

    public TimeMgr TimeMgr
    {
        get
        {
            if(_cachedTimeMgr == null)
                _cachedTimeMgr = GetServer<TimeMgr>();
            return _cachedTimeMgr;
        }
    }

    public SoundMgr SoundMgr
    {
        get
        {
            if (_cachedSoundMgr == null)
                _cachedSoundMgr = GetServer<SoundMgr>();
            return _cachedSoundMgr;
        }
    }

    public RumbleMgr RumbleMgr
    {
        get
        {
            if (_cachedRumbleMgr == null)
                _cachedRumbleMgr = GetServer<RumbleMgr>();
            return _cachedRumbleMgr;
        }
    }




    public bool ExistServer(string name)
	{
		return m_servers.GetComponent(name) != null;
	}
	
	public bool ExistServer<T>() where T : Component
	{
		return m_servers.GetComponent<T>() != null;
	}
	
	public MonoBehaviour GetServer(string name)
	{
		return m_servers.GetComponent(name) as MonoBehaviour;
	}
	
	public T GetServer<T>() where T : Component
	{
		if(m_servers != null)
			return m_servers.GetComponent<T>();
		else
			return null;
	}
	
	public void Register<T>() where T : Component
	{
		m_servers.AddComponent<T>();
	}



    public bool ISDEMO
    {
        get
        {
            return m_gameMgrConfig.m_demo;
        }
    }



    public void UnRegister<T>() where T : Component
	{
		Component.Destroy(m_servers.GetComponent<T>());
	}


    public T AddServer<T>() where T : Component
    {
        T t = m_servers.GetComponent<T>();
        if (t != null)
            Component.DestroyImmediate(t);
        t = m_servers.AddComponent<T>();
        return t;
    }

    public PlayersMgrBase GetPlayersMgr
    {
        get
        {
            return m_playersMgrBase;
        }
    }


    public CoroutineMgr CoroutineMgr
    {
        get
        {
            return GetServer<CoroutineMgr>();
        }
    }


    public Globals.BUILD_TYPE BuildType
    {
        get
        {
            return m_gameMgrConfig.m_buildType;
        }
    }

    public LogicMap GetLogicMap()
    {
        if (_map == null)
            _map = new LogicMap();
        return _map;
    }

    public Globals.PLATFORMS CurrentPlatform
    {
        get
        {
            return m_gameMgrConfig.m_platform;
        }
    }
    protected void ProcessBaseConfiguration()
	{
        m_gameMgrConfig = ScriptableObjectMgr.Load<GameMgrConfig>(CONFIGURATION_FOLDER+"/"+CONFIGURATION_FILE);
        _localizationConfig = ScriptableObjectMgr.Load<LocalizationConfig>(CONFIGURATION_FOLDER + "/" + LOCALIZATION_FILE);


        //m_localizationFile = gameMgrConfig.m_localizationConfig.File;

        _soundResources = ScriptableObjectMgr.Load<SoundResources>(CONFIGURATION_FOLDER + "/" + SOUND_RESOURCES_FILE);
        //m_storageFileName = gameMgrConfig.m_storageMgrConfig.StorageFileName;

        /*m_MM_Active = gameMgrConfig.m_memoryMgrConfig.ActiveAutoRecolect;
        m_MM_maxFramerateToRecolect = gameMgrConfig.m_memoryMgrConfig.MaxFrameRateToRecolect;
        m_MM_maxTimeToRecoect = gameMgrConfig.m_memoryMgrConfig.TimeSiceLastGarbage;
        m_MM_RecolectUnityAssets = gameMgrConfig.m_memoryMgrConfig.RecollectUnityAssets;*/

    }

    //Statics
    private static GameMgr _instance = null;
	
    //Managers
	private StorageMgr m_storageMgr =   null;
	private SpawnerMgr m_spawnerMgr =   null;
	private GameObject m_servers    =   null;
    private AchievementsMgr m_achievements = null;
    private GUIControllerBase m_gUIControllerBase;
    private GameMgrConfig m_gameMgrConfig;
    private SoundResources _soundResources;
    private LocalizationConfig _localizationConfig;
    private PlayersMgrBase m_playersMgrBase;
    private LogicMap _map;



    //Configuration fields...

    private bool m_MM_Active;
	private float m_MM_maxFramerateToRecolect;
	private float m_MM_maxTimeToRecoect;
	private bool m_MM_RecolectUnityAssets;

    /// Cached servers
    private TimeMgr _cachedTimeMgr = null;
    private SoundMgr _cachedSoundMgr = null;
    private RumbleMgr _cachedRumbleMgr = null;
}
