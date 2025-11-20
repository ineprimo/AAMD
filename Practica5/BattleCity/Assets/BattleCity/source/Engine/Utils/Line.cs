using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Line
{
    private Vector3 _pFrom;
    private Vector3 _pTo;
    private int _matId;

    public Line(Vector3 p1, Vector3 p2, int matId = 0)
    {
        _pFrom = p1;
        _pTo = p2;
        _matId = matId;
    }

    public Vector3 From
    {
        get { return _pFrom; }
        set { _pFrom = value; }
    }

    public Vector3 To
    {
        get { return _pTo; }
        set { _pTo = value; }
    }

    public int MatId
    {
        get { return _matId; }
        set { _matId = value; }
    }
}
