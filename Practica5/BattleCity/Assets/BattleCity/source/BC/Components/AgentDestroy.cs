using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentDestroy : Destroy
{
    public GameObject graphics;
    protected bool isDestroy=false;

    public bool IsDestroy
    {
        get
        {
            return isDestroy;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        isDestroy = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void InitDestroy(GameObject attacker)
    {
        GameMgr.Instance.GetSpawnerMgr().CreateNewGameObject(explossion, this.transform.position, Quaternion.identity, null);
        graphics.SetActive(false);
        isDestroy = true;
        GameLogic.Get().AgentDestroy(this.gameObject);
        //base.InitDestroy();
        //Hay que informar AL AGENTE.
    }
}
