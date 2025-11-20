using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public delegate void OnVideoEnd();

    public VideoPlayer _videoPlayer;
    private event OnVideoEnd _onVideoEnd;

    public void Register(OnVideoEnd del)
    {
        _onVideoEnd += del;
    }

    public void Unregister(OnVideoEnd del)
    {
        _onVideoEnd -= del;
    }

    public void Stop()
    {
        if (_videoPlayer != null)
            _videoPlayer.Stop();
    }

    public void Play()
    {
        if (_videoPlayer != null)
            _videoPlayer.Play();
    }

    public void Pause()
    {
        if(_videoPlayer != null)
            _videoPlayer.Pause();
    }
    // Start is called before the first frame update
    void Start()
    {
        _videoPlayer.loopPointReached += _OnVideoEnd;
    }

    // Update is called once per frame
    private void _OnVideoEnd(VideoPlayer vp)
    {
        if(_onVideoEnd!=null)
            _onVideoEnd();
    }
}
