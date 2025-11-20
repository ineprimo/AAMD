using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectDirtyGameMgr : MonoBehaviour
{
    protected bool gmrInit;
    public void Init()
    {
        gmrInit = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        if(!gmrInit)
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
