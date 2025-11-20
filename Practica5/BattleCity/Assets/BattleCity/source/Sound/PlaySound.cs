using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public string clip;
    public bool isLoop;
    public bool isPositional;
    public float MaxDistance = 30f, distanceToMax = 8f;
    public float minPan = 0.3f;
    public int priority;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Play()
    {
        if(isLoop)
        {
            if (isPositional)
                GameMgr.Instance.GetServer<SoundMgr>().PlayLoopSoundPositional(clip, MaxDistance, distanceToMax, minPan, this.gameObject, priority);
            else
                GameMgr.Instance.GetServer<SoundMgr>().PlaySound(clip);
        }
        else
        {
            if (isPositional)
                GameMgr.Instance.GetServer<SoundMgr>().PlayOneShotPositional(clip, this.gameObject.transform, MaxDistance, distanceToMax, minPan, priority);
            else
                GameMgr.Instance.GetServer<SoundMgr>().PlaySound(clip);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
