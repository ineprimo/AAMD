using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCenter : Destroy
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void InitDestroy(GameObject attacker)
    {
        base.InitDestroy(attacker);
        Map map = GameLogic.Get().Map;
        Vector2Int tile = map.GetTile(this.transform.position);
        map.UpdateTile(tile, TankPerception.INPUT_TYPE.NOTHING);
        GameLogic.Get().GameOver(attacker);
    }
}
