using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundPosition
{
    protected List<GameObject> soundPositions;
    protected float maxDistance;
    protected float distanceToMaxVolumen;
    protected float minPan;

    public SoundPosition(float mxD, float distVolum, float mPan)
    {
        maxDistance = mxD;
        distanceToMaxVolumen = distVolum;
        minPan = mPan;
        soundPositions = new List<GameObject>();
        
    }

    public void AddPosition(GameObject t)
    {
        soundPositions.Add(t);
    }

    public void Remove(GameObject t)
    {
        soundPositions.Remove(t);
    }

    public List<GameObject> GetGameObjectList()
    {
        return soundPositions;
    }

    /*
    protected float maxDistance;
    protected float distanceToMaxVolumen;
    protected float minPan;
    */

    public float MaxDistance
    {
        get { return maxDistance; }
        set { maxDistance = value; }
    }

    public float DistanceToMaxVolumen
    {
        get { return distanceToMaxVolumen; }
        set { distanceToMaxVolumen = value; }
    }

    public float MinPan
    {
        get { return minPan; }
        set { minPan = value; }
    }

}

public class SoundSource : IComparable<SoundSource>
{
    protected string audioClipName;
    protected AudioSource audioSource;
    protected int priority;
    protected SoundChannels soundChannel;
    protected Coroutine corutine;
    protected SoundPosition soundPosition;
    protected float maxVolumen;

    public SoundSource(string acN, AudioSource adS, SoundChannels channel, float vol, int p)
    {
        audioClipName = acN;
        audioSource = adS;
        priority = p;
        soundChannel = channel;
        corutine = null;
        soundPosition = null;
        maxVolumen = vol;
    }

    public SoundChannels SoundChannel
    {
        get { return soundChannel; }
        set { soundChannel = value; }
    }

    public float MaxVolume
    {
        get { return maxVolumen; }
        set { maxVolumen = value; }
    }

    public Coroutine Coroutine
    {
        get { return corutine; }
        set { corutine = value; }
    }

    public string ClipName
    {
        get { return audioClipName; }
    }

    public AudioSource AudioSource
    {
        get { return audioSource; }
        set { audioSource = value; }
    }

    public SoundPosition SoundPosition
    {
        get { return soundPosition; }
        set { soundPosition = value; }
    }

    public void Stop(SoundMgr sm)
    {
        if (corutine != null)
            sm.StopCoroutine(corutine);
        corutine = null;
        audioSource.Stop();
    }

    public void Pause()
    {
        audioSource.Pause();
    }

    public void Resume()
    {
        audioSource.UnPause();
    }
    public int CompareTo(SoundSource other)
    {
        if (other == null) return 1;

        return priority - other.priority;
    }
}
public enum SoundTypes { MUSIC, AMBIENT, SOUND}
public enum SoundChannels { MUSIC = 0, AMBIENT = 1, LOOP_FX = 2, OneShot_FX = 3, VisiblePositionalSound = 4 }
public class SoundMgr : MonoBehaviour
{
    
    private AudioListener _listener = null;
    private bool configure = false;
    private Dictionary<string, SoundData> _soundDic;
    private Dictionary<SoundChannels, OrderList<SoundSource>> _soundPlaying;
    private SoundSource _music;
    private AudioSource _musicAS;
    private SoundSource _ambient;
    private AudioSource _ambientAS;
    private Dictionary<string, SoundSource> _soundPlayingDictionary;
    private SoundResources _soundResources;
    private List<AudioSource> _soundSourcesGOs;
    private bool _mute = false;
    private bool _musicPaused = false;
    private bool _ambientPaused = false;
    private float _soundVolume = 1f;
    private float _musicVolume = 1f;
    private float _ambientVolume = 1f;
    List<string> keysToDelete = new List<string>();
    private bool _pauseMenu;


    public bool PauseMenu
    {
        get
        {
            return _pauseMenu;
        }

        set
        {
            _pauseMenu = value;
            ManageLoopSound();
        }
    }


    public void Configure(SoundResources soundResources)
    {
        Listener = this.gameObject.AddComponent<AudioListener>();
        _soundDic = new Dictionary<string, SoundData>();
        _soundResources = soundResources;
        _soundPlaying = new Dictionary<SoundChannels, OrderList<SoundSource>>();

        _soundPlaying.Add(SoundChannels.LOOP_FX, new OrderList<SoundSource>());
        _soundPlaying.Add(SoundChannels.OneShot_FX, new OrderList<SoundSource>());
        _soundPlaying.Add(SoundChannels.VisiblePositionalSound, new OrderList<SoundSource>());
        _soundPlayingDictionary = new Dictionary<string, SoundSource>();
        MusicResourceInfo[] sounds = soundResources.musicResourceInfo;
        foreach (MusicResourceInfo sound in sounds)
        {
            SoundData soundData = new SoundData(sound);
            if (!_soundDic.ContainsKey(soundData.Name))
                _soundDic.Add(soundData.Name, soundData);
            else _soundDic[soundData.Name] = soundData;
        }
        _soundSourcesGOs = new List<AudioSource>();
        _musicAS = CreateAditionalAudioSources("MusicAudioResurce");
        _ambientAS = CreateAditionalAudioSources("AmbientAudioResurce");
        for (int i = 0; i < _soundResources.maxSounds; ++i)
        {
            AddSoundGameObject();
        }

        _music = null;
        _ambient = null;
        configure = true;
        /*Source = this.gameObject.AddComponent<AudioSource>();

        soundSourcesGOs = new List<AudioSource>();
        currentSounds = new Dictionary<string, AudioSource>();

        for (int i = 0; i < _maxSounds; ++i)
        {
            GameObject go = new GameObject();
            go.transform.parent = this.transform;
            AudioSource a = go.AddComponent<AudioSource>();
            soundSourcesGOs.Add(a);

        }
        _audioPrefab = soundResources.audioPrefab;

        */

    }

    /*public float MusicVolume
    {
        get { return _music != null ? _music.AudioSource.volume : 0f; }
        set { if (_music != null) _music.AudioSource.volume = value; }
    }

    public float AmbientVolume
    {
        get { return _ambient != null ? _ambient.AudioSource.volume : 0f; }
        set { if (_ambient != null) _ambient.AudioSource.volume = value; }
    }*/

    public void PauseMusic()
    {
        if (_music == null)
            return;
        if (_music.AudioSource.isPlaying && !_musicPaused)
            PauseMusic(true);
        else
            PauseMusic(false);
    }

    public void PauseMusic(bool pause)
    {
        if (_music == null)
            return;
        if (pause)
            _music.AudioSource.Pause();
        else
            _music.AudioSource.Play();
        _musicPaused = pause;
    }

    public void PauseAmbient()
    {
        if (_ambient == null)
            return;
        if (_ambient.AudioSource.isPlaying && !_ambientPaused)
            PauseAmbient(true);
        else
            PauseAmbient(false);
    }

    public void PauseAmbient(bool pause)
    {
        if (_ambient == null)
            return;
        if (pause)
            _ambient.AudioSource.Pause();
        else
            _ambient.AudioSource.Play();
        _ambientPaused = pause;
    }

    public bool IsAmbientPaused
    {
        get
        {
            return _ambientPaused;
        }
    }

    public bool IsMusicPausePaused
    {
        get
        {
            return _musicPaused;
        }
    }

    public float SoundVolume
    {
        get { return _soundVolume; }
        set
        {
            float oldVolumne = _soundVolume;
            _soundVolume = value;
            OnSoundVolumenChange(oldVolumne);
        }
    }

    public float MusicVolume
    { //_musicVolume
        get { return _musicVolume; }
        set
        {
            _musicVolume = value;
            if (_music != null)
            {
                _music.MaxVolume = _soundDic[_music.ClipName].Volume;
                _music.AudioSource.volume = GetMusicVolumen(_music);
            }
                
        }
    }

    //_ambientVolumen
    public float AmbientVolume
    { //_musicVolume
        get { return _ambientVolume; }
        set
        {
            _ambientVolume = value;
            if (_ambient != null)
            {
                _ambient.MaxVolume = _soundDic[_ambient.ClipName].Volume;
                _ambient.AudioSource.volume = GetAmbientVolumen(_ambient);
            }
        }
    }

    public float AmbientSourceVolumen
    {
        get
        {
            return _soundDic[_ambient.ClipName].Volume;
        }
        set
        {
            _soundDic[_ambient.ClipName].Volume = value;
            _ambient.MaxVolume = _soundDic[_ambient.ClipName].Volume;
            _ambient.AudioSource.volume = GetAmbientVolumen(_ambient);
        }

    }

    public float MusicSourceVolumen
    {
        get
        {
            return _soundDic[_music.ClipName].Volume;
        }
        set
        {
            _soundDic[_music.ClipName].Volume = value;
            _music.MaxVolume = _soundDic[_ambient.ClipName].Volume;
            _music.AudioSource.volume = GetAmbientVolumen(_ambient);
        }

    }


    public static string GetRandomSound(List<string> soundList, ref string lastSound, ref int soundTimes, int maxTimes = 3)
    {
        string soundSelected = "";

        if (soundTimes == maxTimes)
        {
            //Sacamos provisionalmente de la lista el sonido ya repetido maxTimes veces
            soundList.Remove(lastSound);
            soundSelected = soundList[UnityEngine.Random.Range(0, soundList.Count)];
            soundList.Add(lastSound);
        }
        else
            soundSelected = soundList[UnityEngine.Random.Range(0, soundList.Count)];

        //Debug.Log("RANDOM SOUND: " + soundSelected);

        //Gestionamos las veces que se repite el último sonido.
        if (soundSelected == lastSound)
            soundTimes++;
        else
        {
            lastSound = soundSelected;
            soundTimes = 1;
        }

        return soundSelected;
    }
    protected AudioListener Listener
    {
        get
        {
            return _listener;
        }
        set
        {
            _listener = value;
        }
    }

    public bool IsSoundPlaying(List<string> audioName)
    {
        if (_soundPlayingDictionary == null || _soundPlayingDictionary.Count == 0 || audioName == null)
            return false;
        bool b = false;
        int i = 0;
        while (i < audioName.Count && !b)
        {
            b |= _soundPlayingDictionary.ContainsKey(audioName[i]);
            i++;
        }

        return b;
    }
    public bool IsSoundPlaying(string[] audioName)
    {
        if (_soundPlayingDictionary == null || _soundPlayingDictionary.Count == 0 || audioName == null)
            return false;
        bool b = false;
        int i = 0;
        while (i < audioName.Length && !b)
        {
            b |= _soundPlayingDictionary.ContainsKey(audioName[i]);
            i++;
        }

        return b;
    }
    public bool IsSoundPlaying(string audioName)
    {
        if (_soundPlayingDictionary == null || _soundPlayingDictionary.Count == 0 || audioName == null)
            return false;
        return _soundPlayingDictionary.ContainsKey(audioName);
    }

    public bool CanSoundPlayMoreTimes(SoundChannels channel, string audioName, int maxTimes = 0)
    {
        if (audioName == null)
            return false;
        if (_soundPlayingDictionary == null || _soundPlayingDictionary.Count == 0)
            return true;

        if (maxTimes <= 0) // El 0 sirve para marcar la opción SIN LÍMITE
            return true;

        if (_soundPlaying[channel] == null || _soundPlaying[channel].Count == 0)
            return true;

        int currentTimes = 0;

        foreach(SoundSource sound in _soundPlaying[channel].GetElements())
        {
            if(sound.ClipName == audioName)
                ++currentTimes;
        }
        //Debug.Log("CANSOUND current times? " + currentTimes);
        return currentTimes < maxTimes; 
    }


    public void PlayMusic(string audioClipName, float fadeIn = 0.0f, bool isAmbience = false)
    {
        if (!_soundDic.ContainsKey(audioClipName))
        {
            Debug.LogError(audioClipName + " no ha sido encontrado en el diccionario de sonidos ");
            return;
        }
            
        if (!IsSoundPlaying(audioClipName))
        {
            StopMusic(0, isAmbience);
            AudioSource audioSource = isAmbience ? _ambientAS : _musicAS;

            audioSource.mute = _mute;
            SoundSource SS = new SoundSource(audioClipName,audioSource, isAmbience ? SoundChannels.AMBIENT : SoundChannels.MUSIC, (isAmbience ? _ambientVolume : _musicVolume) * _soundDic[audioClipName].Volume,0);
            _soundPlayingDictionary.Add(audioClipName, SS);

            if (isAmbience)
                _ambient = SS;
            else
                _music = SS;
            //audioSource.volume = SoundVolume;
            if (LoadClip(audioClipName))
            {
                audioSource.clip = _soundDic[audioClipName].Clip;
                audioSource.clip.name = _soundDic[audioClipName].Name;
                audioSource.loop = _soundDic[audioClipName].Loop;
                audioSource.volume = isAmbience? GetAmbientVolumen(SS) : GetSoundVolumen(SS);
                audioSource.Play();
                Debug.Log("PLAY MUSIC: " + audioClipName);
                if (fadeIn > 0.0f)
                {
                    Coroutine c = this.StartCoroutine(FadeIn(SS,audioSource, fadeIn, isAmbience ? SoundTypes.AMBIENT : SoundTypes.MUSIC));
                    SS.Coroutine = c;
                }
            }
        }
    }

    public void TurnUpMusicAmbient(float grades)
    {
        MusicVolume += grades;
        AmbientVolume += grades;
    }

    public void TurnDownMusicAmbient(float grades)
    {
        MusicVolume -= grades;
        AmbientVolume -= grades;
    }

    public bool IsMute
    {
        get { return _mute; }
    }
    /// <summary>
    /// Deja en silencio la aplicacion (pero no dejan de reproducirse)
    /// </summary>
    public void Mute()
    {
        _mute = true;
        ChangeMute();
    }

    /// <summary>
    /// Vuelve a poner el volumen al sonido 
    /// </summary>
    public void Unmute()
    {
        _mute = false;
        ChangeMute();
    }

    private float GetMusicVolumen(SoundSource SS)
    {
        return _musicVolume * SS.MaxVolume;
    }

    private float GetSoundVolumen(SoundSource SS)
    {
        return _soundVolume * SS.MaxVolume;
    }

    private float GetAmbientVolumen(SoundSource SS)
    {
        return _ambientVolume * SS.MaxVolume;
    }

    private void OnSoundVolumenChange(float old)
    {
        if(_soundPlaying[SoundChannels.LOOP_FX].Count > 0)
        {
            foreach (SoundSource sources in _soundPlaying[SoundChannels.LOOP_FX].GetElements())
            {
                float dif = Mathf.Abs(sources.AudioSource.volume - old);
                if (dif < 0.01f)
                {
                    sources.MaxVolume =  _soundDic[sources.ClipName].Volume;
                    sources.AudioSource.volume = GetSoundVolumen(sources);
                }
            }
        }

        if (_soundPlaying[SoundChannels.OneShot_FX].Count > 0)
        {
            foreach (SoundSource sources in _soundPlaying[SoundChannels.OneShot_FX].GetElements())
            {
                float dif = Mathf.Abs(sources.AudioSource.volume - old);
                if (dif < 0.01f)
                {
                    sources.MaxVolume = _soundDic[sources.ClipName].Volume;
                    sources.AudioSource.volume = GetSoundVolumen(sources);
                }
            }
        }

        if (_soundPlaying[SoundChannels.VisiblePositionalSound].Count > 0)
        {
            foreach (SoundSource sources in _soundPlaying[SoundChannels.VisiblePositionalSound].GetElements())
            {
                float dif = Mathf.Abs(sources.AudioSource.volume - old);
                if (dif < 0.01f)
                {
                    sources.MaxVolume =  _soundDic[sources.ClipName].Volume;
                    sources.AudioSource.volume = GetSoundVolumen(sources);
                }
            }
        }

    }

    private void ChangeMute()
    {
        if (_music != null)
            _music.AudioSource.mute = _mute;
        if (_ambient != null)
            _ambient.AudioSource.mute = _mute;


        if (_soundPlaying[SoundChannels.LOOP_FX].Count > 0)
        {
            foreach (SoundSource sources in _soundPlaying[SoundChannels.LOOP_FX].GetElements())
            {
                sources.AudioSource.mute = _mute;
            }
        }

        if (_soundPlaying[SoundChannels.OneShot_FX].Count > 0)
        {
            foreach (SoundSource sources in _soundPlaying[SoundChannels.OneShot_FX].GetElements())
            {
                sources.AudioSource.mute = _mute;
            }
        }


        if (_soundPlaying[SoundChannels.VisiblePositionalSound].Count > 0)
        {
            foreach (SoundSource sources in _soundPlaying[SoundChannels.VisiblePositionalSound].GetElements())
            {
                sources.AudioSource.mute = _mute;
            }
        }

    }
    public void PlayLoopSoundPositional(string audioClipName, float maxDistance, float distanceToMaxVolumen, float minPan, GameObject pos = null, int priority = 0)
    {
        if (_soundPlayingDictionary.ContainsKey(audioClipName))
        {
            SoundSource soundSource = _soundPlayingDictionary[audioClipName];
            if (soundSource.SoundChannel == SoundChannels.VisiblePositionalSound)
            {
                if (soundSource.SoundPosition == null || soundSource.SoundPosition.GetGameObjectList().Count == 0)
                    soundSource.SoundPosition = new SoundPosition(maxDistance, distanceToMaxVolumen, minPan);
                //ignoramos la invocación al método si ya tenemos un sonido sonando y volvemos a intentar establecer una configuración dle mismo.
                if (pos != null)
                    soundSource.SoundPosition.AddPosition(pos);
            }
            else
                Debug.LogError("El sonido ya está sonando en otro canal, no podemos asignarlo al canal de audio posicional");
        }
        else
        {
            AudioSource audioSource = GetAudioSource();
            audioSource.mute = _mute;
            SoundSource soundSource = new SoundSource(audioClipName,audioSource, SoundChannels.VisiblePositionalSound, _soundDic[audioClipName].Volume, priority);
            soundSource.SoundPosition = new SoundPosition(maxDistance, distanceToMaxVolumen, minPan);
            _soundPlayingDictionary.Add(audioClipName, soundSource);
            
            if (LoadClip(audioClipName))
            {
                if (_soundDic[audioClipName].Loop)
                {
                    audioSource.clip = _soundDic[audioClipName].Clip;
                    audioSource.clip.name = _soundDic[audioClipName].Name;
                    audioSource.loop = _soundDic[audioClipName].Loop;
                    audioSource.volume = GetSoundVolumen(soundSource);
                }
                else
                    Debug.LogError("Este canal es para sonidos en loop, usa la función PlaySound para reproducir este archivo: " + audioClipName);
                OrderList<SoundSource> ol = _soundPlaying[SoundChannels.VisiblePositionalSound];
                ol.InsertInOrder(soundSource);
            }
            if (pos != null)
                soundSource.SoundPosition.AddPosition(pos);
        }
    }

    public void AddsourcePositionToSound(string audioClipName, GameObject t)
    {
        if (_soundPlayingDictionary.ContainsKey(audioClipName))
        {
            SoundSource soundSource = _soundPlayingDictionary[audioClipName];
            if (soundSource.SoundChannel == SoundChannels.VisiblePositionalSound)
            {
                if (soundSource.SoundPosition != null && soundSource.SoundPosition.GetGameObjectList().Count >= 0)
                {
                    if (t != null)
                        soundSource.SoundPosition.AddPosition(t);
                }
            }
            else
                Debug.LogError("El sonido ya está sonando en otro canal, no podemos asignarlo al canal de audio posicional");
        }
    }

    public bool IsLoop(string audioClipName)
    {
        bool result = _soundPlayingDictionary.ContainsKey(audioClipName) && _soundPlayingDictionary[audioClipName].AudioSource.loop;

        if (!result)
            Debug.Log("Not valid audioClip: " + audioClipName);
        return result;
    }

    public bool IsLoopFromSoundSource(string audioClipName)
    {
        bool result = false;
        if (_soundDic.ContainsKey(audioClipName))
        {
            result = _soundDic[audioClipName].Loop;
        }
        else
        {
            Debug.Log("Not valid audioClip: " + audioClipName);
        }
        return result;
    }

    public float SoundLenght(string audioClipName)
    {
        float result = 0f;
        if (_soundDic.ContainsKey(audioClipName))
        {
            result = _soundDic[audioClipName].Clip.length;
        }
        else
        {
            Debug.Log("Not valid audioClip: " + audioClipName);
        }
        return result;
    }

    public void StopMusic(float fadeOut = 0.0f, bool isAmbience = false)
    {

        SoundSource ss = isAmbience ? _ambient : _music;

        if (ss != null)
        {
            Debug.Log("STOP MUSIC SS = " + ss.ClipName);
            if (ss.Coroutine != null)
            {
                StopCoroutine(ss.Coroutine);
            }

            AudioSource a = ss.AudioSource;
            if (fadeOut > 0.0f)
            {
                Coroutine c = this.StartCoroutine(FadeOut(ss,a, fadeOut,SoundTypes.MUSIC));
                ss.Coroutine = c;
            }
            else
            {
                StopAudioSource(ss.ClipName,a,true);
                if (isAmbience) _ambient = null;
                else _music = null;
            }
        }
    }

    /*public void StopMusic(string audioClip, float fadeOut = 0.0f)
    {

        if (IsSoundPlaying(audioClip))
        {
            SoundSource ss = _soundPlayingDictionary[audioClip];
            Debug.Log("STOP MUSIC SS = " + ss.ClipName);
            if (ss.Coroutine != null)
            {
                StopCoroutine(ss.Coroutine);
            }

            AudioSource a = ss.AudioSource;
            if (fadeOut > 0.0f)
            {
                Coroutine c = this.StartCoroutine(FadeOut(ss, a, fadeOut,true));
                ss.Coroutine = c;
            }
            else
            {
                StopAudioSource(ss.ClipName, a,true);
                if (ss == _ambient) _ambient = null;
                else if (ss == _music) _music = null;
            }
        }
    }*/

    public void PauseAllSounds()
    {
        OrderList<SoundSource> loopFx = _soundPlaying[SoundChannels.LOOP_FX];
        OrderList<SoundSource> positionalSound = _soundPlaying[SoundChannels.VisiblePositionalSound];
        if (loopFx.Count > 0)
            foreach (SoundSource ss in loopFx.GetElements())
            {
                ss.Pause();
            }
        
        if (positionalSound.Count > 0)
            foreach (SoundSource ss in positionalSound.GetElements())
            {
                ss.Pause();
            }
    }

    public void ResumeAllSounds()
    {
        OrderList<SoundSource> loopFx = _soundPlaying[SoundChannels.LOOP_FX];
        OrderList<SoundSource> positionalSound = _soundPlaying[SoundChannels.VisiblePositionalSound];
        if (loopFx.Count > 0)
            foreach (SoundSource ss in loopFx.GetElements())
            {
                ss.Resume();
            }

        if (positionalSound.Count > 0)
            foreach (SoundSource ss in positionalSound.GetElements())
            {
                ss.Resume();
            }
    }

    public void StopAllSounds()
    {
        OrderList<SoundSource> loopFx = _soundPlaying[SoundChannels.LOOP_FX];
        OrderList<SoundSource> oneshotFx = _soundPlaying[SoundChannels.OneShot_FX];
        OrderList<SoundSource> positionalSound = _soundPlaying[SoundChannels.VisiblePositionalSound];
        if(loopFx.Count > 0)
            foreach (SoundSource ss in loopFx.GetElements())
            {
                ss.Stop(this);
            }
        if (oneshotFx.Count > 0)
            foreach (SoundSource ss in oneshotFx.GetElements())
            {
                ss.Stop(this);
            }
        if (positionalSound.Count > 0)
            foreach (SoundSource ss in positionalSound.GetElements())
            {
                ss.Stop(this);
            }
        loopFx.Clear();
        oneshotFx.Clear();
        positionalSound.Clear();
    }

    public void StopSound(string audioClip, GameObject t, float fadeOut = 0.0f)
    {
        if (t == null)
            StopSound(audioClip, fadeOut);
        else
        {
            if (IsSoundPlaying(audioClip))
            {
                SoundSource ss = _soundPlayingDictionary[audioClip];
                if (ss.SoundPosition != null && ss.SoundPosition.GetGameObjectList().Count > 0)
                {
                    ss.SoundPosition.GetGameObjectList().Remove(t);
                    if (ss.SoundPosition.GetGameObjectList().Count == 0)
                    {
                        StopSound(audioClip, fadeOut);
                    }
                }
            }
        }
    }
    public void StopSound(string clipName, float fadeOut = 0.0f)
    {
        if (IsSoundPlaying(clipName))
        {
            Debug.Log("STOP SOUND: " + clipName);
            SoundSource ss = _soundPlayingDictionary[clipName];
            if (ss != null)
            {
                if (ss.Coroutine != null)
                {
                    StopCoroutine(ss.Coroutine);
                }
                AudioSource a = ss.AudioSource;
                if (fadeOut > 0.0f)
                {
                    this.StartCoroutine(FadeOut(ss,a, fadeOut,SoundTypes.SOUND));
                }
                else
                {
                    StopAudioSource(clipName,a,false);
                }
            }
        }
    }

    // Por defecto la prioridad es baja, cuanto más alta más lejos estará en la cola de prioridad y mas improbable es que 
    // se desactive con un nuevo sonido.
    public void PlaySound(string audioClipName, int priority = 0)
    {
        if (!SanityCheck(audioClipName))
            return;
        SoundData sd = _soundDic[audioClipName];
        
        SoundChannels channel = sd.Loop ? SoundChannels.LOOP_FX : SoundChannels.OneShot_FX;
        if (!IsSoundPlaying(audioClipName) || channel == SoundChannels.OneShot_FX)
        {
            //Debug.Log("PlaySound " + audioClipName);
            AudioSource audioSource = GetAudioSource();
            audioSource.mute = _mute;
            OrderList<SoundSource> ol = _soundPlaying[channel];
            if (ol.Count >= MaxFX(sd.Loop))
            {
                if(ol.Count > 0)
                {
                    SoundSource ssToDelete = ol.ExtractFirst();
                    if (ssToDelete != null)
                        ssToDelete.Stop(this);
                }
            }

            SoundSource ss = new SoundSource(audioClipName,audioSource, channel, _soundDic[audioClipName].Volume, priority);
            if(channel == SoundChannels.OneShot_FX && !_soundPlayingDictionary.ContainsKey(audioClipName))
                _soundPlayingDictionary.Add(audioClipName, ss);
            else if(channel == SoundChannels.LOOP_FX)
                _soundPlayingDictionary.Add(audioClipName, ss);
            ol.InsertInOrder(ss);
            //audioSource.volume = SoundVolume;
            if (LoadClip(audioClipName))
            {
                ConfigureAudioSource(audioSource, _soundDic[audioClipName].Clip, _soundDic[audioClipName].Name, 
                    _soundDic[audioClipName].Loop, GetSoundVolumen(ss), 0f);
                /*audioSource.clip = _soundDic[audioClipName].Clip;
                audioSource.clip.name = _soundDic[audioClipName].Name;
                audioSource.loop = _soundDic[audioClipName].Loop;
                audioSource.volume = ss.MaxVolume;
                audioSource.panStereo = 0f;*/
                if (audioSource.loop)
                    audioSource.Play();
                else
                    audioSource.PlayOneShot(audioSource.clip);
            }
        }
    }

    public void PlayOneShotPositional(string audioClipName, Transform transform, float maxDistance, float distanceToMaxVolumen, float minPan, int priority = 0)
    {
        PlayOneShotPositional(audioClipName, transform.position, maxDistance, distanceToMaxVolumen,minPan, priority);
    }
    public void PlayOneShotPositional(string audioClipName, Vector3 position, float maxDistance, float distanceToMaxVolumen, float minPan, int priority = 0)
    {
        if (!SanityCheck(audioClipName))
            return;
        BaseCamera2D camera = BaseCamera2D.Get();
        if (camera == null)
        {
            // si no tenemos cámara, no deberiamos reproducir ninguno de los sonidos
            return;
        }
        // generalizar en un método para no repetir código.
        OrderList<SoundSource> ol = _soundPlaying[SoundChannels.OneShot_FX];
        if (ol.Count >= MaxFX(false))
        {
            if (ol.Count > 0)
            {
                SoundSource ssToDelete = ol.ExtractFirst();
                if (ssToDelete != null)
                    ssToDelete.Stop(this);
            }
        }
        AudioSource audioSource = GetAudioSource();

        if (LoadClip(audioClipName))
        {
            SoundData sd = _soundDic[audioClipName];
            if (sd.Loop)
            {
                Debug.LogError("Estas reproduciendo un sonido marcado como loop en como sonido One shot posicional, deberias reproducirlo como sonido loop posicional " + audioClipName);
            }
            
            float range =  maxDistance - distanceToMaxVolumen;
            float distance = 0f;
            float panStereo = Pan(distanceToMaxVolumen, minPan, position, camera, range, position.x < camera.transform.position.x ? -1 : +1, out distance);
            if (distance < maxDistance)
            {
                float vol = _soundVolume * sd.Volume;
                if (distance > distanceToMaxVolumen)
                {
                    float moreThanMaxVol = distance - distanceToMaxVolumen;
                    vol = vol * (range - moreThanMaxVol) / range;
                }
                ConfigureAudioSource(audioSource, sd.Clip, sd.Name, false, vol, panStereo);
                audioSource.PlayOneShot(audioSource.clip);
                SoundSource ss = new SoundSource(audioClipName, audioSource, SoundChannels.OneShot_FX, vol, priority);
                if (!_soundPlayingDictionary.ContainsKey(audioClipName))
                    _soundPlayingDictionary.Add(audioClipName, ss);
                ol.InsertInOrder(ss);
            }
                
        }
    }

    public bool SanityCheck(string audioClipName)
    {
        if (audioClipName == null || audioClipName == "")
            return false;

        if (!_soundDic.ContainsKey(audioClipName))
        {
            Debug.LogError("El sonido " + audioClipName + " no ha sido encontrado en el diccionario");
            return false;
        }
        return true;
    }

    public void ConfigureAudioSource(AudioSource audioSource, AudioClip audioClip,string name, bool loop, float volumen, float pan)
    {
        audioSource.clip = audioClip;
        audioSource.clip.name = name;
        audioSource.loop = loop;
        audioSource.volume = volumen;
        audioSource.panStereo = pan;
    }

    protected int MaxFX(bool isLoop)
    {
        return isLoop ? _soundResources.maxLoopFX : _soundResources.maxFX;
    }

    //Precarga resources de audio al cargar la escena lanzadora, se debe invocar con un mono behavior en cada escena lanzadora
    public void PreloadAudioResources(string[] arrNames)
    {
        for (int i = 0; i < arrNames.Length; ++i)
        {
            LoadClip(arrNames[i]);
        }
    }

    //nos permite limpiar la cache de audio. Podrian seguir sonando los audios que esten en funcionamiento, por lo que esto
    //en principio solo tiene sentido que se haga por ejemplo al cambiar de nivel (al volver al menu de selección de niveles)
    public void ClearDictionary()
    {
        _soundDic.Clear();
    }

    protected bool LoadClip(string audioClipName)
    {
        bool result = false;
        if (_soundDic.ContainsKey(audioClipName))
        {
            if (_soundDic[audioClipName].Clip == null)
            {
                _soundDic[audioClipName].Clip = Resources.Load(_soundDic[audioClipName].FileName) as AudioClip;
            }
            result = _soundDic[audioClipName].Clip != null;
            if (!result)
                Debug.Log("Can not load: " + _soundDic[audioClipName].FileName);
        }

        return result;

    }

    protected AudioSource GetAudioSource()
    {
        if (_soundSourcesGOs.Count == 0)
            AddSoundGameObject();
        AudioSource audioSource = _soundSourcesGOs[0];
        _soundSourcesGOs.RemoveAt(0);
        return audioSource;
    }

    protected void AddSoundGameObject()
    {
        GameObject go = new GameObject();
        go.transform.parent = this.transform;
        go.name = "FXAudioResources";
        AudioSource a = go.AddComponent<AudioSource>();
        _soundSourcesGOs.Add(a);
    }

    protected AudioSource CreateAditionalAudioSources(string goName)
    {
        GameObject go = new GameObject();
        go.transform.parent = this.transform;
        go.name = goName;
        AudioSource a = go.AddComponent<AudioSource>();
        return a;
    }
    // Start is called before the first frame update

    public float GetVolumen(SoundTypes type, SoundSource soundSource)
    {
        if (type == SoundTypes.MUSIC)
            return GetMusicVolumen(soundSource);
        else if (type == SoundTypes.AMBIENT)
            return GetAmbientVolumen(soundSource);
        else 
            return GetSoundVolumen(soundSource);
    }
    private IEnumerator FadeIn(SoundSource soundSource, AudioSource audioSource, float time, SoundTypes type)
    {
        //Se preparan las variables
        float previousTime = Time.realtimeSinceStartup;
        float elapsedTime = 0;
        var vol = audioSource.volume;
        float accumTime = 0;
        float maxSound = GetVolumen(type, soundSource);
        //Iteramos
        do
        {
            elapsedTime = Time.realtimeSinceStartup - previousTime;
            previousTime = Time.realtimeSinceStartup;
            accumTime += elapsedTime;
            float percent = accumTime / time;
            
            percent = percent > maxSound ? maxSound : percent;
            audioSource.volume = Mathf.Lerp(0.0f, vol, percent);
            //Debug.Log(this, "VOLUME:" + percent);
            yield return new WaitForEndOfFrame();
        } while ((accumTime < time) && (audioSource.volume < maxSound));
        SoundSource ss = _soundPlayingDictionary[audioSource.name];
        ss.Coroutine = null;

    }

    private IEnumerator FadeOut(SoundSource soundSource, AudioSource audioSource, float time, SoundTypes type)
    {
        //Se preparan las variables
        float previousTime = Time.realtimeSinceStartup;
        float elapsedTime = 0;
        var vol = audioSource.volume;
        float accumTime = 0;

        //Iteramos
        do
        {
            ////elapsedTime = (elapsedTime + Time.deltaTime > time) ? time : elapsedTime + Time.deltaTime;
            ////_source.volume = Mathf.Lerp(vol, 0.0f, (Time.time - startTime) / (endTime - startTime));//_source.volume--;
            elapsedTime = Time.realtimeSinceStartup - previousTime;
            previousTime = Time.realtimeSinceStartup;
            accumTime += elapsedTime;
            float percent = accumTime / time;
            float maxSound = GetVolumen(type, soundSource);
            percent = percent > maxSound ? maxSound : percent;
            audioSource.volume = Mathf.Lerp(vol, 0.0f, percent);
            yield return new WaitForEndOfFrame();
        } while ((accumTime < time) && (audioSource.volume > 0));
        //audioSource.Stop();
        StopAudioSource(soundSource.ClipName, audioSource, type == SoundTypes.MUSIC);
    }

    protected void StopAudioSource(string clipName,AudioSource audioSource, bool isMusic)
    {
        audioSource.Stop();
        if(!isMusic)
            _soundSourcesGOs.Add(audioSource);

        if(_soundPlayingDictionary.ContainsKey(clipName))
        {
            SoundSource ss = _soundPlayingDictionary[clipName];
            ss.Coroutine = null;
            _soundPlayingDictionary.Remove(clipName);
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        keysToDelete.Clear();
        if(_soundPlayingDictionary != null)
        {
            foreach (var pair in _soundPlayingDictionary)
            {
                if (!pair.Value.AudioSource.isPlaying && pair.Value.AudioSource.time == 0 && pair.Value.SoundChannel != SoundChannels.VisiblePositionalSound)
                {
                    keysToDelete.Add(pair.Key);
                }
            }
        }


        for (int i = 0; i < keysToDelete.Count; ++i)
        {
            SoundSource aux = _soundPlayingDictionary[keysToDelete[i]];
            bool restoreTosoundResorceList = false;
            if (aux.SoundChannel == SoundChannels.MUSIC)
            {
                _music = null;
            } 
            else if (aux.SoundChannel == SoundChannels.AMBIENT)
            {
                _ambient = null;
            }
            else
            {
                OrderList<SoundSource> order = _soundPlaying[aux.SoundChannel];
                order.Remove(aux);
                restoreTosoundResorceList = true;
            }
            aux.Stop(this);
            if(restoreTosoundResorceList)
                _soundSourcesGOs.Add(aux.AudioSource);
            _soundPlayingDictionary.Remove(keysToDelete[i]);
        }
        keysToDelete.Clear();

        if(!PauseMenu)
            ManagePositionSounds(Time.deltaTime);
    }

    private void ManageLoopSound()
    {
        if (_soundPlaying.ContainsKey(SoundChannels.LOOP_FX))
        {
            OrderList<SoundSource> list = _soundPlaying[SoundChannels.VisiblePositionalSound];
            var iterator = list.GetIterator();
            while (iterator != null)
            {
                SoundSource soundSource = iterator.Item;
                if (_pauseMenu)
                {
                    soundSource.AudioSource.Pause();
                }
                else
                {
                    soundSource.AudioSource.UnPause();
                }
                iterator = iterator.Next;
            }
        }

        if (_soundPlaying.ContainsKey(SoundChannels.VisiblePositionalSound))
        {
            OrderList<SoundSource> list = _soundPlaying[SoundChannels.VisiblePositionalSound];
            var iterator = list.GetIterator();
            while (iterator != null)
            {
                SoundSource soundSource = iterator.Item;
                if (_pauseMenu)
                {
                    soundSource.AudioSource.Pause();
                }
                else
                {
                    soundSource.AudioSource.UnPause();
                }
                iterator = iterator.Next;
            }
        }
    }

    protected void ManagePositionSounds(float time)
    {
        if (_soundPlaying == null)
            return;


        if(_soundPlaying.ContainsKey(SoundChannels.VisiblePositionalSound))
        {
            OrderList<SoundSource> list = _soundPlaying[SoundChannels.VisiblePositionalSound];
            var iterator = list.GetIterator();
            BaseCamera2D camera = BaseCamera2D.Get();
            if (camera == null)
            {
                // si no tenemos cámara, no deberiamos reproducir ninguno de los sonidos
                return;
            }
            while (iterator != null)
            {
                SoundSource soundSource = iterator.Item;
     
                if (soundSource.SoundPosition != null && soundSource.SoundPosition.GetGameObjectList().Count > 0)
                {
                    Transform nearest = null;
                    float distance = -1f;
                    Transform nearestLeft = NearesLeftObject(camera, soundSource);
                    Transform nearestRight = NearesRightObject(camera, soundSource);
                    float range = (soundSource.SoundPosition.MaxDistance - soundSource.SoundPosition.DistanceToMaxVolumen);
                    if (nearestLeft == null && nearestRight != null)
                    {
                        distance = Pan(soundSource, nearestRight, camera, range, +1f);
                        nearest = nearestRight;
                    }
                    else if (nearestLeft != null && nearestRight == null)
                    {
                        distance = Pan(soundSource, nearestLeft, camera, range, -1f);
                        nearest = nearestLeft;
                    }
                    else if (nearestLeft == null && nearestRight==null)
                    {
                        soundSource.AudioSource.panStereo = 0f;
                    }
                    else
                    {
                        float distanceR = Vector2.Distance(nearestRight.position, camera.transform.position);
                        float distanceL = Vector2.Distance(nearestLeft.position, camera.transform.position);
                        if (distanceR < soundSource.SoundPosition.MaxDistance && distanceL > soundSource.SoundPosition.MaxDistance)
                        {
                            Pan(soundSource, nearestRight, camera, range, +1f);
                        }
                        else if (distanceR > soundSource.SoundPosition.MaxDistance && distanceL < soundSource.SoundPosition.MaxDistance)
                        {
                            Pan(soundSource, nearestLeft, camera, range, -1f);
                        }
                        else
                        {
                            soundSource.AudioSource.panStereo = 0f;
                        }


                        nearest = (distanceR > distanceL) ? nearestLeft : nearestRight;
                        distance = Mathf.Min(distanceR, distanceL);

                    }

                    if (distance < 0 && nearest != null)
                        distance = Vector2.Distance(nearest.position, camera.transform.position);
                    else if (nearest == null)
                        distance = 0f;
                    if (distance < soundSource.SoundPosition.MaxDistance)
                    {
                        if (distance <= soundSource.SoundPosition.DistanceToMaxVolumen)
                            soundSource.AudioSource.volume = GetSoundVolumen(soundSource);
                        else
                        {
                            float moreThanMaxVol = distance - soundSource.SoundPosition.DistanceToMaxVolumen;
                            soundSource.AudioSource.volume = GetSoundVolumen(soundSource) * (range - moreThanMaxVol) / range;
                        }
                        if (!soundSource.AudioSource.isPlaying)
                        {
                            soundSource.AudioSource.Play();
                        }
                    }
                    else
                    {
                        soundSource.AudioSource.volume = 0f;
                        if (soundSource.AudioSource.isPlaying)
                        {
                            soundSource.AudioSource.Stop();
                        }
                    }
                }
                iterator = iterator.Next;
            }
        }

    }

    protected float Pan(float distanceToMaxVolumen, float minPan, Vector3 nearest, BaseCamera2D camera, float range, float sig, out float distance)
    {
        distance = Vector2.Distance(nearest, camera.transform.position);
        float panStereo = 0f;
        if (distance > distanceToMaxVolumen)
        {
            float maxPan = (sig > 0f) ? (sig - minPan) : (sig + minPan);
            if (sig > 0f)
                panStereo = Mathf.Min(maxPan, maxPan * ((distance - distanceToMaxVolumen) / range));
            else
                panStereo = Mathf.Max(maxPan, maxPan * ((distance - distanceToMaxVolumen) / range));
        }
        return panStereo;
    }

    protected float Pan(SoundSource soundSource, Transform nearest, BaseCamera2D camera, float range, float sig)
    {
        return Pan(soundSource, nearest.position, camera, range, sig);
    }
    protected float Pan(SoundSource soundSource, Vector3 nearest, BaseCamera2D camera, float range, float sig)
    {
        float distance = 0f;
        soundSource.AudioSource.panStereo = Pan(soundSource.SoundPosition.DistanceToMaxVolumen, soundSource.SoundPosition.MinPan, nearest, camera, range, sig, out distance);
        return distance;
    }

    protected Transform NearesObject(BaseCamera2D c, SoundSource s, float sig)
    {
        float minDistance = float.PositiveInfinity;
        Transform ret = null;
        if (s.SoundPosition != null)
        {
            List<GameObject> l = s.SoundPosition.GetGameObjectList();
            for (int i = 0; i <l.Count; i++)
            {
                if(l[i] != null)
                {
                    float dif = l[i].transform.position.x - c.transform.position.x;
                    if(Mathf.Sign(dif) == sig)
                    {
                        float dis = Vector2.Distance(c.transform.position, l[i].transform.position);
                        if(dis < minDistance)
                        {
                            ret = l[i].transform;
                            minDistance = dis;
                        }
                    }
                }
            }
        }

        return ret;
    }

    protected Transform NearesLeftObject(BaseCamera2D c, SoundSource s)
    {
        return NearesObject(c,s,-1f);
    }
    protected Transform NearesRightObject(BaseCamera2D c, SoundSource s)
    {
        return NearesObject(c, s, 1f);
    }
}
