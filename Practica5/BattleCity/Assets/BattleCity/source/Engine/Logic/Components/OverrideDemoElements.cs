using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils;


/// <summary>
/// Este componente nos permite sobreescribir los comportamiento que cargamos del tiled automáticamente para las demos.
/// Por jemplo nosp ermite reescribir en una salida concreta, por ejemplo la salida del boss, la escena que cargará.
/// </summary>
public class OverrideDemoElements : DemoComponent
{
    [Tooltip("Busca las puertas existentes a traves de su Id en Tiled y se le asigna el destino de la puerta. Esto es transparente para diseño en tiled y nos permite crear demos facilmente")]
    public SerializeIntIDValue[] overrideExitScene;
    private Dictionary<int, string> overrideExitSceneDic;
    // Start is called before the first frame update

    public string FindExitSceneByTiledID(int tiledID)
    {
        string exitScene = null;
        if(overrideExitSceneDic == null)
        {
            overrideExitSceneDic = new Dictionary<int, string>();
            for(int i = 0; i < overrideExitScene.Length; i++)
            {
                overrideExitSceneDic.Add(overrideExitScene[i].hashID, overrideExitScene[i].val);
                if(overrideExitScene[i].hashID == tiledID)
                {
                    exitScene = overrideExitScene[i].val;
                }
            }
        }
        else
        {
            exitScene = overrideExitSceneDic[tiledID];
        }
        return exitScene;
    }
    
}
