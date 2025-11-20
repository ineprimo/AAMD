using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void UnloadAssetsCompleted();

public class MemoryMgr : MonoBehaviour
{
    private bool configure = false;

	public void Configure(bool active, float maxFramerateToRecolect, float timeToRecolect, bool recolectUnityAssets)
	{
		m_collectioncount = 0;
		m_timeToRecolect = timeToRecolect;
		m_active = active;
		m_configure = true;
		m_maxFramerateToRecolect = maxFramerateToRecolect;
		m_recolectUnityAssets = recolectUnityAssets;
		m_timeTheLastGarbages = 0f;
        configure = true;
	}
	
	public bool GarbageRecolect(bool forceToRecolect)
	{
		Assert.AbortIfNot(m_configure,"MemoryMgr no ha sido configurado");
		bool isGarbage = false;
		if(forceToRecolect || GetTimeSiceTheLastGarbageCall() > m_timeToRecolect)
		{
			Debug.Log("GarbageRecolect run "+GetTimeSiceTheLastGarbageCall());
			System.GC.Collect();
			System.GC.WaitForPendingFinalizers();
			m_collectioncount = GetNumberOfCollectCalls();
			m_timeTheLastGarbages = Time.time;
			isGarbage = true;
            if (m_recolectUnityAssets)
                CleanAssets(null);

        }
		
		return isGarbage;
	}
	
	public AsyncOperation CleanAssets(UnloadAssetsCompleted callback)
	{
		
		AsyncOperation op = Resources.UnloadUnusedAssets();
		if(callback != null)
		{
			StartCoroutine("UnloadAssetsFinish",new TPair<AsyncOperation,UnloadAssetsCompleted>(op,callback));
		}
		return op;
	}
	
	IEnumerator UnloadAssetsFinish(TPair<AsyncOperation,UnloadAssetsCompleted> pair)
	{
		AsyncOperation op = pair.First;
		UnloadAssetsCompleted callback = pair.Second;
		while(!op.isDone)
		{
			yield return null;
		}
		if(callback != null)
		    callback();
	}
	
	protected int GetNumberOfCollectCalls()
	{
		int num = 0;
		for(int i = 0; i < System.GC.MaxGeneration; ++i)
		{
			num += System.GC.CollectionCount(i);
		}
		return num;
	}
		
	protected bool IsGarbageCollectionRun()
	{
		return m_collectioncount != GetNumberOfCollectCalls();
	}
	
	protected void UpdateGargabeLastTime()
	{
		if(IsGarbageCollectionRun())
		{
			m_timeTheLastGarbages = Time.time;
		}
	}
	
	void Update ()
	{
        if (m_active)
        {
            Debug.Assert(configure, "Error, el manager de Memoria no ha sido inicializado correctamente. Posible error en la inicialización o duplicado del objeto server");
            if (gameObject.activeSelf)
            {
                Assert.AbortIfNot(m_configure, "MemoryMgr no ha sido configurado");
                bool collect = false;
                if (Time.deltaTime <= m_maxFramerateToRecolect)
                {
                    collect = GarbageRecolect(false);
                }
                if (!collect)
                    UpdateGargabeLastTime();
            }
        }
	}
	
	public float GetTimeSiceTheLastGarbageCall()
	{
		return Time.time - m_timeTheLastGarbages;
	}
	
	
	
	protected float m_timeToRecolect;
	protected float m_maxFramerateToRecolect;
	protected int m_collectioncount;
	protected bool m_active;
	protected bool m_configure;
	protected bool m_recolectUnityAssets;
	protected float m_timeTheLastGarbages;
}
