using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankRandomBehavior : EnemyController
{
    public TankPerception perception;
    public TankCommand tankCommand;
    public float timeToChange;
    public float timeToAttach;

    private float time;
    private float timeAttack;
    private GameLogic gameLogic;
    private int lastAction;
    // Start is called before the first frame update
    void Start()
    {
        time = timeToChange;
        timeAttack = timeToAttach;
        gameLogic = GameLogic.Get();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameLogic.gameDifilty == GameLogic.GAME_DIFICULT.EASY)
        {
            EasyMode();
        }
        else
        {
            NormalMode();
        }
    }

    public void EasyMode()
    {
        RandomMove();
        ShootEasy();
    }

    public void NormalMode()
    {
        int decision = ShootNormal();
        if(decision < 0)
        {
            RandomMove();
        }
        else
        {
            if(decision>0)
                lastAction = decision-1;
            tankCommand.RunCommand(decision);
        }
    }

    public void RandomMove()
    {
        time = time - Time.deltaTime;
        if (time <= 0)
        {
            time = timeToChange + time;
            int action = MakeADecision();
            if(action > 0)
                lastAction = action-1;
            tankCommand.RunCommand(action);
        }
    }

    public void ShootEasy()
    {
        timeAttack -= Time.deltaTime;
        if (timeAttack <= 0)
        {
            timeAttack = timeToAttach + timeAttack;
            bool fire = MakeDecisionFire();
            tankCommand.RunCommand(fire);
        }
    }

    public void NormalShoot()
    {
        if (timeAttack <= 0)
        {
            timeAttack = timeToAttach + timeAttack;
            tankCommand.RunCommand(true);
        }
    }

    public int ShootNormal()
    {
        if (perception.CanFire)
        {
            timeAttack -= Time.deltaTime;
            if(perception.Parameters != null)
            {
                if (NeedShoot(TankPerception.PARAMETER_ID.NEIGHBORHOOD_UP) && IsOriented(TankPerception.PARAMETER_ID.NEIGHBORHOOD_UP))
                    NormalShoot();
                else if (NeedShoot(TankPerception.PARAMETER_ID.NEIGHBORHOOD_DOWN) && IsOriented(TankPerception.PARAMETER_ID.NEIGHBORHOOD_DOWN))
                    NormalShoot();
                else if (NeedShoot(TankPerception.PARAMETER_ID.NEIGHBORHOOD_LEFT) && IsOriented(TankPerception.PARAMETER_ID.NEIGHBORHOOD_LEFT))
                    NormalShoot();
                else if (NeedShoot(TankPerception.PARAMETER_ID.NEIGHBORHOOD_RIGHT) && IsOriented(TankPerception.PARAMETER_ID.NEIGHBORHOOD_RIGHT))
                    NormalShoot();
                else
                {
                    int upPlayerDecision = MoveIS(TankPerception.PARAMETER_ID.NEIGHBORHOOD_UP, PerceptionBase.INPUT_TYPE.PLAYER);
                    int dawnPlayerDecision = MoveIS(TankPerception.PARAMETER_ID.NEIGHBORHOOD_DOWN, PerceptionBase.INPUT_TYPE.PLAYER);
                    int leftPlayerDecision = MoveIS(TankPerception.PARAMETER_ID.NEIGHBORHOOD_LEFT, PerceptionBase.INPUT_TYPE.PLAYER);
                    int rightPlayerDecision = MoveIS(TankPerception.PARAMETER_ID.NEIGHBORHOOD_RIGHT, PerceptionBase.INPUT_TYPE.PLAYER);
                    if (upPlayerDecision > 0)
                        return upPlayerDecision;
                    if (dawnPlayerDecision > 0)
                        return dawnPlayerDecision;
                    if (leftPlayerDecision > 0)
                        return leftPlayerDecision;
                    if (rightPlayerDecision > 0)
                        return rightPlayerDecision;
                    int upCCDecision = MoveIS(TankPerception.PARAMETER_ID.NEIGHBORHOOD_UP, PerceptionBase.INPUT_TYPE.COMMAND_CENTER);
                    int dawnCCDecision = MoveIS(TankPerception.PARAMETER_ID.NEIGHBORHOOD_DOWN, PerceptionBase.INPUT_TYPE.COMMAND_CENTER);
                    int leftCCDecision = MoveIS(TankPerception.PARAMETER_ID.NEIGHBORHOOD_LEFT, PerceptionBase.INPUT_TYPE.COMMAND_CENTER);
                    int rightCCDecision = MoveIS(TankPerception.PARAMETER_ID.NEIGHBORHOOD_RIGHT, PerceptionBase.INPUT_TYPE.COMMAND_CENTER);
                    if (upCCDecision > 0)
                        return upCCDecision;
                    if (dawnCCDecision > 0)
                        return dawnCCDecision;
                    if (leftCCDecision > 0)
                        return leftCCDecision;
                    if (rightCCDecision > 0)
                        return rightCCDecision;

                    //return -1;
                    //me muevo en esa dirección
                }
            }
        }
        return -1;
    }

    public bool IsOriented(TankPerception.PARAMETER_ID param)
    {
        return lastAction == (int)param;
    }

    public int MoveIS(TankPerception.PARAMETER_ID dir, PerceptionBase.INPUT_TYPE tp)
    {
        PerceptionBase.INPUT_TYPE input = perception.GetPerceptionDirection(dir);
        if (input == tp)
            return ((int)dir) +1;
        else
            return -1;
    }

    public bool NeedShoot(TankPerception.PARAMETER_ID param)
    {
        PerceptionBase.INPUT_TYPE input = perception.GetPerceptionDirection(param);
        return input == PerceptionBase.INPUT_TYPE.COMMAND_CENTER || input == PerceptionBase.INPUT_TYPE.BRICK || input == PerceptionBase.INPUT_TYPE.PLAYER;
    }

    public int MakeADecision()
    {
        int action = Random.Range(0, 5);
        return action;
    }

    public bool MakeDecisionFire()
    {
        if (perception.CanFire)
        {
            int fire = Random.Range(0, 2);
            return fire == 1;
        }
        return false;
    }
}
