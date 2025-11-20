using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPerception : PerceptionBase
{

    private Transform player;
    private Transform agentRival;

    Vector2 playerPosition =  Vector2.zero;
    GameLogicListener gameLogicListener;





    public Vector2 PlayerPosition
    {
        get { return playerPosition; }
    }


    public Transform Player
    {
        get
        {
            if (player == null)
            {
                GameObject playerGo = GameObject.FindGameObjectWithTag(Globals.PlayerTag);
                if (playerGo != null)
                    player = playerGo.transform;
            }
            return player;
        }
    }

    public INPUT_TYPE GetPerceptionDirection(PARAMETER_ID id)
    {
        if(id == PARAMETER_ID.NEIGHBORHOOD_UP || id == PARAMETER_ID.NEIGHBORHOOD_DOWN || id == PARAMETER_ID.NEIGHBORHOOD_RIGHT || id == PARAMETER_ID.NEIGHBORHOOD_LEFT)
        {
            float inputFloat = parameters[(int)id];
            int inputInt = (int)inputFloat;
            INPUT_TYPE t = (INPUT_TYPE)inputInt;
            return t;
        }
        Debug.LogException(new Exception("id " + id + " no es un ID convertible a INPUT_TYPE"));
        return INPUT_TYPE.NOTHING;
    }

    public float GetPerceptionFloat(PARAMETER_ID id)
    {
        if (id == PARAMETER_ID.NEIGHBORHOOD_DIST_UP || id == PARAMETER_ID.NEIGHBORHOOD_DIST_DOWN || id == PARAMETER_ID.NEIGHBORHOOD_DIST_RIGHT || 
            id == PARAMETER_ID.NEIGHBORHOOD_DIST_LEFT || id == PARAMETER_ID.PLAYER_X || id == PARAMETER_ID.PLAYER_Y || id == PARAMETER_ID.COMMAND_CENTER_X ||
             id == PARAMETER_ID.COMMAND_CENTER_Y || id == PARAMETER_ID.LIFE_X || id == PARAMETER_ID.LIFE_Y)
        {
            float inputFloat = parameters[(int)id];
            return inputFloat;
        }
        Debug.LogException(new Exception("id " + id + " no es un ID convertible a float"));
        return -1f;
    }

    public Transform AgentRival
    {
        get
        {
            if (agentRival == null)
            {
                GameObject[] agents = GameObject.FindGameObjectsWithTag(Globals.EnemyTag);
                if (agents != null)
                {
                    for (int i = 0; agentRival == null && i < agents.Length; i++)
                    {
                        TankAgent tankAgent =  agents[i].GetComponent<TankAgent>();
                        if(tankAgent != null)
                        {
                            agentRival = agents[i].gameObject != this.gameObject ? agents[i].transform : null;
                        }
                    }
                }
            }
            return agentRival;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        this._Start();
        player=Player;
    }

    // Update is called once per frame
    void Update()
    {
        this._Update((int)INPUT_TYPE.NOTHING);

        bool isMultyAgent = GameLogic.Get().IsMultyAgent;

        
        if (!isMultyAgent)
            playerPosition = Player != null ? Player.position : Vector2.zero;
        else
            playerPosition = AgentRival != null ? AgentRival.position : Vector2.zero;

            
        parameters = ReadParameters(Enum.GetNames(typeof(PARAMETER_ID)).Length, time, this,health.health);
    }

    public string TankName
    {
        get
        {
            if (gameLogicListener == null)
                gameLogicListener = GetComponent<GameLogicListener>();

            if (gameLogicListener.entityType == GameLogicListener.EntityType.AGENT)
            {
                return GetComponent<TankAgent>().PublicAgentName;
            }
            else if (gameLogicListener.entityType == GameLogicListener.EntityType.ENEMY)
                return this.gameObject.name;
            else
                return "Player";
        }
    }



    public static MLGym.Parameters ReadParameters(int numParameters, float t, TankPerception perception, int health)
    {

        MLGym.Parameters p = new MLGym.Parameters(numParameters, t);
        for (int i = 0; i < perception.PerceptionNeighborhood.Length; i++)
        {
            p[i] = perception.PerceptionNeighborhood[i];
        }

        for (int i = 0; i < perception.PerceptionNeighborhoodDistance.Length; i++)
        {
            p[4+i] = perception.PerceptionNeighborhoodDistance[i];
        }

        p[(int)PARAMETER_ID.PLAYER_X] = perception.PlayerPosition.x;
        p[(int)PARAMETER_ID.PLAYER_Y] = perception.PlayerPosition.y;
        p[(int)PARAMETER_ID.COMMAND_CENTER_X] = perception.CommandCenterPosition.x;
        p[(int)PARAMETER_ID.COMMAND_CENTER_Y] = perception.CommandCenterPosition.y;
        p[(int)PARAMETER_ID.AGENT_X] = perception.transform.position.x;
        p[(int)PARAMETER_ID.AGENT_Y] = perception.transform.position.y;
        p[(int)PARAMETER_ID.CAN_FIRE] = perception.CanFire ? 1 : 0;
        p[(int)PARAMETER_ID.HEALTH] = health;
        if(GameLogic.Get().Life != null)
        {
            p[(int)PARAMETER_ID.LIFE_X] = perception.LifePosition.x;
            p[(int)PARAMETER_ID.LIFE_Y] = perception.LifePosition.y;
        }
        else
        {
            p[(int)PARAMETER_ID.LIFE_X] = -1;
            p[(int)PARAMETER_ID.LIFE_Y] = -1;
        }

        return p;
    }

    public enum PARAMETER_ID
    {
        NEIGHBORHOOD_UP = 0, NEIGHBORHOOD_DOWN = 1, NEIGHBORHOOD_RIGHT = 2, NEIGHBORHOOD_LEFT = 3,
        NEIGHBORHOOD_DIST_UP = 4, NEIGHBORHOOD_DIST_DOWN = 5, NEIGHBORHOOD_DIST_RIGHT = 6, NEIGHBORHOOD_DIST_LEFT = 7,
        PLAYER_X = 8, PLAYER_Y = 9, COMMAND_CENTER_X = 10, COMMAND_CENTER_Y = 11, AGENT_X = 12, AGENT_Y = 13, CAN_FIRE = 14, HEALTH = 15, LIFE_X = 16, LIFE_Y = 17
    }



}
