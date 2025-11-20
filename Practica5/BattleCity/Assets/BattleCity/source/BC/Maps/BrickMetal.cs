using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickMetal : Brick
{

    public override void DestroyBrick()
    {
        Map map = GameLogic.Get().Map;
        Vector2Int tile = map.GetTile(this.transform.position);
        map.UpdateTile(tile, TankPerception.INPUT_TYPE.SEMI_UNBREKABLE);
    }
}
