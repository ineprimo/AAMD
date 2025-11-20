using UnityEngine;
using System.Collections;

//Generador de enemigos. Puede generar enemigos aleatoriamente hasta un cierto numero.
//Los genera al rededor suyo. 
public abstract class Generator : MonoBehaviour
{

	private string m_icon = "Electric_Generator_icon.png";
    public float m_distanceToActivate;
    public bool firstEnemyInmdiatly;
	public float  m_timeToGenerateS = 5.0f;
	public int m_numMaxEnemies = 20;
	public int m_numEnemies = 0;
	public GameObject[] _prefabs;
    public Vector3 m_offset = new Vector3(0.5f, 0.0f, 0.5f);



    private float m_timeToLastEnemyGenerate = 0;
    private GameObject[] _players;
	//El gizmo debe estar en assets/Gizmos
	void OnDrawGizmos() 
	{
		Gizmos.DrawIcon(transform.position, m_icon,true);
	}
	
	// Use this for initialization
	void Start () 
	{
        GameObject[] players = Utils.GetPlayers();
        _players = new GameObject[players.Length];
        for(int i = 0; i < _players.Length; ++i)
        {
            _players[i] = players[i].gameObject;
        }
        if (firstEnemyInmdiatly)
            m_timeToLastEnemyGenerate = 0f;
        else
            m_timeToLastEnemyGenerate = m_timeToGenerateS;
	}
	
	// Update is called once per frame
	void Update(){

        Transform nearestPlayer = Utils.GetNearest(_players,this.gameObject);
        float distance = Mathf.Abs(nearestPlayer.position.x - transform.position.x);
        if (distance < m_distanceToActivate)
        {
            m_timeToLastEnemyGenerate -= Time.deltaTime;
            //Cuando termina el tiempo entre generacion y generacion, creamos un objeto del pool.
            if (m_timeToLastEnemyGenerate <= 0 && _prefabs.Length > 0 && (m_numEnemies < m_numMaxEnemies || m_numMaxEnemies < 0))

            {
                m_numEnemies++;
                m_timeToLastEnemyGenerate = m_timeToGenerateS;
                int enemyIdx = Random.Range(0, _prefabs.Length);
                Vector3 position = CalculatePosition();

                //Par instanciarlo usamos el SpawnerMgr. Si lo usamos podremos utilizar el pool de objetos precargados
                //lo que nos ayudara a reducir la fragmentacio nde memoria.
                GameObject go = GameMgr.Instance.GetSpawnerMgr().CreateNewGameObject(_prefabs[enemyIdx], position, Quaternion.identity,this.transform.parent);

            }
        }
	}

    protected abstract Vector3 CalculatePosition();
	

}