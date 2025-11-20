using UnityEngine;

public class TankControllerSwitch : MonoBehaviour
{
    public Record record;
    public PlayerPerception playerPerception;
    public MLAgent agent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameLogic gameLogic = GameLogic.Get();
        if(gameLogic.gameMode == GameLogic.GAME_MODE.RECORDED)
        {
            record.recordMode = true;
            record.enabled = true;
            playerPerception.enabled = true;
        }
        else if(gameLogic.gameMode == GameLogic.GAME_MODE.IA_DRIVEN)
        {
            record.recordMode = false;
            record.enabled = false;
            playerPerception.enabled = true;
            agent.enabled = true;
        }
    }

}
