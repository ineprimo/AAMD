using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankFire : MonoBehaviour
{
    public Rigidbody m_Shell;
    public Transform m_FireTransform;
    public float m_LaunchForce;
    public GraphicUpdate graphicUpdate;
    [SerializeField]
    protected bool canFire;
    public string fireSound;
    // Start is called before the first frame update
    void Start()
    {
        canFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CanFire
    {
        get
        {
            return canFire;
        }
    }

    public void Fire()
    {
        if (!canFire)
            return;
        canFire = false;
        GameObject shellInstance = GameMgr.Instance.GetSpawnerMgr().CreateNewGameObject(m_Shell.gameObject, m_FireTransform.position, Quaternion.identity,null);
        Rigidbody shellInstanceRB = shellInstance.GetComponent<Rigidbody>();
        if(shellInstanceRB!=null)
            shellInstanceRB.linearVelocity = m_LaunchForce * graphicUpdate.LookDirection();
        ShellImpact sinpact = shellInstance.GetComponent<ShellImpact>();
        if(sinpact != null)
            sinpact.Configure(this, graphicUpdate.LookDirection());
        GameMgr.Instance.GetServer<SoundMgr>().PlaySound(fireSound);
    }

    public void Release()
    {
        canFire = true;
    }
}
