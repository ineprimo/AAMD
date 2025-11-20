using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TankAgent : AgentController
{
    public float snaptShotTime;
    public TankPerception perception;
    public TankCommand tankCommand;
    public AgentDestroy agentDestroy;
    public TMP_Text agentName;
    public string addAgentSound;
    public bool enableSendMap;

    private float time;
    private string agentID;
    private string agent;
    private bool recivedActions = false;
    private CommandLineReader commandLineReader;
    private float perceptionsize;
    // Start is called before the first frame update
    void Start()
    {
        enabled = false;
        commandLineReader = new CommandLineReader();
        if(commandLineReader.ExistCommand("-snapshot"))
        {
            snaptShotTime = float.Parse(commandLineReader.GetCommandArgument("-snapshot"));
        }
        if (commandLineReader.ExistCommand("-perceptionsize"))
        {
            perceptionsize = float.Parse(commandLineReader.GetCommandArgument("-perceptionsize"));
            perception.perceptionSizeFactor = perceptionsize;
        }
    }

    public void Configure(string id, string a)
    {
        time = snaptShotTime;
        agentID = id;
        agent = a;
        agentName.text = agent;
        recivedActions = true;
        GameMgr.Instance.GetServer<SoundMgr>().PlaySound(addAgentSound);
    }

    public string PublicAgentName
    {
        get
        {
            return agent;
        }
    }

    public string PrivateAgentName
    {
        get
        {
            return agentID;
        }
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if(time <= 0f)
        {
            time = snaptShotTime + time;
            MLGym.Parameters parameters = perception.Parameters;
            if(parameters != null)
            {
                try
                {
                    if(recivedActions)
                    {
                        if(!enableSendMap)
                            GameLogic.Get().SendPerception(agentID,parameters, agentDestroy.IsDestroy);
                        else
                            GameLogic.Get().SendPerceptionAndMap(agentID,parameters, agentDestroy.IsDestroy);
                        recivedActions = false;
                    }
                        
                }
                catch(Exception e)
                {
                    recivedActions = true;
                    GameLogic.Get().Log("Exception "+e);
                }
            }
        }
    }

    public void Actions(int movement, bool fire)
    {
        if(!recivedActions)
        {
            recivedActions = true;
            tankCommand.RunCommand(movement, fire);
        }
    }
}
