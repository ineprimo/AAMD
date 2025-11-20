using UnityEngine;
using System.Collections;


//Componente que destriye el objeto pasados X segundos.
public class DestroyMsg : Message<bool> { };

public class AutoDestroy : MonoBehaviour
{
	public float m_timeToDestroy;
	private float m_time;
	public bool m_destroy = false;
    private MessageMgr _messageMgr;

    void Update()
	{
		m_time -= Time.deltaTime;
		if(m_time < 0f)
		{
            GameMgr.Instance.GetSpawnerMgr().DestroyGameObject(this.gameObject,m_destroy);
		}
	}
	
	
	void OnEnable() 
	{
		m_time = m_timeToDestroy;
	}
	
	void Awake()
	{
		m_time = m_timeToDestroy;
        _messageMgr = gameObject.GetMessageMgr();
    }
	
}
