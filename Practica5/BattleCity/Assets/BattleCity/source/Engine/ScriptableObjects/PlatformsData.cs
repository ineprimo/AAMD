using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GUIButtonsData : Platform
{
    public Sprite image;
    public string name;
}

[System.Serializable]
public class GUIButtons
{
    public string key;
    public GUIButtonsData[] platforms;
}

[CreateAssetMenu(fileName = "PlatformData", menuName = "8Picaros/Config/PlatformsData")]
public class PlatformsData : ScriptableObject
{
    // Start is called before the first frame update
    public GUIButtons[] gUIButtonsInfo;


    public Sprite GetSprite(string key, Globals.PLATFORMS platform)
    {
        for(int i = 0; i < gUIButtonsInfo.Length; i++)
        {
            if(gUIButtonsInfo[i].key == key)
            {
                for (int j = 0; j < gUIButtonsInfo[i].platforms.Length; j++)
                {
                    if (gUIButtonsInfo[i].platforms[j].platform == platform)
                        return gUIButtonsInfo[i].platforms[j].image;
                }
            }
        }
        return null;
    }
}
