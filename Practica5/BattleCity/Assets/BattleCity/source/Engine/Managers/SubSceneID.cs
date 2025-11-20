using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSceneID : MonoBehaviour
{
    public int _sectionID;

    private static SubSceneID _uniqueSubSceneId;
    public int SectionID
    {
        get
        {
            return _sectionID;
        }
    }
    public string ID
    {
        get {return this.gameObject.scene.name; }
    }

    private void Awake()
    {
        SubSceneID[] ssIDs = FindObjectsByType<SubSceneID>(FindObjectsSortMode.None);
        if (ssIDs.Length != 1)
            Debug.LogException(new System.Exception("Error, en esta escena hay más de un SubSceneID"));

        _uniqueSubSceneId = this;
    }

    public static SubSceneID GetSubSceneID() { return _uniqueSubSceneId; }
}
