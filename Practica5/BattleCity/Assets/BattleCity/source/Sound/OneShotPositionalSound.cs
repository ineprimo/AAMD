using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotPositionalSound : MonoBehaviour
{
    public string audioClipName;
    public float maxDistanceToPlaySound;
    public float distanceToMaxVolumen;
    public float minPan = 0.3f;
    public int priority;
    // Start is called before the first frame update

    public void PlayPositionalSound()
    {
        GameMgr.Instance.GetServer<SoundMgr>().PlayOneShotPositional(audioClipName, this.transform, maxDistanceToPlaySound, distanceToMaxVolumen, minPan, priority);
    }
}
