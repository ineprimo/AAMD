using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using static Globals;

[System.Serializable]
public class SceneManualGrapicPrefabs
{
    [SerializeField]
    protected GameObject _prefab;
    [SerializeField]
    protected Vector3 _position;
    [SerializeField]
    protected Vector3 _rotation;

    public GameObject Prefab
    {
        get { return _prefab; }
    }

    public Vector3 Position
    {
        get { return _position; }
    }

    public Vector3 Rotation
    {
        get { return _rotation; }
    }
}



public class TileMapLoaderV2 : MonoBehaviour
{
    public enum ObjectTiledNames
    {
        Moshroom = 0, MoshroomJumping, Skreppa, Feiknott, Vinfisk, VinshellFly, VinshellWalk, Frokkir, Svugger, SnailWithoutWooden,
        Snail, SpiderBomb, Owl, Flag, Point, MPlatform, Teleport, Device, BricksController, LavaWaterfallController, MovingBlock, Sock, Shield, FeiknottFlameado
    };

    public enum GroundTiledNames
    {
        NADA = 1, SOLIDO = 2, CUESTA_ABAJO_45 = 3
    };

    public enum OtherObjecTiledNames
    {
        Player = 1, ChangeScene = 3, AddScene = 4, CameraZoom = 1000
    }
    /// <summary>
    /// - PHYSICS_GRAPHIC_ONLY Sirve para exportar una escena sólo con la parte físca y grafica del tiled,
    /// sin la parte de lógica. Tipico para escenas que luego se van a crear a mano como la de los bosses.
    /// - ADDITIVE_ONLY Sólo genera una escena que se carga de forma aditiva. En mapas grandes necesitamos este tipo de escenas.
    /// </summary>

    public const string GROUND_LAYER = "Ground";
    public const string GROUND_FRONT = "GroundFront";
    public const string LOGIC_LAYER = "Logic";


    public const string OBJECT_LAYER = "Objetos";
    public const string MIRROR_ONLY_LAYER = "MirrorOnly";
    public const int TILE_EMPTY = 0;

    protected const int LEFT = 0;
    protected const int RIGHT = 1;
    protected const float GROUND_Z = 0F;
    protected string CURRENT_VERSION = "1.2";

    [Header("Scene Exporter Parameters")]
    public bool exportInPrefabMode = true;
    public string _scenePath;
    [SerializeField]
    private string _sceneName = "";
    [SerializeField]
    private string _prefabSubsceneName = "";
    [SerializeField]
    private bool _includeBaseLogic = false;

    [Header("Text Maps SetUp")]
    [ReadOnlyInspector]
    public string _textMapHash;
    [SerializeField]
    private TextAsset[] _textMap;
    [SerializeField]
    private SceneManualGrapicPrefabs[] _sceneGraphicsPrefabs;
    [SerializeField]
    private Vector2 _tileSize;
    [SerializeField]
    private Vector2 _physicTileSize;
    [SerializeField]
    private Transform _sceneRoot;
    [SerializeField]
    private SubSceneControllerBase _subSceneController;
    [SerializeField]
    private Vector3 _positionOffset;

    [Header("Export Settings")]
    [SerializeField]
    private bool _debugLogic;

    [Header("Tiles SetUp")]
    [SerializeField]
    private int offsetLogic;
    [SerializeField]
    private GraphicLayerConfig _graphicLayerConfig;
    [SerializeField]
    private ObjectTileConfig _objectConfig;
    [SerializeField]
    private PhysicTileConfig _groundConfig;
    [SerializeField]
    private Biome _biomeConfig;


    [Header("Prefabs SetUp")]
    [SerializeField]
    private BaseCamera2D _mainCamera;
    [SerializeField]
    private GameObject _basePlayerLogicPrefab;
    [SerializeField]
    [Header("Prefab de tile erroneo")]
    private GameObject _tileErrorDebug;


    [Header("Other settings")]



    private Dictionary<string, LayerInfoV2> _prefabCache;
    private Dictionary<string, List<ObjectMap>> _objetcMaps;
    private Dictionary<string, List<ObjectMap>> _pointsMap;


    private List<ObjectData> _objectTileData;
    private Dictionary<int, ObjectData> _objectTileDataDic;
    private Dictionary<string, ObjectTile> _objectTileConfigDic;
    private Dictionary<int, GraphicTile> _graphicTileConfigDic;
    private Dictionary<int, PhysicTile> _groundTileConfigDic;
    
    //private ObjectTileMappingV2[] _objectTileMapping;
    //private LogicTileMappingV2[] _logicTileMapping;
    //private GroundTileMappingV2[] _groundTileMapping;

    private List<Vector3> _forbiddenPositions;
    private List<Vector3> _LogicWalls;

    private const string tmpSubScenePrefabPath = "Assets/Prefabs/Scenes/TemporalSubScenePref/";

    // Use this for initialization
    void Start()
    {
        //En modo editor, intentemos crear los prefabs con PrefabUtility.InstantiatePrefab(object); y despues ejecutar la escena. para no perder los prefabs
        LoadMapAndGenerateLevel();
        //Creamos la subescena donde ira toda la lógica
        CreatingSubScene();
    }

    public SubSceneControllerBase SubSceneController
    {
        get
        {
            return _subSceneController;
        }
    }

    public string SceneName
    {
        get { return _sceneName; }
    }

    public void LoadMapAndGenerateLevel(bool inEditorMode = false)
    {
        if(inEditorMode)
        {
            ExportVersion v = _subSceneController.GetComponent<ExportVersion>();
            int ver = 0;
            if (v != null)
                ver = v.version;
            Utils.RevertPrefabIfIsPossible(_subSceneController.gameObject);
            v = _subSceneController.GetComponent<ExportVersion>();
            if (v != null)
                v.version = ver;
        }
            
        CreatingTilemappings();
        //_objectTileConfigDic = new Dictionary<string, ObjectTileConfig>();
        _objetcMaps = new Dictionary<string, List<ObjectMap>>();
        _pointsMap = new Dictionary<string, List<ObjectMap>>();
        if (_sceneRoot == null)
            Debug.LogError("sceneRoot is null. It have to be asigned");


        InitCache();

        TileMap map = null;
        for (int i = 0; i < _textMap.Length; ++i)
        {
            string tilemapStr = _textMap[i].text;
            //Limpiamos el JSON para que coja la etiqueta class (Ñapa porque no puedo llamar class a la variable en c#
            tilemapStr = CleanJSON(tilemapStr);
            TileMap map1 = JsonUtility.FromJson<TileMap>(tilemapStr);

            if (i == 0)
            {

                TileSets[] tileSets = map1.Tilesets;
                _objectTileDataDic = new Dictionary<int, ObjectData>();
                for (int j = 0; j < tileSets.Length; ++j)
                {
                    TileSets tileSet = tileSets[j];
                    if (tileSet.Source != null)
                    {
                        string[] sourceSplit = tileSet.Source.Split('/');
                        string tilesetName = sourceSplit[sourceSplit.Length - 1];
                        SetCorrectObjectIdIntoDic(tileSet.Firstgid, tilesetName);
                    }
                }
                map1.ConvertToDictionary();
                map = map1;
                CreateLayers(map);
            }

            if (i != 0)
            {
                map.Add(map1, map1.getPropertyInt("OffsetX"), map1.getPropertyInt("OffsetY"));
                map.ConvertToDictionary();
            }
        }
        CreatePhysicalLayers(map, inEditorMode);

        //GameMgr.Instance.GetLogicMap().MarkBondaries(GROUND_LAYER, _forbiddenPositions);


        CreateObjectWithoutConfiguration(map, inEditorMode);


        CreatingManualGraphics(inEditorMode);
        

        //MapManager mapManager = GetComponent<MapManager>();
        BeginModifyComponent(_subSceneController);
        EndModifyComponent(_subSceneController);
        BeginModifyComponent(_mainCamera);
        
        EndModifyComponent(_mainCamera);
    }

    private void BeginModifyComponent(MonoBehaviour m)
    {
        m.enabled = !m.enabled;
    }

    private void EndModifyComponent(MonoBehaviour m)
    {
        m.enabled = !m.enabled;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Localization.Translate("TITLE"));
    }

    public static string CleanJSON(string json)
    {
        return json.Replace("\"class\"", "\"_class\"");
    }

    

    public void CreatePhysicalLayers(TileMap map, bool inEditorMode)
    {
        //CreatePhysicalLayer(map, GROUND_LAYER, _subSceneController.GroundParent, true);
        CreatePhysicLayer(map, GROUND_LAYER, _subSceneController.GroundParent, _groundTileConfigDic, true, _groundConfig.Zposition, inEditorMode);
        CreatePhysicLayer(map, GROUND_FRONT, _subSceneController.GroundParent, _groundTileConfigDic, true, _groundConfig.Zposition, inEditorMode);
        CreatePhysicLayer(map, LOGIC_LAYER, _subSceneController.LogicParent, _groundTileConfigDic, true, _groundConfig.Zposition, inEditorMode);
        //CreatePhysicalLayer(map, LOGIC_LAYER, _subSceneController.LogicParent, false);
        for (int i = 0; i < _graphicLayerConfig.graphicLayers.Length; ++i)
        {
            CreateGraphicLayer(map, _graphicLayerConfig.graphicLayers[i].name, _subSceneController.GraphicParent, _graphicTileConfigDic, _groundTileConfigDic, _graphicLayerConfig.graphicLayers[i].position, _graphicLayerConfig.graphicLayers[i].addToLogic, _graphicLayerConfig.graphicLayers[i].layerDepth, inEditorMode);
        }

        ConfigureGraphicMobilePlatforms();
        ConfigureAttachableGraphics();
    }

    private void ConfigureAttachableGraphics()
    {
        ReSkinable[] reSkin = FindObjectsByType<ReSkinable>(FindObjectsSortMode.None);
        if(reSkin != null && reSkin.Length > 0)
        {
            AttachableToPhysic[] atachableGraphics = FindObjectsByType<AttachableToPhysic>(FindObjectsSortMode.None);

            List<Pair<GameObject, GameObject>> graphicPlatform = Utils.FindGraphicInPosition(reSkin, new Vector3(_tileSize.x, _tileSize.y, _tileSize.x), atachableGraphics);
            for (int i = 0; i < graphicPlatform.Count; ++i)
            {
                Transform t = graphicPlatform[i].First.transform;
                graphicPlatform[i].Second.transform.parent = t;
                graphicPlatform[i].Second.GetComponent<AttachableToPhysic>().IsAssigned = true;
                graphicPlatform[i].First.GetComponent<ReSkinable>().skinToChange.enabled = false;
            }

            for (int i = 0; i < atachableGraphics.Length; ++i)
            {
                if(atachableGraphics[i].IsAssigned)
                    DestroyImmediate(atachableGraphics[i]);
            }
        }
    }

    private void ConfigureGraphicMobilePlatforms()
    {
        MobilePlatformTile[] platforms = FindObjectsByType<MobilePlatformTile>(FindObjectsSortMode.None);
        AttachableToPhysic[] atachableGraphics = FindObjectsByType<AttachableToPhysic>(FindObjectsSortMode.None);

        List<Pair<GameObject, GameObject>> graphicPlatform = Utils.FindGraphicInPosition(platforms, new Vector3(_tileSize.x, _tileSize.y, _tileSize.x), atachableGraphics);
        for (int i = 0; i < graphicPlatform.Count; ++i)
        {
            Transform t = graphicPlatform[i].First.transform;
            graphicPlatform[i].Second.transform.parent = t;
            graphicPlatform[i].Second.GetComponent<AttachableToPhysic>().IsAssigned = true;
            graphicPlatform[i].First.GetComponent<MeshRenderer>().enabled = false;
        }

        for (int i = 0; i < atachableGraphics.Length; ++i)
        {
            if(atachableGraphics[i].IsAssigned)
                DestroyImmediate(atachableGraphics[i]);
        }
    }

    private void SetCorrectObjectIdIntoDic(int firstGid, string tilesetName)
    {
        if (_objectTileDataDic == null)
            _objectTileDataDic = new Dictionary<int, ObjectData>();
        for (int i = 0; i < _objectTileData.Count; ++i)
        {
            if (_objectTileData[i].teleset == tilesetName)
            {
                _objectTileData[i].idTile = _objectTileData[i].idTile + firstGid;
                _objectTileDataDic.Add(_objectTileData[i].idTile, _objectTileData[i]);
            }
        }

    }

    

    public void CreatingSubScene()
    {
        _subSceneController.gameObject.name = _prefabSubsceneName;
        SceneMgr sceneMgr = GameMgr.Instance.GetServer<SceneMgr>();

        //subSceneMgr._initialSubScene = _subSceneController.gameObject;
        sceneMgr.AddSubScene(_subSceneController.gameObject);
    }


    public string GetHash()
    {
        return _textMapHash;
    }

    public bool IsTiledChanged()
    {
        string hashStored = GetHash();
        string newHash = CalculateHash();
        return hashStored != newHash;
    }

    public string CalculateHash()
    {
        Hash128 hash = Hash128.Compute(_textMap[0].text);
        return hash.ToString();
    }

    public string SaveHash()
    {
        if(_textMap != null && _textMap.Length > 0)
        {
#if UNITY_EDITOR
            Debug.Log("Hash inicial " + GetHash());
            _textMapHash = CalculateHash();
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            //En caso de que el usuario haga cambios, se marca la escena como "Dirty"
#endif
        }
        return _textMapHash;
    }

    public void ResetScene()
    {
        GameMgr.Instance.Destroy();
#if UNITY_EDITOR
        SaveHash();
#endif
        ExportVersion version = _subSceneController.GetComponent<ExportVersion>();
        if (version != null)
        {
            version.version++;
        }
            
        _subSceneController.gameObject.SetActive(true);
    }
    public bool GenerateScene(bool silentMode = false)
    {

#if UNITY_EDITOR

        GameMgr.Instance.Destroy();
        //EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        string oldName = _subSceneController.name;
        _subSceneController.name = _prefabSubsceneName;
        Directory.CreateDirectory(_scenePath);
        if (EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), _scenePath + "/" + _sceneName + ".unity"))
            Debug.Log("(EXPORT) (SAVE) Escena " + _sceneName + " Guardada correctamente");
        else
            Debug.Log("(EXPORT) (ERROR) La escena " + _sceneName + " no ha podido ser guardada correctamente");

        //PrefabUtility.RevertPrefabInstance(_subSceneController.gameObject, InteractionMode.AutomatedAction);
        _subSceneController.name = oldName;

        
        Scene s = EditorSceneManager.OpenScene(_scenePath + "/" + _sceneName + ".unity");
        if (!_includeBaseLogic)
        {
            GameObject go = GameObject.FindGameObjectWithTag(Globals.BasePlayerLogic);
            DestroyImmediate(go);
        }

        TileMapLoaderV2 ss = GameObject.FindAnyObjectByType<TileMapLoaderV2>();
        DestroyImmediate(ss.gameObject);
        
        //EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

        if (!EditorSceneManager.SaveScene(s))
            Debug.Log("(Error) (SAVE) Escena " + s.name + " no ha podido ser guardada correctamente");

        EditorSceneManager.CloseScene(s, true);

        
        if (!silentMode)
            EditorUtility.DisplayDialog("Export info", "Exportación del nivel " + _prefabSubsceneName + " completada con éxito !", "OK");
#endif
        return true;
    }

    private void CreateGraphicLayer(TileMap map, string layerID, Transform parent, Dictionary<int, GraphicTile> tileConfigure, Dictionary<int, PhysicTile> PhysicTileConfigure, float z, bool addToLogic, int layerDepth, bool editorMode)
    {
        if (map.Exist(layerID))
        {
            GameObject layer = null;
            if (parent != null)
            {
                layer = new GameObject(layerID);
                layer.transform.parent = parent;
            }

            int[] layerData = map[layerID].Data;
            for (int i = 0; i < layerData.Length; ++i)
            {
                int tileID = layerData[i];
                if (tileConfigure.ContainsKey(tileID) || PhysicTileConfigure.ContainsKey(tileID))
                {
                    GraphicTile graphicTile = null;
                    if (!tileConfigure.TryGetValue(tileID, out graphicTile))
                    {
                        PhysicTile ft = PhysicTileConfigure[tileID];
                        graphicTile = (GraphicTile)ft;
                    }
                    GameObject[] prefabs = graphicTile.Prefab;
                    Debug.Assert(prefabs != null && prefabs.Length > 0, "No se ha encontrado el prefab del tile " + tileID + " ID en tiled " + graphicTile.id + " en la layer " + layerID);
                    if (prefabs != null && prefabs.Length > 0)
                    {
                        GameObject tile = null;
                        int depth = layerDepth / 3;
                        if (depth == 1) // desplaza la capa en las capas que están escaladas para que se coloque correctamente.
                            depth = 0;

                        if (prefabs.Length > 1)
                        {

                            int rang = UnityEngine.Random.Range(0, prefabs.Length);
                            tile = ClonePrefab(editorMode,false,prefabs[rang], CalcPosition(map, i, layerID, z + depth), Quaternion.Euler(graphicTile.rotation));
                            //tile = Instantiate(prefabs[rang], CalcPosition(map, i, layerID, z + depth), Quaternion.Euler(graphicTile.rotation));
                        }
                        else if (prefabs[0] != null)
                        {
                            try
                            {
                                tile = ClonePrefab(editorMode,false, prefabs[0], CalcPosition(map, i, layerID, z + depth), Quaternion.Euler(graphicTile.rotation));
                                //tile = Instantiate(prefabs[0], CalcPosition(map, i, layerID, z + depth), Quaternion.Euler(graphicTile.rotation));
                            }
                            catch (Exception e)
                            {
                                Debug.LogError(e.Message);
                                Debug.LogError(" Tile id " + tileID + " graphicTile " + graphicTile.Name + " prefab " + prefabs[0] + " graphicTile " + graphicTile.Prefab);
                            }
                        }
                        else
                        {
                            Debug.LogWarning(" Tile id " + tileID + " graphicTile " + graphicTile.Name + " Se ha ignorado al no tener prefab ");
                        }

                        if (tile != null)
                        {
                            tile.transform.localScale = new Vector3(tile.transform.localScale.x, tile.transform.localScale.y, (layerDepth / 3));
                            if (layer != null)
                                tile.transform.parent = layer.transform;
                            else
                                tile.transform.parent = _sceneRoot;

                            if (tile.name.ToLower().EndsWith("(clone)"))
                            {
                                tile.name = tile.name.Substring(0, tile.name.Length - 7);
                            }

                            if (tile.name.ToLower().EndsWith("(clone)"))
                            {
                                tile.name = tile.name.Substring(0, tile.name.Length - 7);
                            }

                            MeshRenderer mesh = tile.GetComponent<MeshRenderer>();
                            if (mesh != null)
                                mesh.enabled = graphicTile.visible;
                            else
                            {
                                SpriteRenderer sprite = tile.GetComponent<SpriteRenderer>();
                                if (sprite != null)
                                    sprite.enabled = graphicTile.visible;
                            }

                            /*if (addToLogic)
                                GameMgr.Instance.GetLogicMap().Add(layerID, tile.transform, graphicTile.transparent);*/
                        }
                    }
                }
                else if (tileID != TILE_EMPTY)
                {
                    Debug.LogWarning("tile " + tileID + " no reconocido en layer " + layerID);
                    //GameObject tileError = Instantiate(_tileErrorDebug, CalcPosition(map, i, layerID, z), Quaternion.identity);
                    GameObject tileError = ClonePrefab(editorMode,false, _tileErrorDebug, CalcPosition(map, i, layerID, z), Quaternion.identity);
                    TextMesh tm = tileError.GetComponentInChildren<TextMesh>();
                    if (tm != null)
                    {
                        Vector3 position = CalcTiledPosition(map, i, layerID, z);
                        tm.text = "(" + position.x + "," + position.y + ")" + tileID + "layer:"+ layerID;
                        tileError.name = tm.text;
                    }

                }
            }
        }
    }

    

    private GameObject ClonePrefab(bool editorMode, bool spawner, GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        GameObject gameObjectedReturned = null;
#if UNITY_EDITOR
        if ((!editorMode && !spawner) || (editorMode && !spawner && !PrefabUtility.IsPartOfAnyPrefab(prefab)))
        {
            gameObjectedReturned = Instantiate(prefab, position, rotation);
        }
        else if (editorMode && !spawner)
        {

            gameObjectedReturned = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            gameObjectedReturned.transform.position = position;
            gameObjectedReturned.transform.rotation = rotation;

        }
        else if (!editorMode && spawner || (editorMode && spawner && !PrefabUtility.IsPartOfAnyPrefab(prefab)))
        {
            gameObjectedReturned = GameMgr.Instance.GetSpawnerMgr().CreateNewGameObject(prefab,position,rotation,parent);
            return gameObjectedReturned;
        }
        else if (editorMode && spawner)
        {
            gameObjectedReturned = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            gameObjectedReturned.transform.position = position;
            gameObjectedReturned.transform.rotation = rotation;
            GameMgr.Instance.GetSpawnerMgr().SetAsSpawneableGO(gameObjectedReturned, prefab);

        }

        if (parent != null)
            gameObjectedReturned.transform.parent = parent;

#endif
        return gameObjectedReturned;
    }

    private void CreatePhysicLayer(TileMap map, string layerID, Transform parent, Dictionary<int, PhysicTile> tileConfigure, bool optimize, float z, bool editorMode)
    {
        if (!map.Exist(layerID))
            return;
        List<Vector3> defferredTileList = new List<Vector3>();




        int[] layerData = map[layerID].Data;
        for (int i = 0; i < layerData.Length; ++i)
        {
            int tileID = layerData[i];

            if (tileConfigure.ContainsKey(tileID))
            {
                PhysicTile logicTile = tileConfigure[tileID];
                GameObject[] prefabs = logicTile.Prefab;
                Debug.Assert(prefabs != null && prefabs.Length > 0, "No se ha encontrado el prefab del tile " + tileID + " en la layer " + layerID);
                GameObject tile = null;
                if (prefabs.Length > 1)
                {
                    int rang = UnityEngine.Random.Range(0, prefabs.Length);
                    tile = ClonePrefab(editorMode, false, prefabs[rang], CalcPosition(map, i, layerID, z), Quaternion.Euler(logicTile.rotation));
                    //tile = Instantiate(prefabs[rang], CalcPosition(map, i, layerID, z), Quaternion.Euler(logicTile.rotation));
                }
                else
                    tile = ClonePrefab(editorMode, false, prefabs[0], CalcPosition(map, i, layerID, z), Quaternion.Euler(logicTile.rotation));
                    //tile = Instantiate(prefabs[0], CalcPosition(map, i, layerID, z), Quaternion.Euler(logicTile.rotation));


                if (tile != null)
                {
                    UnpackedPrefab unpacked = tile.GetComponent<UnpackedPrefab>();
                    if(unpacked != null)
                    {
                        Utils.UnpackPrefabIfIsPossible(tile);
                    }

                    if (parent != null)
                        tile.transform.parent = parent;
                    else
                        tile.transform.parent = _sceneRoot;


                    if (tile.name.ToLower().EndsWith("(clone)"))
                    {
                        tile.name = tile.name.Substring(0, tile.name.Length - 7);
                    }

                    //optimización de escenas, si la fisica no se va a ver, le quitamos el apartado gráfico
                    if(!logicTile.visible && !_debugLogic)
                    {
                        if(tile.GetComponent<RectangleArea>() == null)
                        {
                            MeshRenderer mesh = tile.GetComponent<MeshRenderer>();
                            if (mesh != null)
                                DestroyImmediate(mesh);
                            MeshFilter meshfilter = tile.GetComponent<MeshFilter>();
                            if (meshfilter != null)
                                DestroyImmediate(meshfilter);
                            SpriteRenderer spriteRender = tile.GetComponent<SpriteRenderer>();
                            if (spriteRender != null)
                                DestroyImmediate(spriteRender);
                        }
  
                    }
                    else
                    {
                        MeshRenderer mesh = tile.GetComponent<MeshRenderer>();
                        if (mesh != null)
                            mesh.enabled = _debugLogic ? true : logicTile.visible;
                        else
                        {
                            SpriteRenderer sprite = tile.GetComponent<SpriteRenderer>();
                            if (sprite != null)
                                sprite.enabled = _debugLogic ? true : logicTile.visible;
                        }
                    }
                }
            }
            else if (layerData[i] != TILE_EMPTY)
            {
                Debug.LogWarning("tile " + tileID + " no reconocido en layer " + layerID);
                GameObject tileError = ClonePrefab(editorMode,false, _tileErrorDebug, CalcPosition(map, i, layerID, z), Quaternion.identity);
                //GameObject tileError = Instantiate(_tileErrorDebug, CalcPosition(map, i, layerID, z), Quaternion.identity);
                TextMesh tm = tileError.GetComponentInChildren<TextMesh>();
                if (tm != null)
                {
                    Vector3 position = CalcTiledPosition(map, i, layerID, z);
                    tm.text = "(" + position.x + "," + position.y + ")" + tileID + "layer:" + layerID;
                    tileError.name = tm.text;
                }
                Debug.LogWarning("tile " + tileID + " no reconocido en layer " + layerID);
            }
        }
    }

    private void CreatingManualGraphics(bool editorMode)
    {
        for (int i = 0; i < _sceneGraphicsPrefabs.Length; ++i)
        {
            SceneManualGrapicPrefabs sceneGraphic = _sceneGraphicsPrefabs[i];
            GameObject go = ClonePrefab(editorMode, false, sceneGraphic.Prefab, sceneGraphic.Position, Quaternion.Euler(sceneGraphic.Rotation));
            //GameObject go = Instantiate(sceneGraphic.Prefab, sceneGraphic.Position, Quaternion.Euler(sceneGraphic.Rotation));
            go.transform.parent = _subSceneController.GraphicParent;
        }
    }


    private void CreatingTilemappings()
    {
        _objectTileData = CreateObjectsData(_objectConfig);
        _objectTileConfigDic = CreateObjectTileConfigDict(_objectConfig);
        _groundTileConfigDic = CreateLogicTileMap(this.offsetLogic, _groundConfig);
        _graphicTileConfigDic = CreateGraphicTileMap(_biomeConfig); 
        _forbiddenPositions = new List<Vector3>();
        _LogicWalls = new List<Vector3>();
    }



    private Dictionary<int, PhysicTile> CreateLogicTileMap(int offset, PhysicTileConfig logicConfig)
    {
        Dictionary<int, PhysicTile> logicTileDic = new Dictionary<int, PhysicTile>();
        for (int i = 0; i < logicConfig.physicTiles.Length; ++i)
        {
            PhysicTile phTile = logicConfig.physicTiles[i];
            logicTileDic.Add(offset + phTile.id, phTile);
        }
        return logicTileDic;
    }

    private Dictionary<int, GraphicTile> CreateGraphicTileMap( Biome biome)
    {
        Dictionary<int, GraphicTile> graphicTileDic = new Dictionary<int, GraphicTile>();

        for (int k = 0; k < biome.patters.Length; k++)
        {
            for (int i = 0; i < biome.patters[k].graphicTiles.Length; ++i)
            {
                GraphicTile grTile = biome.patters[k].graphicTiles[i];
                if (graphicTileDic.ContainsKey(biome.patters[k].offset + grTile.id))
                {
                    GraphicTile g = graphicTileDic[biome.patters[k].offset + grTile.id];
                    Debug.LogError("Duplicada la key " + (biome.patters[k].offset + grTile.id) + " de nombre " + grTile.Name + " Ya está asignada a " + g.Name + " en el patrón "+ biome.patters[k].name);
                }
                else
                    graphicTileDic.Add(biome.patters[k].offset + grTile.id, grTile);
            }
        }
        
        for (int k = 0; k < biome.patternProps.Length; k++)
        {
            for (int i = 0; i < biome.patternProps[k].graphicTiles.Length; ++i)
            {
                GraphicTile grTile = biome.patternProps[k].graphicTiles[i];
                if (graphicTileDic.ContainsKey(biome.patternProps[k].offset + grTile.id))
                {
                    GraphicTile g = graphicTileDic[biome.patternProps[k].offset + grTile.id];
                    Debug.LogError("Duplicada la key " + (biome.patternProps[k].offset + grTile.id) + " Ya está asignada a " + g.Name);
                }
                else
                    graphicTileDic.Add(biome.patternProps[k].offset + grTile.id, grTile);
            }
        }

        return graphicTileDic;
    }

    private List<ObjectData> CreateObjectsData(ObjectTileConfig objectTileConfig)
    {
        List<ObjectData> objectData = new List<ObjectData>();
        for (int i = 0; i < objectTileConfig.objectConfig.Length; ++i)
        {
            TextAsset text = objectTileConfig.objectConfig[i];
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(text.text);
            XmlNode telesetNode = xmlDoc.SelectSingleNode("tileset");
            foreach (XmlNode childrenNode in telesetNode)
            {
                if (childrenNode.Name == "tile")
                {
                    ObjectData data = new ObjectData();
                    data.teleset = text.name;
                    XmlAttribute id = childrenNode.Attributes["id"];
                    data.idTile = int.Parse(id.Value);
                    XmlAttribute type = childrenNode.Attributes["type"];
                    if (type != null)
                        data.type = type.Value;
                    XmlAttribute clase = childrenNode.Attributes["class"];
                    if (clase != null)
                        data.type = clase.Value;
                    if (childrenNode.ChildNodes.Count > 0)
                    {
                        XmlNode properties = childrenNode.FirstChild;
                        data.objectProperties = new ObjectProperty[properties.ChildNodes.Count];
                        int iProperty = 0;
                        foreach (XmlNode propery in properties)
                        {
                            XmlAttribute sName = propery.Attributes["name"];
                            XmlAttribute sType = propery.Attributes["type"];
                            XmlAttribute sValue = propery.Attributes["value"];
                            ObjectProperty op = new ObjectProperty();
                            op.name = sName.Value;
                            if (sType != null)
                                op.type = sType.Value;
                            else
                                op.type = "string";
                            op.value = sValue.Value;
                            data.objectProperties[iProperty] = op;
                            iProperty++;
                        }
                    }
                    objectData.Add(data);
                }
            }


        }
        return objectData;
    }

    private Dictionary<string, ObjectTile> CreateObjectTileConfigDict(ObjectTileConfig objectTileConfig)
    {
        Dictionary<string, ObjectTile> tileConfigDic = new Dictionary<string, ObjectTile>();
        try
        {
            
            for (int i = 0; i < objectTileConfig.objectTiles.Length; ++i)
            {
                ObjectTile objTile = objectTileConfig.objectTiles[i];

                if (objTile.type == null || objTile.type == "")
                    Debug.LogError("Los objetos lógicos se identifica por su tipo. El objeto no tiene tipo " + i);
                tileConfigDic.Add(objTile.type, objTile);
            }

            return tileConfigDic;
        }
        catch(Exception e)
        {
            Debug.LogException(e);
            return null;
        }

    }

    private void CreateObjectGroup(ObjectGroup objectGroup, int newLayer)
    {

        for (int i = 0; i < objectGroup.Objects.Length; ++i)
        {
            ObjectMap objMap = objectGroup.Objects[i];
            objMap.Layer = newLayer;
            // Si no está en los datos del mapa, estará en los datos por defecto
            if ((objMap.Type == null || objMap.Type == "") && (objMap.Class == null || objMap.Class == ""))
            {
                if (_objectTileDataDic.ContainsKey(objMap.Gid))
                    objMap.Type = _objectTileDataDic[objMap.Gid].type;
                else
                    Debug.LogError("El objMap.Gid " + objMap.Gid + " No está en el diccionario. ID del objeto " + objMap.Id + " Tile (" + objMap.TileX + "," + objMap.TileY + ")");
            }
            if (objMap.Type == ObjectTiledNames.Point.ToString() || objMap.Class == ObjectTiledNames.Point.ToString())
            {
                if (_pointsMap.ContainsKey(objMap.Name))
                {
                    List<ObjectMap> list = _pointsMap[objMap.Name];
                    list.Add(objMap);
                }
                else
                {
                    List<ObjectMap> list = new List<ObjectMap>();
                    list.Add(objMap);
                    _pointsMap.Add(objMap.Name, list);
                }
            }
            else if (objMap.Type == OtherObjecTiledNames.CameraZoom.ToString() || objMap.Class == OtherObjecTiledNames.CameraZoom.ToString())
            {
                if (!_objetcMaps.ContainsKey(OtherObjecTiledNames.CameraZoom.ToString()))
                    _objetcMaps.Add(OtherObjecTiledNames.CameraZoom.ToString(), new List<ObjectMap>());
                _objetcMaps[OtherObjecTiledNames.CameraZoom.ToString()].Add(objMap);
            }
            else 
            {
                if (objMap.Type == "diseño" || objMap.Class == "diseño")
                {
                    Debug.Log("Objeto ignorado por tratarse de un objeto de diseño ID " + objMap.Id + " Tile X " + objMap.TileX + " tile Y " + objMap.TileY);
                }
                else
                {
                    if (objMap.Class != null && objMap.Class != "" && (objMap.Type == null || objMap.Type == ""))
                    {
                        objMap.Type = objMap.Class;
                    }

                    if (objMap.Type == null || objMap.Type == "")
                    {
                        if (_objectTileDataDic.ContainsKey(objMap.Gid))
                            objMap.Type = _objectTileDataDic[objMap.Gid].type;
                        else
                            Debug.LogError("Objeto no reconocido (ni tiene tipo ni está en el diccionario de tipos) será ignorado. Comprueba por si es un error en el Tiled. ID " + objMap.Id + " Tile X " + objMap.TileX + " tile Y " + objMap.TileY);
                    }
                    if (objMap.Type == null)
                        Debug.LogError("El campo type no peude ser null");
                    if (!_objetcMaps.ContainsKey(objMap.Type))
                        _objetcMaps.Add(objMap.Type, new List<ObjectMap>());
                    _objetcMaps[objMap.Type].Add(objMap);
                }
            }
        }
    }

    public void CreateLayers(TileMap map)
    {
        GameMgr.Instance.GetLogicMap().CreateMap(_tileSize.x);
        GameMgr.Instance.GetLogicMap().CreateLayer(GROUND_LAYER, (int)map.Width, (int)map.Height, 0);
        GameMgr.Instance.GetLogicMap().CreateLayer(LOGIC_LAYER, (int)map.Width, (int)map.Height, 0);
        for (int i = 0; i < _graphicLayerConfig.graphicLayers.Length; ++i)
        {
            if (_graphicLayerConfig.graphicLayers[i].addToLogic)
            {
                GameMgr.Instance.GetLogicMap().CreateLayer(_graphicLayerConfig.graphicLayers[i].name, (int)map.Width, (int)map.Height, 0);
            }
        }
        //GameMgr.Instance.GetLogicMap().CreateLayer(GRAPHIC_LAYER, (int)map.Width, (int)map.Height, 0);
        //GameMgr.Instance.GetLogicMap().CreateLayer(GRAPHIC_BACK_LAYER, (int)map.Width, (int)map.Height, 0);
        GameMgr.Instance.GetLogicMap().CreateLayer(GROUND_FRONT, (int)map.Width, (int)map.Height, 0);
    }

    private void InitCache()
    {
        _prefabCache = new Dictionary<string, LayerInfoV2>();
        LayerInfoV2 groundLayerInfo = new LayerInfoV2(new Dictionary<int, LayerInfoData>(), 0, _subSceneController.GroundParent);
        LayerInfoV2 logicLayerInfo = new LayerInfoV2(new Dictionary<int, LayerInfoData>(), 0, _subSceneController.LogicParent);
        _prefabCache.Add(GROUND_LAYER, groundLayerInfo);
        _prefabCache.Add(LOGIC_LAYER, logicLayerInfo);
        for (int i = 0; i < _graphicLayerConfig.graphicLayers.Length; ++i)
        {
            if (_graphicLayerConfig.graphicLayers[i].addToLogic)
            {
                LayerInfoV2 graphicLayerInfo = new LayerInfoV2(new Dictionary<int, LayerInfoData>(), _graphicLayerConfig.graphicLayers[i].position, _subSceneController.GraphicParent);
                _prefabCache.Add(_graphicLayerConfig.graphicLayers[i].name, graphicLayerInfo);
            }
        }
        //LayerInfoV2 graphicLayerInfo = new LayerInfoV2(new Dictionary<int, LayerInfoData>(), -1, _subSceneController.GraphicParent);
        //LayerInfoV2 graphicBackLayerInfo = new LayerInfoV2(new Dictionary<int, LayerInfoData>(), 2, _subSceneController.GraphicBackParent);
        //_prefabCache.Add(GRAPHIC_LAYER, graphicLayerInfo);
        //_prefabCache.Add(GRAPHIC_BACK_LAYER, graphicBackLayerInfo);
    }

    

   

    public object GetPropertyDefaultValue(int id, System.Type t, string name)
    {
        if (!_objectTileDataDic.ContainsKey(id))
        {
            Debug.LogError("El id " + id + " no es un id de atributo conocido");
            return null;
        }

        ObjectData obData = _objectTileDataDic[id];
        string value = "";
        for (int i = 0; i < obData.objectProperties.Length; ++i)
        {
            if (obData.objectProperties[i].name == name)
            {
                //Comprobamos si el tipo es correcto por detección de errores
                string sType = t.ToString();
                if (!sType.ToLower().Contains(obData.objectProperties[i].type.ToLower()))
                {
                    Debug.LogError("El tipo solicitado no es el establecido por defecto para " + name);
                    return null;
                }
                value = obData.objectProperties[i].value;
            }
        }

        if (t.Equals(typeof(int)))
        {
            return int.Parse(value);
        }
        else if (t.Equals(typeof(float)))
        {
            return float.Parse(value);
        }
        else if (t.Equals(typeof(string)))
        {
            return value;
        }
        else
        {
            Debug.LogError("The type " + t + " is not allowed ");
            return null;
        }

    }

    private void CreateObjectWithoutConfiguration(TileMap tileMap,bool editorMode)
    {
        GameObject spawnerPointsParent = new GameObject("SpawnerPoints_PlayerCouples");
        //Se desactiva para que no se realice la llamada del Awake de sus componentes
        spawnerPointsParent.SetActive(false);
        //Se asigna el GO del nivel como el padre del spawner.
        spawnerPointsParent.transform.parent = _subSceneController.transform;


        //Una vez inicializados los parametros, se activa el GO
        spawnerPointsParent.SetActive(true);

        foreach (string type in _objetcMaps.Keys)
        {

            if (type == OtherObjecTiledNames.Player.ToString())
            {
                List<ObjectMap> objMapList = _objetcMaps[type];
                for (int i = 0; i < objMapList.Count; ++i)
                {
                    ObjectMap objMap = objMapList[i];
                    int playerID = -1;
                    if (objMap.PropertyExist("playerid"))
                        playerID = (int)objMap.getProperty(typeof(int), "playerid");
                    else
                        playerID = (int)GetPropertyDefaultValue(objMap.Gid, typeof(int), "playerid");
                    Vector3 position = CalcObjectPosition(tileMap, objMap.TileX, objMap.TileY, LOGIC_LAYER);

                    //Obtenemos el parametro direction (right: 1, left: -1)
                    int direction = 1;
                    if (objMap.PropertyExist("dir"))
                    {
                        string directionLiteral = (string)objMap.getProperty(typeof(string), "dir");
                        direction = directionLiteral == "right" ? 1 : -1;
                    }

                    //PlayerSpawner genera la logica en caso de que no exista
                    //Creamos los dos puntos de spawn y se asignan al componente PlayerSpawner para ser referenciados.
                    //Asignamos la direccion a la que miran los jugadores
                    GameObject spawnPoint = new GameObject("Player_" + playerID + "_SpawnPoint");
                    spawnPoint.transform.parent = spawnerPointsParent.transform;
                    spawnPoint.transform.position = position;
                }
            }
            else if (_objectTileConfigDic.ContainsKey(type) && !_objectTileConfigDic[type].needPostprocessing)
            {
                List<ObjectMap> objMapList = _objetcMaps[type];
                for (int i = 0; i < objMapList.Count; ++i)
                {
                    ObjectMap objMap = objMapList[i];

                    Vector3 position = CalcObjectPosition(tileMap, objMap.TileX, objMap.TileY, LOGIC_LAYER);
                    GameObject[] prefabs = _objectTileConfigDic[type].prefab;
                    if (prefabs.Length > 0)
                    {
                        GameObject prefab = null;
                        GameObject objGo = null;

                        if (type != ObjectTiledNames.Device.ToString()) 
                        {
                            prefab = SelectRamdomPrefab(prefabs, type);
                            objGo = ClonePrefab(editorMode,true, prefab, position, Quaternion.identity, null);
                            //objGo = GameMgr.Instance.GetSpawnerMgr().CreateNewGameObject(prefab, position, Quaternion.identity, null);
                        }


                        objGo.transform.parent = _subSceneController.LogicParent;
                        if (objMap.NumProperties > 0)
                            MagicConfigureParameters(objGo, objMap);
                    }
                    else
                    {
                        Debug.LogWarning("SE ha obviado la creación del objeto "+type+" porque no tiene prefab asociado");
                    }
                }
            }
            else if (!_objectTileConfigDic.ContainsKey(type))
            {
                List<ObjectMap> objects = _objetcMaps[type];
                Debug.LogError("No se ha encontrado el tipo " + type + " objects X " + objects[0].TileX + " Y " + objects[0].TileY + " id " + objects[0].Id);
            }
        }
    }

  
    private void MagicConfigureParameters(GameObject go, ObjectMap objectMap)
    {
        Component[] components = go.GetComponents<MonoBehaviour>();
        if (go.name.ToLower().StartsWith("frokkir"))
            Debug.Log("Paramos");
        for (int i = 0; i < components.Length; ++i)
        {
            Component c = components[i];
            if(c.name.ToLower().StartsWith("frokkirtiledparameters"))
            {
                Debug.Log("Paramos2");
            }
            if (c == null)
                Debug.LogError("El componente " + i + " del objeto " + go.name + " es nulo");
            else
            {
                object[] attributtes = c.GetType().GetCustomAttributes(true);
                ClassTileMapAsignableAttributeAttribute classTileMapAsign = Utils.GetAttribute<ClassTileMapAsignableAttributeAttribute>(attributtes);
                if (classTileMapAsign != null)
                {
                    CopyAttributes(go.name, c, objectMap);
                }
            }
        }

        for (int i = 0; i < go.transform.childCount; ++i)
        {
            MagicConfigureParameters(go.transform.GetChild(i).gameObject, objectMap);
        }
    }

    private void CopyAttributes(string goName, Component c, ObjectMap objectMap)
    {
        FieldInfo[] fields = c.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        for (int i = 0; i < fields.Length; ++i)
        {
            FieldInfo fieldInfo = fields[i];
            object[] attributtes = fieldInfo.GetCustomAttributes(true);
            FieldTileMapAsignableAttributeAttribute fieldTileMapAsign = Utils.GetAttribute<FieldTileMapAsignableAttributeAttribute>(attributtes);
            if (fieldTileMapAsign != null)
            {
                if (!objectMap.PropertyExist(fieldTileMapAsign.Name) && fieldTileMapAsign.Required)
                    Debug.LogError(" La propiedad requeridad " + fieldTileMapAsign.Name + " no se ha definido en el mapa para el objeto " + objectMap.Gid + " de nombre " + objectMap.Name + " y de tipo " + objectMap.Class + " en la posición (" + objectMap.TileX + "," + objectMap.TileY + ") tipo " + objectMap.Type + " field info " + fieldInfo.Name);


                if (fieldInfo.FieldType.IsEnum)
                {
                    //si no se queda con el valor por defecto que tenga
                    if (objectMap.PropertyExist(fieldTileMapAsign.Name))
                    {
                        string val = (string)objectMap.getProperty(typeof(string), fieldTileMapAsign.Name);
                        object objVal = Enum.Parse(fieldInfo.FieldType, val, true);
                        fieldInfo.SetValue(c, objVal);
                    }
                }
                else
                {
                    //si no se queda con el valor por defecto que tenga
                    if (objectMap.PropertyExist(fieldTileMapAsign.Name))
                    {
                        object v = objectMap.getProperty(fieldInfo.FieldType, fieldTileMapAsign.Name);

                        Debug.Log("Plataforma " + goName + " " + fieldTileMapAsign.Name + ":" + v);

                        fieldInfo.SetValue(c, v);
                    }
                }
            }
            FieldTileMapAsignableIDAttribute fieldTileID= Utils.GetAttribute<FieldTileMapAsignableIDAttribute>(attributtes);
            if (fieldTileID != null)
            {
                fieldInfo.SetValue(c, objectMap.Id);
            }
        }
    }
    private GameObject SelectRamdomPrefab(GameObject[] prefabs, string type)
    {
        if (prefabs.Length == 1)
            return prefabs[0];
        else
        {
            int rang = UnityEngine.Random.Range(0, prefabs.Length);
            if (rang >= prefabs.Length)
                Debug.LogError(" Se ha generado un rango "+ rang+" fuera de rango "+ prefabs.Length + " para el prefab del tile "+ type);
            return prefabs[rang];
        }
    }

    private Vector3 CalcObjectPosition(TileMap map, float x, float y, string layerID)
    {
        float xPosition = map[layerID].X + x * _tileSize.x;
        float yPosition = map[layerID].Y + (map.Height - ((int)y * _tileSize.y));
        return new Vector3(xPosition, yPosition, _prefabCache[layerID].ZPosition);
    }

    private void GetPoints(ObjectMap obj, out Vector3[] points, out float[] offsets, float height, float z)
    {
        if(!_pointsMap.ContainsKey(obj.Name))
        {
            Debug.LogError("obj.Name " + obj.Name + " GID " + obj.Gid + " ID " + obj.Id + " X " + obj.X + " Y " + obj.Y + " Tile X " + obj.TileX + " TileY " + obj.TileY + " type " + obj.Type + " no se ha encontrado en el diccionario de puntos");
        }
        List<ObjectMap> objPoints = _pointsMap[obj.Name];
        points = new Vector3[objPoints.Count];
        offsets = new float[objPoints.Count];

        Tuple<Vector3, float>[] tupleArray = new Tuple<Vector3, float>[objPoints.Count];

        for (int i = 0; i < objPoints.Count; ++i)
        {
            Debug.Assert(objPoints[i].PropertyExist("offset"), "Error, no se ha definido la propiedad offset en el punto");
            Debug.Assert(objPoints[i].PropertyExist("order"), "Error, no se ha definido la propiedad order en el punto");
            float offs = objPoints[i].getPropertyFloat("offset");
            int order = objPoints[i].getPropertyInt("order");
            Vector3 p = new Vector3(objPoints[i].TileX, height - (int)objPoints[i].TileY, z);
            Debug.Assert(order >= 0 && order < tupleArray.Length, "El orden definido "+ order + " no es válido, debe estar entre 0 y "+ order);
            tupleArray[order] = new Tuple<Vector3, float>(p, offs);
        }

        for (int i = 0; i < tupleArray.Length; ++i)
        {
            if (tupleArray[i] != null)
            {
                points[i] = tupleArray[i].Item1;
                offsets[i] = tupleArray[i].Item2;
            }
            else
                Debug.LogError("El item "+i+" está vacio en "+ obj.Name);
        }

    }

    

    private ObjectMap[] CalcPoints(ObjectMap obj, int tilex, int tiley, string tipo, int max = -1)
    {
        ObjectMap[] objArray = null;
        if(!_pointsMap.ContainsKey(obj.Name))
        {
            Debug.LogError("El objeto con nombre " + obj.Name + " no tiene puntos asociados "+ _pointsMap.Count + " posicion "+ tilex + ","+ tiley + " tipo de objeto "+ tipo);
        }
        List<ObjectMap> points = _pointsMap[obj.Name];
        if(max > 0)
        {
            if (points.Count > 2)
                Debug.LogError("Error, there is more than two points linked to the platform " + obj.Name);
            else
            {
                ObjectMap p1 = points[0];
                ObjectMap p2 = points[1];
                ObjectMap pLeft = p1;
                ObjectMap pRight = p2;
                bool pAsigned = false;
                if (p1.PropertyExist("start"))
                {
                    if (p1.getPropertyBool("start"))
                    {
                        pLeft = p1;
                        pRight = p2;
                        pAsigned = true;
                    }
                }
                else if (p2.PropertyExist("start"))
                {
                    if (p2.getPropertyBool("start"))
                    {
                        pLeft = p2;
                        pRight = p1;
                        pAsigned = true;
                    }
                }
                if (!pAsigned)
                {
                    if (p1.X < p2.X)
                    {
                        pLeft = p1;
                        pRight = p2;
                    }
                    else
                    {
                        pLeft = p2;
                        pRight = p1;
                    }
                }
                objArray = new ObjectMap[2];
                objArray[LEFT] = pLeft;
                objArray[RIGHT] = pRight;
            }
        }
        else
        {
            //new array
            objArray = new ObjectMap[points.Count];

            foreach (ObjectMap point in points)
            {
                if(point.PropertyExist("turn"))
                {
                    int index = point.getPropertyInt("turn");
                    if (index >= 0 && index < objArray.Length)
                    {
                        if (objArray[index] != null)
                        {
                            Debug.LogError("The point with turn " + index + " is duplicated");
                            continue;
                        }

                        objArray[index] = point;
                    }
                    else
                    {
                        Debug.Log("The point " + point + " of type" + tipo + "has no valid number");
                    }
                }
                else
                {
                    Debug.LogError("No has especificado el atributo turn");
                }
            }
            //objArray = points.ToArray();
            //Array.Sort(objArray, (a, b) =>
            //{
            //    int compareX = a.X.CompareTo(b.X);
            //    return compareX == 0 ? a.Y.CompareTo(b.Y) : compareX;
            //});
        }

        return objArray;
    }

    private Vector3 CalcPosition(TileMap map, int i, string layerID, float z)
    {
        float xPosition = map[layerID].X + (i % map[layerID].Width);
        int col = ((int)(i)) / map[layerID].Width;

        float yPosition = map[layerID].Y + ((map.Height - col) - 1);

        return new Vector3(xPosition*_tileSize.x, yPosition*_tileSize.y, z) + _positionOffset;
    }

    private Vector3 CalcTiledPosition(TileMap map, int i, string layerID, float z)
    {
        float xPosition = map[layerID].X + ((i * _tileSize.x) % map[layerID].Width);
        int col = ((int)(i * _tileSize.y)) / map[layerID].Width;

        float yPosition = map[layerID].Y + col;

        return new Vector3(xPosition, yPosition, z) - _positionOffset;
    }
}
