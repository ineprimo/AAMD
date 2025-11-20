using UnityEngine;
using System.Collections;

[System.Serializable]
public class StorageMgrConfig
{
    [SerializeField]
    private string m_storageFileName;

    public string StorageFileName { get { return m_storageFileName; } set { m_storageFileName = value; } }
}

[System.Serializable]
public class MemoryMgrConfig
{
    [SerializeField]
    private bool m_activeAutoRecolect;
    [SerializeField]
    private float m_maxFrameRateToRecolect;
    [SerializeField]
    private float m_timeSiceLastGarbage;
    [SerializeField]
    private bool m_recollectUnityAssets;

    public bool ActiveAutoRecolect { get { return m_activeAutoRecolect; } set { m_activeAutoRecolect = value; } }
    public float MaxFrameRateToRecolect { get { return m_maxFrameRateToRecolect; } set { m_maxFrameRateToRecolect = value; } }
    public float TimeSiceLastGarbage { get { return m_timeSiceLastGarbage; } set { m_timeSiceLastGarbage = value; } }
    public bool RecollectUnityAssets { get { return m_recollectUnityAssets; } set { m_recollectUnityAssets = value; } }
}

[CreateAssetMenu(fileName = "Base Config", menuName = "8Picaros/Base Config")]
public class GameMgrConfig : ScriptableObject
{
    public Achievements m_achievements;
    public Globals.PLATFORMS m_platform;
    public PlatformVariants[] platformVariants;
    public Globals.BUILD_TYPE m_buildType;
    public bool m_demo;
    public bool m_useQualitySetting;
    public PlatformsData m_PlatformsData;
    public StorageMgrConfig m_storageMgrConfig;
    public MemoryMgrConfig m_memoryMgrConfig;


    public PlatformVariants GetPlatformVariant()
    {
        for (int i = 0; i < platformVariants.Length; i++)
        {
            if (platformVariants[i].m_platform == m_platform)
                return platformVariants[i];
        }
        if (platformVariants.Length > 0)
            return platformVariants[0];
        else
            return null; 
    }



    public GameMgrConfig()
    {
        m_storageMgrConfig = new StorageMgrConfig();
        m_memoryMgrConfig = new MemoryMgrConfig();
    }
}
