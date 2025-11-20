using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAllSoundsWhenDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnDestroy()
    {
        SoundMgr sound = GameMgr.Instance.SoundMgr;
        if(sound != null)
        {
            sound.StopAllSounds();
        }
    }

    private void OnDisable()
    {
        SoundMgr sound = GameMgr.Instance.SoundMgr;
        if (sound != null)
        {
            sound.PauseAllSounds();
        }
    }

    private void OnEnable()
    {
        SoundMgr sound = GameMgr.Instance.SoundMgr;
        if (sound != null)
        {
            sound.ResumeAllSounds();
        }
    }

}
