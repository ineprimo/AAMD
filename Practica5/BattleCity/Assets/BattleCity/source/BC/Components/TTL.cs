using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTL : MonoBehaviour
{
    public float ttl;
    public bool _initOnEnable;
    public bool _initOnStart;
    public bool _realDestroy;

    // Start is called before the first frame update
    private float _ttl = float.NegativeInfinity;
    private System.Action OnTTlFinish;
    public void InitTTL()
    {
        _ttl = ttl;
        enabled = true;
    }

    public void SetOnTTLFinish(System.Action a)
    {
        OnTTlFinish = a;
    }

    private void OnEnable()
    {
        if (_initOnEnable)
            InitTTL();
    }

    private void Start()
    {
        if (_initOnStart)
            InitTTL();
    }
    // Start is called before the first frame update

    // Update is called once per frame
    private void Update()
    {
        if(_ttl != float.NegativeInfinity)
        {
            _ttl -= Time.deltaTime;
            if (_ttl <= 0f)
            {
                if (OnTTlFinish != null)
                    OnTTlFinish();
                GameMgr.Instance.GetSpawnerMgr().DestroyGameObject(this.gameObject, _realDestroy);
            }
        }
    }
}
