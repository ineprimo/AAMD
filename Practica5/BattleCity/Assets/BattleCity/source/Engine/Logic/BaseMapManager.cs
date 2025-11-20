using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class BaseMapManager : MonoBehaviour {

    public TextAsset _mapResourceFile;
    public TextAsset _mapLogicResourceFile;
    public float _tileSize;
    public int _mapWidth;
    public int _mapHeight;
    public int _physicXtileSize;
    public int _physicYtileSize;
    public int _zPosition;
    public Transform _scenary;
    public bool _updateMap;
    protected bool _isCreated = false;
    protected GameObject[,] _physicsTiles;

    
    protected int _fXSize;
    protected int _fYSize;
    // Use this for initialization
    //Se crea el mapa al cargar la escena o subscene. Si se lanza desde el editor se crea el mapa manualmente.

    private void Awake()
    {

    }

    void Start () {
        CreateMap();
        _Start();
    }

    protected abstract void _Start();

    public static BaseMapManager Get(Scene scene)
    {
        GameObject[] rootGameObjects = scene.GetRootGameObjects();
        BaseMapManager mapMngr_cmp = null;
        for(int i = 0; i < rootGameObjects.Length && mapMngr_cmp == null; i++)
        {
            mapMngr_cmp = rootGameObjects[i].GetComponentInChildren<BaseMapManager>();
        }

        return mapMngr_cmp;
    }
	
    public virtual void CreateMap()
    {
        if (_isCreated)
            return;

        _fXSize = _mapWidth / _physicXtileSize;
        _fYSize = _mapHeight / _physicYtileSize;

        int size = _scenary.childCount;
   
        _isCreated = true;
    }

}
