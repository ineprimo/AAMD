using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSprite : MonoBehaviour
{
    private SceneMgr m_gameMgr;
    public Image _imageSprinteRotation;
    public Image _imageLoading;
    public float _speed;
    // Use this for initialization
    void Start()
    {

        m_gameMgr = GameMgr.Instance.GetServer<SceneMgr>();
        m_gameMgr.RegisterLoadingSceneProgressCallback(ProgressLoading);
        _imageLoading.fillAmount = 0.15f;
    }

    // Update is called once per frame
    void Update()
    {
        if(_imageSprinteRotation != null)
            _imageSprinteRotation.transform.Rotate(new Vector3(0, 0, _speed * Time.deltaTime));
    }

    protected void ProgressLoading(float progress, bool finish)
    {
        _imageLoading.fillAmount = progress > _imageLoading.fillAmount ? progress : _imageLoading.fillAmount;
    }


    void OnDestroy()
    {
        if (m_gameMgr != null)
            m_gameMgr.UnRegisterLoadingSceneProgressCallback(ProgressLoading);
    }
}
