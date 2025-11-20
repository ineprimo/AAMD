using UnityEngine;
using System.Collections;

/// <summary>
/// Spawner. Trigger que almacena en el SpawnerMgr la ultima aparicion del player....
/// </summary>
public class Spawner : MonoBehaviour
{
	
	public string[] m_tags;
	private SpawnerMgr m_spawnerMgr;

	
	void Start()
	{
		m_spawnerMgr = GameMgr.Instance.GetSpawnerMgr();
		Assert.AbortIfNot(m_spawnerMgr != null,"Error, no existe el SpawnerMgr en la lista de servidores");
	}
	
	void OnTriggerEnter(Collider other) {
		
		bool found = false;
		for(int i = 0; !found && i < m_tags.Length; ++i)
		{
			if(other.gameObject.tag == m_tags[i])
				found = true;
		}
		
		if(found)
		{
			if(m_spawnerMgr.GetPlayerSpawnerPoint() != this.transform)
			{
				m_spawnerMgr.ChangeSpawnPoint(this.transform);;
			}
		}
	}
}
