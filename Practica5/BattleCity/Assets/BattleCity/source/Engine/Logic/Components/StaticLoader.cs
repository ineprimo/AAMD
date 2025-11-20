using UnityEngine;
using System.Collections;

/// <summary>
/// Componente que permite cargar el siguiente nivel desde una pantalla de carga estática. Se presupone que el nivel que ha puesto la pantalla de carga debe
/// haber establecido previamente la variable SCENE_SECTION/NEXT_SCENE con el nombre de la escena a cargar.
/// </summary>
public class StaticLoader : MonoBehaviour
{

	// Update is called once per frame
	void Update()
	{
		SceneMgr sceneMgr = GameMgr.Instance.GetServer<SceneMgr>();
		sceneMgr.ChangeScene(GameMgr.Instance.GetStorageMgr().GetVolatile<string>(SceneMgr.SCENE_SECTION,SceneMgr.NEXT_SCENE));
	}
}
