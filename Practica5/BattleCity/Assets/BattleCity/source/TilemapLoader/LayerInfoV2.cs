using System.Collections.Generic;
using UnityEngine;


public class LayerInfoData
{
    private List<GameObject> prefabs;
    private Vector3 rotation;
    private bool debug;

    public LayerInfoData()
    {
        prefabs = new List<GameObject>();
    }

    public LayerInfoData(Vector3 a_rotation, bool a_debug) : this()
    {
        debug = a_debug;
        rotation = a_rotation;
        prefabs = new List<GameObject>();
    }

    public List<GameObject> Prefabs
    {
        get {return prefabs;}
        set{prefabs = value;}
    }

    public void AddPrefab(GameObject prefab)
    {
        prefabs.Add(prefab);
    }

    public Vector3 Rotation
    {
        get
        {
            return rotation;
        }

        set
        {
            rotation = value;
        }
    }

    public bool Debug
    {
        get
        {
            return debug;
        }

        set
        {
            debug = value;
        }
    }
}


public class LayerInfoV2
{
    private Dictionary<int, LayerInfoData> _prefabs;
    private Dictionary<int, Vector3> _rotations;
    private float _zPosition;
    private Transform _parent;

    public LayerInfoV2(Dictionary<int, LayerInfoData> dic, float z, Transform parent)
    {
        _prefabs = dic;
        ZPosition = z;
        _parent = parent;
    }

    public Dictionary<int, LayerInfoData> Prefabs
    {
        get
        {
            return _prefabs;
        }
        set
        {
            _prefabs = value;
        }
    }

    public Dictionary<int, Vector3> Rotations
    {
        get
        {
            return _rotations;
        }
        set
        {
            _rotations = value;
        }
    }

    public float ZPosition
    {
        get
        {
            return _zPosition;
        }

        set
        {
           _zPosition = value;
        }
    }

    public Transform Parent
    {
        get
        {
            return _parent;
        }
        set
        {
            _parent = value;
        }
    }
}
