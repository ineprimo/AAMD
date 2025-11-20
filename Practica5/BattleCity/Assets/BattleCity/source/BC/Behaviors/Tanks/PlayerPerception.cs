using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPerception : PerceptionBase
{
    public int numAgents = 2;
    private Transform[] agentRivals;
    private Transform exit;
    private Vector3 exitPosition;

    private List<int> lastActions;
    private const int NUM_CONTEXT = 5;
    private int actions = 0;

    void Start()
    {
        this._Start();
        lastActions = new List<int>();
        for(int i = 0; i < NUM_CONTEXT; i++)
        {
            lastActions.Add(0);
        }
        agentRivals = new Transform[numAgents];
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int numTanks = 0;
        for(int i = 0; i < enemies.Length; i++)
        {
            TankColor tankC = enemies[i].GetComponent<TankColor>();
            if(tankC != null)
            {
                agentRivals[numTanks] = enemies[i].transform;
                numTanks++;
            }
            
        }
    }

    public void AddAction(int action)
    {
        if(actions < NUM_CONTEXT)
        {
            lastActions[actions] = action;
        }
        else
        {
            lastActions.RemoveAt(0);
            lastActions.Add(action);
        }
        actions++;
    }

    public List<int> PreviousActions
    {
        get
        {
            return lastActions;
        }
    }



    public Vector2 ExitPosition
    {
        get
        {
            return exitPosition;
        }
    }



    public Transform Exit
    {
        get
        {
            if (exit == null)
            {
                GameObject ex = GameLogic.Get().Exit;
                if (ex != null)
                    exit = ex.transform;
            }
            return exit;
        }
    }

    // Update is called once per frame
    void Update()
    {
        this._Update((int)INPUT_TYPE.NOTHING);

        bool isMultyAgent = GameLogic.Get().IsMultyAgent;
        exitPosition = Exit != null ? Exit.position : Vector2.zero;

        parameters = ReadParameters(Enum.GetNames(typeof(Player_PARAMETER_ID)).Length, time, this, health.health);
    }

    public enum Player_PARAMETER_ID
    {
        NEIGHBORHOOD_UP = 0, NEIGHBORHOOD_DOWN = 1, NEIGHBORHOOD_RIGHT = 2, NEIGHBORHOOD_LEFT = 3,
        NEIGHBORHOOD_DIST_UP = 4, NEIGHBORHOOD_DIST_DOWN = 5, NEIGHBORHOOD_DIST_RIGHT = 6, NEIGHBORHOOD_DIST_LEFT = 7,
        COMMAND_CENTER_X = 8, COMMAND_CENTER_Y = 9, AGENT_1_X = 10, AGENT_1_Y = 11, AGENT_2_X = 12, AGENT_2_Y = 13, CAN_FIRE = 14, HEALTH = 15, LIFE_X = 16, 
        LIFE_Y = 17, EXIT_X = 18, EXIT_Y = 19//, ACT_1 = 20, ACT_2 = 21, ACT_3 = 22, ACT_4 = 23, ACT_5 = 24

    }

    public static string[] GetParameterNames()
    {
        Array ar = Enum.GetValues(typeof(Player_PARAMETER_ID));
        string[] names = new string[ar.Length];
        foreach (Player_PARAMETER_ID param in ar)
        {
            names[(int)param] = param.ToString();
        }
        return names;
    }

    public static MLGym.Parameters ReadParameters(int numParameters, float t, PlayerPerception perception, int health)
    {

        MLGym.Parameters p = new MLGym.Parameters(numParameters, t);
        for (int i = 0; i < perception.PerceptionNeighborhood.Length; i++)
        {
            p[i] = perception.PerceptionNeighborhood[i];
        }

        for (int i = 0; i < perception.PerceptionNeighborhoodDistance.Length; i++)
        {
            p[4 + i] = perception.PerceptionNeighborhoodDistance[i];
        }

        p[(int)Player_PARAMETER_ID.COMMAND_CENTER_X] = perception.CommandCenterPosition.x;
        p[(int)Player_PARAMETER_ID.COMMAND_CENTER_Y] = perception.CommandCenterPosition.y;
        if(perception.agentRivals[0] != null)
        {
            p[(int)Player_PARAMETER_ID.AGENT_1_X] = perception.agentRivals[0].transform.position.x;
            p[(int)Player_PARAMETER_ID.AGENT_1_Y] = perception.agentRivals[0].transform.position.y;
        }
        else
        {
            p[(int)Player_PARAMETER_ID.AGENT_1_X] = 100;
            p[(int)Player_PARAMETER_ID.AGENT_1_Y] = 100;
        }

        if (perception.agentRivals[1] != null)
        {
            p[(int)Player_PARAMETER_ID.AGENT_2_X] = perception.agentRivals[1].transform.position.x;
            p[(int)Player_PARAMETER_ID.AGENT_2_Y] = perception.agentRivals[1].transform.position.y;
        }
        else
        {
            p[(int)Player_PARAMETER_ID.AGENT_2_X] = 100;
            p[(int)Player_PARAMETER_ID.AGENT_2_Y] = 100;
        }


        p[(int)Player_PARAMETER_ID.CAN_FIRE] = perception.CanFire ? 1 : 0;
        p[(int)Player_PARAMETER_ID.HEALTH] = health;
        if (GameLogic.Get().Life != null)
        {
            p[(int)Player_PARAMETER_ID.LIFE_X] = perception.LifePosition.x;
            p[(int)Player_PARAMETER_ID.LIFE_Y] = perception.LifePosition.y;
        }
        else
        {
            p[(int)Player_PARAMETER_ID.LIFE_X] = 100;
            p[(int)Player_PARAMETER_ID.LIFE_Y] = 100;
        }
        p[(int)Player_PARAMETER_ID.EXIT_X] = perception.ExitPosition.x;
        p[(int)Player_PARAMETER_ID.EXIT_Y] = perception.ExitPosition.y;

        /*p[(int)Player_PARAMETER_ID.ACT_1] = perception.PreviousActions[0];
        p[(int)Player_PARAMETER_ID.ACT_2] = perception.PreviousActions[1];
        p[(int)Player_PARAMETER_ID.ACT_3] = perception.PreviousActions[2];
        p[(int)Player_PARAMETER_ID.ACT_4] = perception.PreviousActions[3];
        p[(int)Player_PARAMETER_ID.ACT_5] = perception.PreviousActions[4];*/
        return p;
    }
}
