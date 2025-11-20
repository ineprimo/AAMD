using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SubSceneControllerBase : MonoBehaviour
{
    [SerializeField]
    private Transform _groundParent;
    [SerializeField]
    private Transform _graphicParent;
    [SerializeField]
    private Transform _logicParent;

    private static SubSceneControllerBase _instance;

    void Awake()
    {
        _instance = this;
    }




    public static SubSceneControllerBase Get()
    {
        return Utils.GetSecureComponent<SubSceneControllerBase>(ref _instance);
    }


    public Transform GroundParent
    {
        get { return _groundParent; }
        set { _groundParent = value; }
    }

    public Transform GraphicParent
    {
        get { return _graphicParent; }
        set { _graphicParent = value; }
    }


    public Transform LogicParent
    {
        get { return _logicParent; }
        set { _logicParent = value; }
    }
    // Start is called before the first frame update

}
