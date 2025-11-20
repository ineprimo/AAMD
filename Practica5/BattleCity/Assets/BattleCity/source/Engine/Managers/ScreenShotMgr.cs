using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScreenShootMgr : MonoBehaviour
{
    private List<Texture2D> _screenCaptures;
    private bool _captureActive;
    private int _supersampling;
    private string _screenshotPath;
    public const string WINDOWS_SCREENSHOT_DEBUG = "";
    public const string WINDOWS_SCREENSHOT_RELEASE = "Screenshoots";


    private void Awake()
    {
        DontDestroyOnLoad(this);
        _screenCaptures = new List<Texture2D>();
        _captureActive = false;

#if UNITY_EDITOR
        _screenshotPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("/"));
#elif UNITY_DEBUG
        _screenshotPath = Application.dataPath;
#elif  UNITY_STANDALONE_WIN
        _screenshotPath = Application.persistentDataPath + "/" + WINDOWS_SCREENSHOT_RELEASE;
#endif
    }

    public string screenshotPath
    {
        get { return _screenshotPath; }
        set { _screenshotPath = value; }
    }

    public void TakeScreenShotAndSerialized(string path, int supersampling)
    {
        DateTime now = DateTime.Now;
        ScreenCapture.CaptureScreenshot(_screenshotPath+"/"+path+ "_"+ now.Year+"_"+now.Month+"_"+now.Day+"_"+now.Hour+"_"+now.Minute+"_"+now.Second+".jpg", supersampling);
    }

    public void TakeScreenShotAndSerialized(string path)
    {
        DateTime now = DateTime.Now;
        ScreenCapture.CaptureScreenshot(_screenshotPath + "/" + path + "_" + now.Year + "_" + now.Month + "_" + now.Day + "_" + now.Hour + "_" + now.Minute + "_" + now.Second + ".jpg");
    }

    public void ClearCaptures()
    {
        Debug.Log("Borrando las capturas");
        for(int i = 0; i < _screenCaptures.Count; ++i)
        {
            if(_screenCaptures[i] != null)
                UnityEngine.Object.Destroy(_screenCaptures[i]);
        }
        _screenCaptures.Clear();
    }

    public void TakeScreeShotAndSave(int supersampling = 1)
    {
        _supersampling = supersampling;
        _captureActive = true;
    }
    
    public List<Texture2D> GetCaptures()
    {
        return _screenCaptures;
    }

    public int NumScreenShots
    {
        get { return _screenCaptures.Count; }
    }

    public Texture2D GetCaptures(int i)
    {
        return _screenCaptures[i];
    }

    public void Save(int index, string path)
    {
        Save(_screenCaptures[index],path);
    }

    public void Save(Texture2D text, string path)
    {
        byte[] byteArray = ImageConversion.EncodeToJPG(text);
        File.WriteAllBytes(path, byteArray);
    }

    IEnumerator RecordFrame()
    {
        yield return new WaitForEndOfFrame();
        _screenCaptures.Add(ScreenCapture.CaptureScreenshotAsTexture(_supersampling));
        Debug.Log("==>> Campuras almacenadas " + _screenCaptures.Count);
    }

    public void LateUpdate()
    {
        if (_captureActive)
        {
            _captureActive = false;
            StartCoroutine(RecordFrame());
        }
    }


    private void OnDestroy()
    {
        ClearCaptures();
    }
}
