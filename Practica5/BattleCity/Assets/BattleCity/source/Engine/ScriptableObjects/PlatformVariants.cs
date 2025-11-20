using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlatformVariant", menuName = "8Picaros/Config/PlatformVariant")]
public class PlatformVariants : ScriptableObject
{
    public Globals.PLATFORMS m_platform;
    public int CPU_POWER;
    public int MAX_MEMORY;
    public int GPU_POWER;
    public int STORAGE_SPEED;
    
}
