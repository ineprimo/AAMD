using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCaptureComponent : MonoBehaviour
{
    public int _supersampling;
    public string _name;
    public bool _withGUI;
    // Start is called before the first frame update

    public void OnScreenCapture()
    {
        StartCoroutine(ScreenCaptureCorutine());
    }

    private IEnumerator ScreenCaptureCorutine()
    {
        GUIControllerBase guicontroller = null;
        if (!_withGUI)
        {
            guicontroller = GameMgr.Instance.GetGUIController(); ;
            if (guicontroller != null)
                guicontroller.ShowGUI(false);
        }

        yield return new WaitForEndOfFrame();
        if (_supersampling > 0)
            GameMgr.Instance.GetServer<ScreenShootMgr>().TakeScreenShotAndSerialized(_name, _supersampling);
        else
            GameMgr.Instance.GetServer<ScreenShootMgr>().TakeScreenShotAndSerialized(_name);

        yield return new WaitForEndOfFrame();
        if(_supersampling > 0)
            yield return new WaitForSeconds(1);
        if (!_withGUI)
        {
            if (guicontroller != null)
                guicontroller.ShowGUI(true);
        }


        yield return null;
    }
}
