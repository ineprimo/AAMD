using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineMeshInfo
{
    public CombineInstance[] _combineInstances;
    public int _index;
    public Material _material;
    public Vector3 _position;
    public int _vertexAcum;

    public CombineMeshInfo(int size)
    {
        _combineInstances = new CombineInstance[size];
        _index=0;
        _material=null;
        _position=Vector3.zero;
        _vertexAcum = 0;
    }
}
