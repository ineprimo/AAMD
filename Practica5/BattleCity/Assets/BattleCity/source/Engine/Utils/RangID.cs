using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangID
{
    private int _id;
    private int _min;
    private int _max;
    private int _inc;

    public RangID(int min, int max, int inc = 1)
    {
        _id = min;
        _min = min;
        _max = max;
        _inc = inc;
    }


    public int GetID
    {
        get
        {
            int r = _id;
            _id = (_id + _inc) % _max;
            if (_id < _min)
                _id = _min;
            return r;
        }
    }
}
