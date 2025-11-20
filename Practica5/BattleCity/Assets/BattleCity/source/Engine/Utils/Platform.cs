using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Platform
{
    public Globals.PLATFORMS platform = Globals.PLATFORMS.DEFAULT;


    public static T GetCurrentPlatformComponent<T>(T[] platforms) where T : PlatformComponent
    {
        T def = null;
        for (int i = 0; i < platforms.Length; i++)
        {
            if (platforms[i].platform == Globals.PLATFORMS.DEFAULT)
                def = platforms[i];
            if (GameMgr.Instance.CurrentPlatform == platforms[i].platform)
                return platforms[i];
        }
        return def;
    }

    public static T GetCurrentPlatform<T>(T[] platforms) where T : Platform
    {
        T def = null;
        for(int i = 0; i < platforms.Length; i++)
        {
            if (platforms[i].platform == Globals.PLATFORMS.DEFAULT)
                def = platforms[i];
            if (GameMgr.Instance.CurrentPlatform == platforms[i].platform)
                return platforms[i];
        }
        return def;
    }

    public static Q GetCurrentPlatform<T,Q>(Dictionary<Globals.PLATFORMS, Q> platforms, out bool ok) where T : Platform
    {
        ok = false;
        Q def = platforms[Globals.PLATFORMS.DEFAULT];
        if (platforms.ContainsKey(GameMgr.Instance.CurrentPlatform))
        {
            ok = true;
            return platforms[GameMgr.Instance.CurrentPlatform];
        }
        else
            return def;
    }
}
