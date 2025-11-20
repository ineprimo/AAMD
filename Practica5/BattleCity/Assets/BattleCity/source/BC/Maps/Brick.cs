using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : BrickContainer
{
    public List<Breakable> brekables;

    private void Start()
    {
        for(int i = 0; i < brekables.Count; i++)
        {
            brekables[i].Parent = this;
        }
    }


    private void OnDestroy()
    {

    }

    public override void ChildDestroy(Breakable b)
    {
        brekables.Remove(b);
        if (brekables.Count == 0)
        {
            DestroyBrick();
        }
    }

    public virtual void DestroyBrick()
    {
        Map map = GameLogic.Get().Map;
        Vector2Int tile = map.GetTile(this.transform.position);
        map.UpdateTile(tile, TankPerception.INPUT_TYPE.NOTHING);
    }
}
