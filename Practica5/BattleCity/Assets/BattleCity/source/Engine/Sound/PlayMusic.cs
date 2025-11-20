using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayMusic : MonoBehaviour
{
    public string MusicTitle;
    public float Music_Fade = 0;
    public string AmbienceTitle;
    public float Ambience_Fade = 0;
    public bool playJustOnStart = true;


    // Start is called before the first frame update
    void Start()
    {
        if (playJustOnStart)
            Play();
    }

    public void Play()
    {
        if (MusicTitle != "")
        {
            Play(MusicTitle, Music_Fade, false);
        }
        else
            Stop(Music_Fade, false);

        if (AmbienceTitle != "")
        {
            Play(AmbienceTitle, Ambience_Fade, true);
        }
        else
            Stop(Ambience_Fade, true);
    }

    public void Play(string title, float fadeIn, bool isAmbience)
    {
        GameMgr.Instance.GetServer<SoundMgr>().PlayMusic(title, fadeIn, isAmbience);
    }

    public void Stop(float fadeOut, bool isAmbience)
    {
        Debug.Log("STOPPIN'");
        SoundMgr smgr = GameMgr.Instance.GetServer<SoundMgr>();
        if(smgr != null)
            smgr.StopMusic(fadeOut, isAmbience);
    }

}
