using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDestroy : Destroy
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
        GameLogic.Get().GameOver(attacker);
    }
}
