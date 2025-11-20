using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlatformSpritePopup
{
    public string key;
    private static GUIContent[] _guiContent = null;

    public static bool GuiContentIsNull
    {
        get 
        {
            return _guiContent == null; 
        }
    }

    /// <summary>
    /// Getter del array de GUIContents de las claves
    /// </summary>
    public static GUIContent[] GUIContentArray
    {
        get 
        { 
            if (_guiContent == null)
                RefreshList();
            return _guiContent; 
        }
    }



    public static void RefreshList()
    {
        GUIButtons[] guiButtons = GameMgr.Instance.GameMgrConfigData.m_PlatformsData.gUIButtonsInfo;

        _guiContent = new GUIContent[guiButtons.Length+1];

        string k = "NONE";
        _guiContent[0] = new GUIContent(k, "");

        for (int i = 0; i < guiButtons.Length; ++i)
        {
            k = guiButtons[i].key;
            GUIButtonsData gbd = GetDataByPlatform(guiButtons[i].platforms, GameMgr.Instance.GameMgrConfigData.m_platform);
            if(gbd != null)
                _guiContent[i+1] = new GUIContent(k, gbd.name);
        }
    }

    public static GUIButtonsData GetDataByPlatform(GUIButtonsData[] guiButtonsData,Globals.PLATFORMS platform)
    {
        for (int i = 0; i < guiButtonsData.Length; ++i)
        {
            if (guiButtonsData[i].platform == platform)
                return guiButtonsData[i];
        }
        return null;
    }

}
