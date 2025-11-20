using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachTarget : MonoBehaviour
{
    public string ID;
    // Start is called before the first frame update
    void Awake()
    {
        AutoAttachGraphicsByID autoAttach = GetAutoAttach(ID);
        if(autoAttach != null)
        {
            autoAttach.transform.parent = this.transform;
        }
    }

    public static AutoAttachGraphicsByID GetAutoAttach(string ID)
    {
        if (ID != null && ID != "")
        {
            AutoAttachGraphicsByID[] autoAttach = FindObjectsByType<AutoAttachGraphicsByID>(FindObjectsSortMode.None);
            for (int i = 0; i < autoAttach.Length; i++)
            {
                if (autoAttach[i].ID == ID)
                {
                    return autoAttach[i];
                }
            }
        }
        return null;
    }

}
