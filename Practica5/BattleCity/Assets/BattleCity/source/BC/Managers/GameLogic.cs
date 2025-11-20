using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;

public class GameLogic : MonoBehaviour
{
    public enum GAME_MODE { NORMAL, RECORDED, IA_DRIVEN }
    public enum GAME_DIFICULT { EASY, NORMAL }

    public GAME_MODE gameMode = GAME_MODE.NORMAL;
    public GAME_DIFICULT gameDifilty = GAME_DIFICULT.EASY;
    public bool withExit;
    public TMP_Text  agentName;
    public GameObject agentDestroy;
    public GameObject gameOver;
    public GameObject waiting;
    public GameObject ready;
    public GameObject playerWin;
    public TMP_Text log;
    public GameObject logPannel;

    public UnityEvent onReady;
    public float initTime;
    public bool waitingForAgents;
    public BCServerCommands server;
    public int numAgentsSpected;
    public TextAsset _textMap;
    public bool showVisualMap;
    public VisualMap visualMap;
    public Vector2Int tileSize;
    public bool showHealth;
    public GameObject healthPrefab;
    public GameObject exitPrefab;
    public List<Color> tankAgentColors;

    public System.Action<bool> gameFinishCallback;

    private string _agentwinNameWin;
    private bool isReady;
    private bool isGameOver;
    private Dictionary<string, TankAgent> agentsLinked;


    private int _unlinkAgents = 0;
    private GameObject player;
    private List<GameObject> enemies;
    private List<GameObject> agents;
    private Dictionary<string, bool> _agentDestroy;
    private Dictionary<string, string> _agentNames;
    private Dictionary<string,System.Net.Sockets.TcpClient> tcpClient;
    private bool logActive = false;
    private Map _map;
    private GameObject life;
    private GameObject exit;
    private bool _isWin;



    static GameLogic instance;

    public int NumAgentSpected
    {
        get
        {
            return numAgentsSpected;
        }
    }

    public Color GetColor()
    {
        Color c = tankAgentColors[0];
        tankAgentColors.RemoveAt(0);
        return c;
    }

    public void SetGameOver()
    {
        GameOver(null);
    }

    public bool IsMultyAgent
    {
        get
        {
            return numAgentsSpected > 1;
        }
    }

    public static GameLogic Get()
    {
        if (instance == null)
            instance = Utils.CreateStaticGet<GameLogic>();
        return instance;
    }

    public GameObject Player
    {
        get
        {
            return player;
        }
    }

    public void SendPerception(string agentID,MLGym.Parameters parameters, bool isDestroyed)
    {
        bool isGameOver = IsGameOver;
        string rivalID = GetRival(agentID);
        if(rivalID != null)
        {
            if (_agentDestroy[rivalID])
            {
                isGameOver = true; // si mato al adversario es como si hubiera matada al player.
            }
        }

        server.SendPerception(tcpClient[agentID], parameters, isGameOver, isDestroyed);
    }



    public void SendPerceptionAndMap(string agentID, MLGym.Parameters parameters, bool isDestroyed)
    {
        bool isGameOver = IsGameOver;
        string rivalID = GetRival(agentID);
        if(rivalID != null)
        {
            if (_agentDestroy[rivalID])
            {
                isGameOver = true; // si mato al adversario es como si hubiera matada al player.
            }
        }


        server.SendPerceptionAndMap(tcpClient[agentID], parameters, this._map, isGameOver, isDestroyed);
    }

    private string GetRival(string id)
    {
        if(_agentDestroy.Count > 2)
        {
            Debug.LogError("No puedo saber cual es mi rival ya que hay mas agentes que los necesarios");
            return null;
        }    
        foreach ( string key in _agentDestroy.Keys)
        {
            if (key != id)
                return key;
        }
        return null;
    }

    public void AgentDestroy(GameObject go)
    {
        TankAgent tankAgent = go.GetComponent<TankAgent>();
        if (tankAgent != null && tankAgent.PrivateAgentName != null)
        {
            if (_agentDestroy.ContainsKey(tankAgent.PrivateAgentName))
                _agentDestroy[tankAgent.PrivateAgentName] = true;
        }
    }

    public void LinkWithAgentActions(string id, Dictionary<string, string> attributes)
    {
        TankAgent tankAgent = agentsLinked[id];
        int movement = int.Parse(attributes["movement"]);
        int fire = int.Parse(attributes["fire"]);
        tankAgent.Actions(movement, fire == 1);
    }

    public void UnlinkWithAgent(string id)
    {
        GameLogic gameLogic = GameLogic.Get();
        gameLogic.PauseGame();
        agentsLinked.Remove(id);
        if (!gameLogic.IsGameOver)
            gameLogic.UnlinkAgent();
    }

    public void LinkWithAgent(string id, string agentInfo, int numAgent)
    {
        if(numAgent > 0 && numAgent <= agents.Count)
        {
            TankAgent tankAgent = agents[numAgent - 1].GetComponent<TankAgent>();
            if(tankAgent)
            {
                tankAgent.Configure(id, agentInfo);
                agentsLinked.Add(id, tankAgent);
                _agentDestroy.Add(id, false);
                _agentNames.Add(id, agentInfo);
            }
        }
    }

    public bool isWin
    {
        get
        {
            return _isWin;
        }
    }

    public List<GameObject> Enemies
    {
        get
        {
            return enemies;
        }
    }

    public List<GameObject> Agents
    {
        get
        {
            return agents;
        }
    }

    public void AddPlayer(GameObject p)
    {
        player = p;
    }

    public void RemovePlayer()
    {
        player = null;
    }


    public void AddEnemy(GameObject p)
    {
        enemies.Add(p);
    }

    public void RemoveEnemy(GameObject p)
    {
        enemies.Remove(p);
        if(!withExit)
            TryToWin();
    }

    public void TryToWin()
    {
        if (enemies.Count == 0 && agents.Count == 0)
        {
            PlayerWin();
        }
    }

    public void AddAgent(GameObject a)
    {
        agents.Add(a);
    }

    public void RemoveAgent(GameObject p)
    {
        TankAgent ag = p.GetComponent<TankAgent>();
        agents.Remove(p);
        if(!withExit)
            TryToWin();
    }

    // Start is called before the first frame update
    void Awake()
    {
        _unlinkAgents = 0;
        _isWin = false;
        agentsLinked = new Dictionary<string, TankAgent>();
        agentDestroy.gameObject.SetActive(false);
        gameOver.gameObject.SetActive(false);
        playerWin.gameObject.SetActive(false);
        waiting.gameObject.SetActive(false);
        ready.gameObject.SetActive(false);
        enemies = new List<GameObject>();
        agents = new List<GameObject>();
        _agentDestroy = new Dictionary<string, bool>();
        _agentNames = new Dictionary<string, string>();
        string tilemapStr = TileMapLoaderV2.CleanJSON(_textMap.text);
        TileMap map1 = JsonUtility.FromJson<TileMap>(tilemapStr);
        map1.ConvertToDictionary();
        _map = new Map(map1, TileMapLoaderV2.GROUND_LAYER, tileSize.x, tileSize.y);
        if(showHealth)
        {
            Vector2Int positionInTile = GenerateRandomItem();
            _map.UpdateTile(positionInTile, PerceptionBase.INPUT_TYPE.LIFE,false);
            life = GameMgr.Instance.GetSpawnerMgr().CreateNewGameObject(healthPrefab, _map.TileToWorldPosition(positionInTile, 0f), Quaternion.identity, null);
        }
        if (withExit)
        {
            //Generamos en una posición de las de arriba el exit.
            Vector2Int positionInTile = GenerateRandomItem(30);
            _map.UpdateTile(positionInTile, PerceptionBase.INPUT_TYPE.EXIT,false);
            exit = GameMgr.Instance.GetSpawnerMgr().CreateNewGameObject(exitPrefab, _map.TileToWorldPosition(positionInTile, 0f), Quaternion.identity, null);
        }
    }

    public GameObject Life
    {
        get
        {
            if (life == null)
                return null;
            return life;
        }
    }

    public GameObject Exit
    {
        get
        {
            if (exit == null)
                return null;
            return exit;
        }
    }

    public void DeleteLife()
    {
        life = null;
    }

    public Map Map
    {
        get
        {
            return _map;
        }
    }

    private void Start()
    {
        if(!logActive)
            logPannel.SetActive(false);
        if (waitingForAgents)
        {
            server.gameObject.SetActive(true);
        }
        _map.SetVisualMap(visualMap);
        visualMap.gameObject.SetActive(showVisualMap);
    }

    public bool IsReady
    {
        get
        {
            return isReady;
        }
    }

    public bool IsGameOver
    {
        get
        {
            return isGameOver;
        }
    }

    public Vector2Int GenerateRandomItem( int maxItems = int.MaxValue)
    {
        List<int> emptyTiles = new List<int>();
        for (int i = 0; i < _map.Lenght; i++)
        {
            if (_map[i] == TankPerception.INPUT_TYPE.NOTHING && i != _map.PlayerSpawnTile && !_map.IsTileAAgentSpawnPosition(i) && !_map.IsTileANativeAgentSpawnPosition(i))
                emptyTiles.Add(i);
        }
        if(maxItems == int.MaxValue)
        {
            int selected = emptyTiles[UnityEngine.Random.Range(0, emptyTiles.Count)];

            return _map.ConvertPosition(selected);
        }
        else
        {
            int selected = emptyTiles[UnityEngine.Random.Range(0, maxItems)];
            return _map.ConvertPosition(selected);
        }
    }

    //NOTA: método llamado desde un UnityEvent por BCServer
    public void OnInitTCPClient(string agentID,System.Net.Sockets.TcpClient client)
    {
        if (tcpClient == null)
            tcpClient = new Dictionary<string, System.Net.Sockets.TcpClient>();
        tcpClient.Add(agentID,client);
        if(tcpClient.Count == NumAgentSpected)
            Ready();
    }

    public void Log(string msg)
    {
        logActive = true;
        logPannel.SetActive(true);
        log.text += "["+Time.timeSinceLevelLoad+"] "+ msg +".\n";
    }

    public void ResetLog()
    {
        log.text = "";
    }

    public void Init()
    {
        isReady = false;
        if(waitingForAgents)
        {
            waiting.gameObject.SetActive(true);
            isGameOver = true;
            PauseGame();
            //server
        }
        else
        {
            Ready();
        }
    }

    public void Ready()
    {
        isReady = true;
        ready.gameObject.SetActive(true);
        waiting.gameObject.SetActive(false);
        isGameOver = false;
        onReady?.Invoke();

        ResumeGame();
    }
    // Update is called once per frame
    void Update()
    {
        GameMgr.Instance.TimeMgr.DeferredCall(this, initTime, DefferedInit);
        enabled = false;
    }

    public void DefferedInit()
    {
        Init();
        enabled = false;
    }

    public void PlayerWin()
    {
        gameFinishCallback?.Invoke(true);
        playerWin.gameObject.SetActive(true);
        isGameOver = true;
        _isWin = true;
        FinishGame(null);
    }

    //agentID es el agente que ha producido el gameOver (Quien ha ganado)
    public void GameOver(GameObject attacker)
    {
        gameFinishCallback?.Invoke(false);
        ready.gameObject.SetActive(false);
        gameOver.gameObject.SetActive(true);
        isGameOver = true;
        _isWin = false;
        if(attacker != null)
        {
            TankAgent tankAgent = attacker.GetComponent<TankAgent>();
            FinishGame(tankAgent);
        }
    }

    protected void FinishGame(TankAgent tankAgent)
    {
        if(tankAgent != null)
        {
            try
            {
                foreach (string key in tcpClient.Keys)
                {
                    bool destroyed = key != tankAgent.PrivateAgentName;
                    server.SendPerception(tcpClient[key], null, IsGameOver, destroyed);
                }

            }
            catch (Exception e)
            {
                GameLogic.Get().Log("Exception " + e);
            }
        }
        PauseGame();
    }

    public void UnlinkAgent()
    {
        _unlinkAgents++;
        if(_unlinkAgents >= _agentDestroy.Count)
        {
            agentDestroy.gameObject.SetActive(true);
            agentName.gameObject.SetActive(true);

            if (_agentDestroy.Count > 1)
            {
                foreach (string key in _agentDestroy.Keys)
                {
                    if (!_agentDestroy[key])
                        _agentwinNameWin = _agentNames[key];
                }
                if (_agentwinNameWin != null)
                    agentName.text = _agentwinNameWin + " Win ";
                else
                    agentName.text = " TIE ";
            }
            else if (_agentDestroy.Count == 1)
            {
                agentName.text = "You Lost";
            }
            else
            {
                agentName.text = "";
            }

        }
    }

    protected void PauseGame()
    {
        if(player != null)
            player.GetComponent<PlayerInputController>().enabled = false;
        for (int i = 0; i < enemies.Count; i++)
        {
            if(enemies[i] != null)
                enemies[i].GetComponent<EnemyController>().Pause();
        }

        for (int i = 0; i < agents.Count; i++)
        {
            if (agents[i] != null)
                agents[i].GetComponent<AgentController>().Pause();
        }
    }
    protected void ResumeGame()
    {
        if (player != null)
            player.GetComponent<PlayerInputController>().enabled = true;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
                enemies[i].GetComponent<EnemyController>().Init();
        }

        for (int i = 0; i < agents.Count; i++)
        {
            if (agents[i] != null)
                agents[i].GetComponent<AgentController>().Init();
        }
    }
}
