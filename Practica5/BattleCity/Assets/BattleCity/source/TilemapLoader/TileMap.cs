using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileMap : MapProperties
{
    [SerializeField]
    private float version;

    //Number of tiles.
    [SerializeField]
    private int height;
    [SerializeField]
    private int width;

    [SerializeField]
    private string tiledversion;
    //Tile size.
    [SerializeField]
    private int tileheight;
    [SerializeField]
    private int tilewidth;

    //rendering info.
    [SerializeField]
    private int nextobjectid;
    [SerializeField]
    private string orientation;
    [SerializeField]
    private string renderorder;

    [SerializeField]
    private TileLayer[] layers;
    [SerializeField]
    private TileSets[] tilesets;
    [SerializeField]
    private Properties[] properties;


    private Dictionary<string, TileLayer> _layerDic;

    

    public void ConvertToDictionary()
    {
        _layerDic = new Dictionary<string, TileLayer>();
        for (int i = 0; i < layers.Length; ++i)
        {
            _layerDic.Add(layers[i].Name, layers[i]);
        }
        //layers = null;
    }


    public float Version
    {
        get{ return version;}
        set{version = value;}
    }

    public int Height
    {
        get{return height;}
        set{height = value;}
    }

    public int Width
    {
        get{return width;}
        set{width = value;}
    }

    public int Tileheight
    {
        get{return tileheight;}
        set{tileheight = value;}
    }

    public int Tilewidth
    {
        get{return tilewidth;}
        set{tilewidth = value;}
    }

    public int Nextobjectid
    {
        get{return nextobjectid;}
        set{nextobjectid = value;}
    }

    public string Orientation
    {
        get{return orientation;}
        set{orientation = value;}
    }

    public string Renderorder
    {
        get{return renderorder;}
        set{renderorder = value;}
    }

    public bool Exist(string name)
    {
        try
        {
            return _layerDic.ContainsKey(name);
        }
        catch(Exception e)
        {
            throw new Exception("The tile " + name + " not found in tilemap " + this.GetType() + " mesg " + e.Message);
        }
    }

    public TileLayer this[string id] 
    {
        get{return _layerDic[id];}
        set{ _layerDic[id] = value;}
    }

    public TileSets[] Tilesets
    {
        get{return tilesets;}
        set{tilesets = value;}
    }

    public const float CurrentVersion = 1.2f;

    public bool IsSameVersion()
    {
        return CurrentVersion == version;
    }

    public void Add(TileMap tilemap, int offsetX, int offsetY)
    {
        for(int i = 0; i < layers.Length; ++i)
        {
            layers[i].Add(tilemap.layers[i], this.Width, this.Height, offsetX, offsetY);
        }
    }

    protected override Properties[] Properties
    {
        get{return properties;}
        set{ properties = value;}
    }

}
