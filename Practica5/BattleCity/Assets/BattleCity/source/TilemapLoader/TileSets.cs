using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileSets
{
    [SerializeField]
    private int columns;
    [SerializeField]
    private int firstgid;
    [SerializeField]
    private string source;
    [SerializeField]
    private string image;
    [SerializeField]
    private int imageheight;
    [SerializeField]
    private int imagewidth;
    [SerializeField]
    private int margin;
    [SerializeField]
    private string name;
    [SerializeField]
    private int spacing;
    [SerializeField]
    private int tileCount;
    [SerializeField]
    private int tileheight;
    [SerializeField]
    private int tilewidth;


    public int Firstgid
    {
        get { return firstgid; }
        set { firstgid = value; }
    }

    public string Source
    {
        get { return source; }
        set { source = value; }
    }
}
