using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BaseQualityMgr : MonoBehaviour
{
    private PlatformVariants platformVariant;
    private float musicVolume;
    private float fxVolume;
    private float ambientVolume;
    private bool mute;
    private bool showFPS;
    private bool rumbleEnable;
    private int resolution; // resolución sacadas de Resolutions [] resolutions = Screen.resolutions; hay que ver si filtra por aspect ratio soportado o no.
    private int msaa; // Valid values are 0 (no MSAA), 2, 4, and 8
    private int vsync; // 0 not Vsync, 1 FullVSync support, 2 middle.
    private int qualityLevel;
    private int mirrorReflexionQuality; // resolución de los reflejos. 0 = 256, 1 = 512, 2 = 1024, 3 = 2046
    private int postProcessingAA; //0 none, 1 FXAA, 2 SMAA, 3 TAA
    private int postProcessingAALevel; //depende de el tipo, si es 2 => 0 low, 1 = medium, 2 = high.
    private BaseQualityMgr _default;
    private static readonly string[] postProcessingAALevelsFXAA = { "LOW", "HIGH" };
    private static readonly SubpixelMorphologicalAntialiasing.Quality[] postProcessingAALevelsSMAA = { SubpixelMorphologicalAntialiasing.Quality.Low,
        SubpixelMorphologicalAntialiasing.Quality.Medium, SubpixelMorphologicalAntialiasing.Quality.High };
    private static readonly string[] postProcessingAALevelsSMAAStr = { "Low", "Medium", "High" };
    private static readonly int[] mirrorReflexionQualityLevels = { 256,512,1024,2048 };
    private static readonly string[] VSyncLabels = { "None", "Full", "Half"};
    private static readonly string[] MSAALabels = { "No MSAA", "MSAAx2", "MSAAx4", "MSAAx8" };
    private static readonly string[] Mirrors = { "Low", "Medium", "High", "Ultra" };
    private static readonly int[] MSAALevels = { 0, 2, 4, 8 };

    public float MusicVolume { get => musicVolume; set => musicVolume = value; }
    public float AmbientVolume { get => ambientVolume; set => ambientVolume = value; }
    public bool Mute { get => mute; set => mute = value; }
    public bool ShowFPS { get => showFPS; set => showFPS = value; }

    public bool RumbleEnable { get => rumbleEnable; set => rumbleEnable = value; }
    public int Resolution { get => resolution; set => resolution = value; }
    public int Msaa { get => msaa; set => msaa = value; }
    public int Vsync { get => vsync; set => vsync = value; }
    public int QualityLevel { get => qualityLevel; set => qualityLevel = value; }
    public int MirrorReflexionQuality { get => mirrorReflexionQuality; set => mirrorReflexionQuality = value; }
    public float FxVolume { get => fxVolume; set => fxVolume = value; }
    public int PostProcessingAA { get => postProcessingAA; set => postProcessingAA = value; }
    public int PostProcessingAALevel { get => postProcessingAALevel; set => postProcessingAALevel = value; }
    public PlatformVariants PlatformVariant { get => platformVariant; set => platformVariant = value; }

    /// <summary>
    /// Nos permite indicarle al servidor si usamos los settings almacenados o por defecto creados por código o los elegidos desde el editor.
    /// </summary>
    public bool useQS;

    public int CPU_POWER
    {
        get
        {
            if (platformVariant == null || platformVariant.CPU_POWER < 0)
            {
                int speed = SystemInfo.processorFrequency;
                int count = SystemInfo.processorCount;
                int power = 1;
                if (count > 3 && count < 10)
                    power = 2;
                else if (count > 10)
                    power = 3;


                if (speed > 1500 && speed <= 2500)
                    power += 1;
                else if (speed > 2500 && speed <= 3500)
                    power += 2;
                else if (speed > 3500)
                    power += 3;

                platformVariant.CPU_POWER = power;

            }

            return platformVariant.CPU_POWER;
        }
    }
    //si tipo es FXAA, 0 es low, 1 es high.
    //string[] names = QualitySettings.names;
    public void LoadSettingsEditorMode()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        fxVolume = PlayerPrefs.GetFloat("FXVolume");
        ambientVolume = PlayerPrefs.GetFloat("AmbientVolume");
        mute = _default.mute;
        qualityLevel = _default.qualityLevel;
        showFPS = _default.showFPS;
        rumbleEnable = PlayerPrefs.GetInt("RumbleEnable") == 0 ? false : true;
        msaa = _default.Msaa;
        vsync = _default.Vsync;
        resolution = _default.Resolution;
        mirrorReflexionQuality = PlayerPrefs.GetInt("MirrorReflexionQuality");
        postProcessingAA = _default.PostProcessingAA;
        postProcessingAALevel = _default.PostProcessingAALevel;
    }
    public void LoadSettings()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        fxVolume = PlayerPrefs.GetFloat("FXVolume");
        ambientVolume = PlayerPrefs.GetFloat("AmbientVolume");
        mute = PlayerPrefs.GetInt("Mute") == 0? false : true ;
        qualityLevel = PlayerPrefs.GetInt("QualityLevel");
        showFPS = PlayerPrefs.GetInt("ShowFPS") == 0 ? false : true;
        rumbleEnable = PlayerPrefs.GetInt("RumbleEnable") == 0 ? false : true;
        msaa = PlayerPrefs.GetInt("MSAA");
        vsync = PlayerPrefs.GetInt("Vsync");
        resolution = PlayerPrefs.GetInt("Resolution");
        mirrorReflexionQuality = PlayerPrefs.GetInt("MirrorReflexionQuality");
        postProcessingAA = PlayerPrefs.GetInt("PostProcessingAA");
        postProcessingAALevel = PlayerPrefs.GetInt("PostProcessingLevel");
    }

    public void Save()
    {
        //no queremos guardar en el editor para tener siempre la configuración por referencia
#if UNITY_EDITOR
        Debug.LogWarning("No guardamos settings de configuración en el editor para mantener siempre la configuración establecida, sólo guardamos aquellos que no están en el editor");
        _SaveInEditorMode();
#else
        _Save();
#endif
    }

    protected void _SaveInEditorMode()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("FXVolume", fxVolume);
        PlayerPrefs.SetFloat("AmbientVolume", ambientVolume);
        PlayerPrefs.SetInt("Mute", PlayerPrefs.GetInt("Mute"));
        PlayerPrefs.SetInt("RumbleEnable", rumbleEnable ? 1 : 0);
        PlayerPrefs.SetInt("QualityLevel", PlayerPrefs.GetInt("QualityLevel"));
        PlayerPrefs.SetInt("ShowFPS", PlayerPrefs.GetInt("ShowFPS"));
        PlayerPrefs.SetInt("MSAA", PlayerPrefs.GetInt("MSAA"));
        PlayerPrefs.SetInt("Vsync", PlayerPrefs.GetInt("Vsync"));

        PlayerPrefs.SetInt("Resolution", PlayerPrefs.GetInt("Resolution"));

        PlayerPrefs.SetInt("MirrorReflexionQuality", mirrorReflexionQuality);
        PlayerPrefs.SetInt("PostProcessingAA", PlayerPrefs.GetInt("PostProcessingAA"));
        PlayerPrefs.SetInt("PostProcessingLevel", PlayerPrefs.GetInt("PostProcessingLevel"));
    }
    protected void _Save()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("FXVolume", fxVolume);
        PlayerPrefs.SetFloat("AmbientVolume", ambientVolume);
        PlayerPrefs.SetInt("Mute", mute ? 1 : 0);
        PlayerPrefs.SetInt("RumbleEnable", rumbleEnable ? 1 : 0);
        PlayerPrefs.SetInt("QualityLevel", qualityLevel);
        PlayerPrefs.SetInt("ShowFPS", showFPS ? 1 : 0);
        PlayerPrefs.SetInt("MSAA", msaa);
        PlayerPrefs.SetInt("Vsync", vsync);

        PlayerPrefs.SetInt("Resolution", resolution);

        PlayerPrefs.SetInt("MirrorReflexionQuality", mirrorReflexionQuality);
        PlayerPrefs.SetInt("PostProcessingAA", postProcessingAA);
        PlayerPrefs.SetInt("PostProcessingLevel", postProcessingAALevel);
    }

    public bool IsSaveAnySettingConfiguration()
    {
        return PlayerPrefs.HasKey("MusicVolume");
    }

    public static BaseQualityMgr DefaultValues()
    {
        BaseQualityMgr qm = new BaseQualityMgr();
        qm.musicVolume = 1f;
        qm.fxVolume = 1f;
        qm.ambientVolume = 1f;
        qm.mute = false;
        qm.rumbleEnable = true;
        qm.qualityLevel = QualitySettings.GetQualityLevel();
        qm.showFPS = false;
        qm.msaa = GetMSAA(QualitySettings.antiAliasing);
        qm.vsync = QualitySettings.vSyncCount;
        qm.resolution = GetResolutionMoreSimilar();
        qm.mirrorReflexionQuality = 2;
        qm.postProcessingAA = 2;
        qm.postProcessingAALevel = 1;
        return qm;
    }

    public static int GetMSAA(int level)
    {
        for(int i = 0; i < MSAALevels.Length; i++)
        {
            if (MSAALevels[i] == level)
                return i;
        }
        return -1;
    }
    public void ReadValues(SoundMgr sm)
    {
        
        musicVolume = sm.MusicVolume;
        fxVolume = sm.AmbientVolume;
        ambientVolume = sm.SoundVolume;
        mute = sm.IsMute;
        qualityLevel = QualitySettings.GetQualityLevel();
        showFPS = false;
        msaa = GetMSAA(QualitySettings.antiAliasing);
        vsync = QualitySettings.vSyncCount;
        Resolution r = Screen.currentResolution;
        resolution = GetResolutionMoreSimilar();
        mirrorReflexionQuality = 2;
        postProcessingAA = 2;
        postProcessingAALevel = 1;
    }
    

    public bool IsPostprocessingLevelAvailable()
    {
        if (postProcessingAA == 1 || postProcessingAA == 2)
            return true;
        return false;
    }


    public string[] GetPostprocessingLevelNamesKeys()
    {
        if (postProcessingAA == 1)
            return postProcessingAALevelsFXAA;
        else if (postProcessingAA == 2)
            return postProcessingAALevelsSMAAStr;
        else
            return null;
    }

    public string[] GetPostprocessingLevelNamesKeys(int p)
    {
        if (p == 1)
            return postProcessingAALevelsFXAA;
        else if (p == 2)
            return postProcessingAALevelsSMAAStr;
        else
            return null;
    }

    public SubpixelMorphologicalAntialiasing.Quality GetPostprocessingLevelSMAA()
    {
        if(postProcessingAA == 2)
            return postProcessingAALevelsSMAA[PostProcessingAALevel];
        return SubpixelMorphologicalAntialiasing.Quality.Medium;
    }

    public string[] GetVSyncLabels()
    {
        return VSyncLabels;
    }

    public string[] GetMirrorsLabels()
    {
        return Mirrors;
    }

    public int[] GetMSAALevels()
    {
        return MSAALevels;
    }
    public string[] GetMSAALabels()
    {
        return MSAALabels;
    }

    public string[] GetQualityLevelNames()
    {
        return QualitySettings.names;
    }

    public int[] GetMirrorReflectionLevels()
    {
        return mirrorReflexionQualityLevels;
    }

    public int GetMirrorReflectionLevels(int current)
    {
        return mirrorReflexionQualityLevels[current];
    }

    public string[] GetAvailableResolutionStr()
    {
        Resolution[] resolutions = GetAvailableResolution();
        string[] restStr = new string[resolutions.Length];
        for(int i = 0; i < restStr.Length; i++)
        {
            restStr[i] = resolutions[i].ToString();
        }
        return restStr;
    }

    public static Resolution[] GetAvailableResolution()
    {
        return Screen.resolutions;
    }

    public void Apply()
    {
        if (!useQS)
            return;
        QualitySettings.vSyncCount = vsync; // 0 not Vsync, 1 FullVSync support, 2 middle.
        QualitySettings.antiAliasing = MSAALevels[msaa]; // Valid values are 0 (no MSAA), 2, 4, and 8
        QualitySettings.SetQualityLevel(qualityLevel);
        SoundMgr soundMgr = GameMgr.Instance.GetServer<SoundMgr>();
        soundMgr.MusicVolume = musicVolume;
        soundMgr.AmbientVolume = ambientVolume;
        soundMgr.SoundVolume = fxVolume;
        if(mute)
            soundMgr.Mute();
        else
            soundMgr.Unmute();
        Resolution[] resolutions = GetAvailableResolution();
        if (resolution < 0 || resolution >= resolutions.Length)
        {
            resolution = GetResolutionMoreSimilar();
        }
        Resolution r = GetAvailableResolution()[resolution];
        Screen.SetResolution(r.width,r.height,true);
        Save();
    }

    public static int GetResolutionMoreSimilar()
    {
        Resolution[] resolutions = GetAvailableResolution();
        int moreSimilar = -1;
        int distance = int.MaxValue;
        Resolution res = Screen.currentResolution;
        for (int i = 0; i < resolutions.Length; i++)
        {
            int d = Mathf.Abs(resolutions[i].width - res.width) + Mathf.Abs(resolutions[i].height - res.height);
            if(d < distance)
            {
                distance = d;
                moreSimilar = i;
            }
        }

        return moreSimilar;
    }

    private void InitEditorMode()
    {
        _default = DefaultValues();
        if (IsSaveAnySettingConfiguration())
            LoadSettingsEditorMode();
        else
            CopyDefault();
        Apply();
    }

    private void Init()
    {
        _default = DefaultValues();
        if (IsSaveAnySettingConfiguration())
            LoadSettings();
        else
            CopyDefault();
        Apply();
    }

    private void CopyDefault()
    {
        musicVolume=_default.musicVolume;
        fxVolume=_default.fxVolume;
        ambientVolume=_default.ambientVolume;
        mute=_default.mute;
        rumbleEnable=_default.rumbleEnable;
        qualityLevel=_default.qualityLevel;
        showFPS=_default.showFPS;
        msaa=_default.msaa;
        vsync= _default.vsync;
        resolution=_default.resolution;
        mirrorReflexionQuality=_default.mirrorReflexionQuality;
        postProcessingAA=_default.postProcessingAA;
        postProcessingAALevel=_default.postProcessingAALevel;
    }
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        InitEditorMode();
#else
        Init();
#endif
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
