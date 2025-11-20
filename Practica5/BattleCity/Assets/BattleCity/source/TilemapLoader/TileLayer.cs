using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Layer
{
    [SerializeField]
    private int height;

    [SerializeField]
    private string name;

    [SerializeField]
    private string type;

    [SerializeField]
    private int opacity;

    [SerializeField]
    private bool visible;

    [SerializeField]
    private int width;

    [SerializeField]
    private int x;

    [SerializeField]
    private int y;

    public int Height
    {
        get { return height; }
        set { height = value; }
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public int Opacity
    {
        get { return opacity; }
        set { opacity = value; }
    }

    public string Type
    {
        get { return type; }
        set { type = value; }
    }

    public bool Visible
    {
        get { return visible; }
        set { visible = value; }
    }

    public int Width
    {
        get { return width; }
        set { width = value; }
    }

    public int X
    {
        get { return x; }
        set { x = value; }
    }

    public int Y
    {
        get { return y; }
        set { y = value; }
    }
}

/// <summary>
/// Class record the description of the layer used in tiled.
/// </summary>
[Serializable]
public class TileLayer : ObjectGroup
{
    [SerializeField]
    private int[] data;

    public const string TileLayer_TYPE = "tilelayer";

    public int[] Data
    {
        get{return data;}
        set{data = value;}
    }

    public ObjectGroup ConverToObjectGroup()
    {
        if (Type == ObjectGroup.ObjectGroup_TYPE)
            return (ObjectGroup) this;
        else
            return null;
    }

    public void Add(TileLayer tileLayer, int witdh, int height, int xOffset, int yOffset)
    {
        if (tileLayer.Data != null)
        {
            for (int i = 0; i < tileLayer.Data.Length; ++i)
            {
                int x = i % tileLayer.Width;
                int y = i / tileLayer.Width;
                int nX = x + xOffset;
                int nY = y + yOffset;
                int position = Width * nY + nX;
                data[position] = tileLayer.data[i];
            }
        }

        if(tileLayer.Objects != null && tileLayer.Objects.Length > 0)
        {
            int initialSize = this.Objects.Length;
            this.ExtendSize(tileLayer.Objects.Length);

            for (int i = 0; i < tileLayer.Objects.Length; ++i)
            {
                ObjectMap objMap = tileLayer.Objects[i];
                this.Objects[initialSize + i] = objMap;
                this.Objects[initialSize + i].TileX = this.Objects[initialSize + i].TileX + xOffset;
                this.Objects[initialSize + i].TileY = this.Objects[initialSize + i].TileY + yOffset;
            }
        }
        /*for(int i = 0; i < tileLayer.Width; ++i)
        {
            for (int j = 0; j < tileLayer.Height; ++j)
            {
                //Debug.Log("tileLayer.data =>index=" + (i * tileLayer.Height + j) + " size " + tileLayer.data.Length + " i "+i + " j "+j);
                int v = tileLayer.data[i * tileLayer.Height + j];
                if (v != 0)
                {
                    int position = (i + xOffsset) * height + (j + yOffset);
                    int actual = data[position];

                    if (actual != 0)
                        Debug.LogError("La casilla " + (i + xOffsset) + " " + (j - yOffset) + " tenia el dato " + actual + " y se pretende cambiar por "+v);
                    //Debug.Log("data => index " + ((i + xOffsset) * height + (j+ yOffset)) + " size " + data.Length);
                    data[(i) * height + (j+ xOffsset)] = tileLayer.data[i * tileLayer.Height + j];
                }
            }
        }*/
    }
}

[Serializable]
public class ObjectGroup : Layer
{
    [SerializeField]
    private ObjectMap[] objects;

    public ObjectMap[] Objects
    {
        get { return objects; }
        set { objects = value; }
    }

    public void ExtendSize(int newElements)
    {
        Array.Resize(ref this.objects, this.objects.Length+newElements);
    }

    public const string ObjectGroup_TYPE = "objectgroup";
}

[Serializable]
public class ObjectMap : MapProperties
{
    public const int PATTERN_SIZE = 128;
    /*public const int MOBILE_PLATFORM = 47;
    public const int POINT = 46;
    public const int NITPICK = 44;
    public const int MASILLA = 23;*/

    [SerializeField]
    private int gid;

    [SerializeField]
    private int height;

    [SerializeField]
    private int id;

    [SerializeField]
    private string name;

    [SerializeField]
    private int rotation;

    [SerializeField]
    private string type;

    [SerializeField]
    private string _class;

    [SerializeField]
    private bool visible;

    [SerializeField]
    private int width;

    [SerializeField]
    private float x;

    [SerializeField]
    private float y;

    [SerializeField]
    private Properties[] properties;

    private int layer;


    protected override Properties[] Properties
    {
        get { return properties; }
        set { properties = value; }
    }

    public int Layer
    {
        get { return layer; }
        set { layer = value; }
    }

    public int Gid
    {
        get { return gid; }
        set { gid = value; }
    }

    public int Height
    {
        get { return height; }
        set { height = value; }
    }

    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }


    public int Rotation
    {
        get { return rotation; }
        set { rotation = value; }
    }

    public string Type
    {
        get { return type; }
        set { type = value; }
    }

    public string Class
    {
        get { return _class; }
        set { _class = value; }
    }

    public bool Visible
    {
        get { return visible; }
        set { visible = value; }
    }

    public int Width
    {
        get { return width; }
        set { width = value; }
    }

    public int WidthInTiles
    {
        get {
            //return width;
            return width / PATTERN_SIZE;
        }
    }

    public int HeightInTiles
    {
        get
        {
            //return width;
            return height / PATTERN_SIZE;
        }
    }

    public float X
    {
        get { return x; }
        set { x = value; }
    }

    public float Y
    {
        get { return y; }
        set { y = value; }
    }

    public float TileX
    {
        get
        {
            float t = x / PATTERN_SIZE;
            return Mathf.Round(t);
        }
        set
        {
            x = value * PATTERN_SIZE;
        }
    }

    public float TileY
    {
        get
        {
            float t = y / PATTERN_SIZE;
            return Mathf.Round(t);
        }

        set
        {
            y = value * PATTERN_SIZE;
        }
    }
}
