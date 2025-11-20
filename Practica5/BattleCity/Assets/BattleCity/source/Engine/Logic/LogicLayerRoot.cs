using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicLayerRoot : MonoBehaviour
{
    private static LogicLayerRoot logicLayerRootInstance = null;
    public static LogicLayerRoot Get
    {
        get
        {
            if(logicLayerRootInstance == null)
            {
                LogicLayerRoot[] logicObjects = FindObjectsByType<LogicLayerRoot>(FindObjectsSortMode.None);
                if(logicObjects != null && logicObjects.Length > 0)
                    logicLayerRootInstance = logicObjects[0];

                if (logicObjects.Length > 1)
                {
                    Debug.LogError("Algo debe estar mal porque hay mas de un LogicLayerRoot");
                }
            }
            return logicLayerRootInstance;
        }
    }

    private void Awake()
    {
        if(logicLayerRootInstance == null)
            logicLayerRootInstance = this;
        else if (logicLayerRootInstance != this)
        {
            Debug.LogError("Algo extraño puede estar pasando porque hay dos objetos de tipo LogicLayerRoot en la escena, voya destruir este "+this.gameObject+" por seguridad");
            Destroy(this.gameObject);
        }
    }
    public Transform Root
    {
        get
        {
            return this.transform;
        }
    }

    private void OnDestroy()
    {
        logicLayerRootInstance = null;
    }

}
