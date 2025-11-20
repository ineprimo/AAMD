using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundData
{
    private AudioClip _clip = null;//new AudioClip();
    private bool _loop = false;
    private string _name=null;
    private string _fileName=null;
    private string _sceneName=null;
    private float _volume;

    public SoundData(MusicResourceInfo info)
    {
        _name = info.name;
        _loop = info.loop;
        _fileName = info.fileName;
        _sceneName = info.sceneName;
        _volume = info.volume;
    }

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public AudioClip Clip
    {
        get { return _clip; }
        set { this._clip = value; }
    }

    public bool Loop
    {
        get { return _loop; }
        set { this._loop = value; }
    }

    public float Volume
    { 
        get { return _volume; }
        set { _volume = value; }
    }

    public string FileName
    {
        get { return _fileName; }
        set { _fileName = value; }
    }

    public string SceneName
    {
        get{return _sceneName;}
        set{_sceneName = value;}
    }
}
