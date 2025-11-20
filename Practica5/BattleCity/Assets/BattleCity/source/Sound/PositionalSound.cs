using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionalSound : MonoBehaviour
{
    public string soundMain;
    public string soundBackground;
    [Tooltip("la distancia z a partir de la cual determino si es un sound u otro")]
    public float zPosition;
    public float distancetoPlaysound;
    public float distanceToMaxVolumen;
    public float minPan = 0.3f;
    private bool _soundEnable;
    private SoundMgr _soundMgr;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        _soundMgr = GameMgr.Instance.GetServer<SoundMgr>();
        _soundMgr.PlayLoopSoundPositional(GetSound(), distancetoPlaysound, distanceToMaxVolumen, minPan, this.gameObject);
        _soundEnable = true;
    }

    private string GetSound()
    {
        if (this.transform.position.z < zPosition)
            return soundMain;
        else
            return soundBackground;
    }

    private void OnDisable()
    {
        if(_soundMgr != null)
        {
            _soundMgr.StopSound(GetSound(), this.gameObject);
            _soundEnable = false;
        }
    }

    private void OnDestroy()
    {
        if(_soundEnable && _soundMgr != null)
        {
            _soundMgr.StopSound(GetSound(),this.gameObject);
            _soundEnable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
