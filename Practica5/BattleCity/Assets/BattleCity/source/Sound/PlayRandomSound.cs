using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomSound : MonoBehaviour
{
    public List<string> audioClipList;
    public string alarmName = "WaitingForRandomSound";
    public bool isPositional = false;
    public int maxTimesRepeat = 3;
    public float minPlayTime, maxPlayTime;
    public float MaxDistance = 30f, distanceToMax = 8f;

    private string _lastSound = "";
    private int _soundTimes = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Alarma
        if (GameMgr.Instance.GetServer<TimeMgr>().ExistAlarm(this, alarmName))
            GameMgr.Instance.GetServer<TimeMgr>().CancelAlarm(this, alarmName);

        float waitingTime = Random.Range(minPlayTime, maxPlayTime);
        GameMgr.Instance.GetServer<TimeMgr>().SetAlarm(this, alarmName, WhenTimeFinished, this.gameObject, waitingTime);
    }

    protected void WhenTimeFinished(float time, object data)
    {
        string clip = SoundMgr.GetRandomSound(audioClipList, ref _lastSound, ref _soundTimes, maxTimesRepeat);

        if (isPositional)
        {
            GameMgr.Instance.GetServer<SoundMgr>().PlayOneShotPositional(clip, this.gameObject.transform, MaxDistance, distanceToMax, .3f, 1);
        }
        else
        {
            GameMgr.Instance.GetServer<SoundMgr>().PlaySound(clip);
        }
    }

    /*private void OnDestroy()
    {
        if (GameMgr.Instance.GetServer<TimeMgr>().ExistAlarm(this, alarmName))
            GameMgr.Instance.GetServer<TimeMgr>().CancelAlarm(this, alarmName);
    }*/
}
