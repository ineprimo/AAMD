using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMusic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Stop()
    {

        GameMgr.Instance.GetServer<SoundMgr>().StopMusic();
    }
}
