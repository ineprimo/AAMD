using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnAnimation : MonoBehaviour
{
    public List<string> audioClipList;
    public bool isPositional = false;
    public int maxTimesRepeat = 1;
    public float MaxDistance = 30f, distanceToMax = 8f;

    private string _lastSound = "";
    private int _soundTimes = 0;
    private bool _isEnabled = false;
    private SoundMgr sm;

    void Start()
    {
        _isEnabled = audioClipList.Count > 0;
        if (_isEnabled)
            sm = GameMgr.Instance.GetServer<SoundMgr>();
    }

    public void Playsound()
    {
        if(_isEnabled)
        {
            string clip = SoundMgr.GetRandomSound(audioClipList, ref _lastSound, ref _soundTimes, maxTimesRepeat);
            if (!sm.IsSoundPlaying(clip))
            {
                if (isPositional)
                {
                    sm.PlayOneShotPositional(clip, this.gameObject.transform, MaxDistance, distanceToMax, .3f, 1);
                }
                else
                {
                    sm.PlaySound(clip);
                }
            }
        }
    }

    public void PlaySoundDisable()
    {
        _isEnabled = false;
    }
}
