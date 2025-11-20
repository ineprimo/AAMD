using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicListener : MonoBehaviour
{
    public enum EntityType { PLAYER, ENEMY, AGENT }
    public EntityType entityType;
    // Start is called before the first frame update
    void Start()
    {
        if (entityType == EntityType.PLAYER)
            GameLogic.Get().AddPlayer(this.gameObject);
        else if (entityType == EntityType.ENEMY)
            GameLogic.Get().AddEnemy(this.gameObject);
        else
            GameLogic.Get().AddAgent(this.gameObject);
    }

    private void OnDestroy()
    {
        if (GameLogic.Get() == null)
            return;
        if (entityType == EntityType.PLAYER)
            GameLogic.Get().RemovePlayer();
        else if (entityType == EntityType.ENEMY)
            GameLogic.Get().RemoveEnemy(this.gameObject);
        else
            GameLogic.Get().RemoveAgent(this.gameObject);
    }
}
