using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugOnly : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameMgr.Instance.BuildType == Globals.BUILD_TYPE.PRODUCTION)
            this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
