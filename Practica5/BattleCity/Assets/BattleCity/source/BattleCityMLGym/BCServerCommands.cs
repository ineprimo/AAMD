using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BCServerCommands : MLGym.ServerCommands
{
    // Start is called before the first frame update
    public GameLogic gameLogic;

    private int numAgentsAdd;

    public override bool AgentAllowed(string id, string agentInfo)
    {
        bool correct = base.AgentAllowed(id, agentInfo);
        if(correct)
        {
            return numAgentsAdd < gameLogic.NumAgentSpected;
        }
        return correct;
    }

    //NOTA: método llamado desde un UnityEvent por BCServer
    public void OnAddAgent(string id, string agentInfo)
    {
        numAgentsAdd++;
        gameLogic.LinkWithAgent(id, agentInfo, numAgentsAdd);
    }

    //NOTA: método llamado desde un UnityEvent por BCServer
    public void OnRemoveAgent(string id)
    {
        numAgentsAdd--;
        gameLogic.UnlinkWithAgent(id);
    }

    public override void Actions(string id, Dictionary<string, string> attributes)
    {
        gameLogic.LinkWithAgentActions(id, attributes);
    }

    protected override void Log(string msg)
    {
        base.Log(msg);
        gameLogic.Log(msg);
    }
}
