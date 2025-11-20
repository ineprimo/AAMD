using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum LogicTileType { EMPTY = 0, EDGE, BONDARY, WALKABLE, SOLID, TRANSPASSABLE, NOT_SECURE, LOGIC_WALL }

[Serializable]
public class LogicTile
{

    private Transform _transform;
    private LogicTileType _type;
    [SerializeField]
    private bool _canWalk;
    private bool _error;
    private string _info;
    private bool _transparent;

    public LogicTile()
    {
        _transform = null;
        _type = LogicTileType.EMPTY;
        _canWalk = true;
        _error = false;
        _info = "";
        _transparent = false;
    }

    public string Serialize()
    {
        string s = "{";
        s += "\"_type\":" +((int) _type)+",";
        s += "\"_canWalk\":" + _canWalk + ",";
        return s + "}";
    }

    public Transform TileTransform
    {
        get { return _transform; }
        set { _transform = value; }
    }

    public bool Transparent
    {
        get { return _transparent; }
        set { _transparent = value; }
    }

    public bool CanWak
    {
        get { return _canWalk; }
        set { _canWalk = value; }
    }

    public LogicTileType Type
    {
        get { return _type; }
        set { _type = value; }
    }

    public bool Error
    {
        get { return _error; }
        set { _error = value; }
    }
    public string Info
    {
        get { return _info; }
        set { _info = value; }
    }
}

public class TileData : AINode<TileData>
{
    private int _x;
    private int _y;
    private LogicTileType _type;
    private bool _canWalk;
    private float _cost;

    public TileData(int x, int y, LogicTileType t, bool walk)
    {
        _x = x;
        _y = y;
        _type = t;
        _canWalk = walk;
        _cost = float.MaxValue;
        switch (t)
        {
            case LogicTileType.BONDARY:
                _cost = float.MaxValue;
                break;
            case LogicTileType.EDGE:
                _cost = float.MaxValue;
                break;
            case LogicTileType.SOLID:
                _cost = float.MaxValue;
                break;
            case LogicTileType.EMPTY:
                _cost = 1f;
                break;
            case LogicTileType.WALKABLE:
            case LogicTileType.TRANSPASSABLE:
                _cost = 4f;
                break;
        }
    }

    public float TileCost
    {
        get { return _cost; }
        set { _cost = value; }
    }


    public override string ToString()
    {
        return "(" + X + "," + Y + ")";
    }


    public LogicTileType Type
    {
        get { return _type; }
        set { _type = value; }
    }

    public bool CanWalk
    {
        get { return _canWalk; }
        set { _canWalk = value; }
    }

    public int X
    {
        get { return _x; }
        set { _x = value; }
    }

    public int Y
    {
        get { return _y; }
        set { _y = value; }
    }
}

public class LogicLayerData
{
    private TileData[,] _tiles;
    private int _xSize;
    private int _ySize;

    public LogicLayerData(LogicLayer layer)
    {
        CreateLayer(layer);
    }

    private void CreateLayer(LogicLayer layer)
    {
        _xSize = layer.XSize;
        _ySize = layer.YSize;
        _tiles = new TileData[_xSize, _ySize];
        for (int i = 0; i < _xSize; ++i)
        {
            for (int j = 0; j < _ySize; ++j)
            {
                _tiles[i, j] = new TileData(i,j,layer[i,j].Type, layer[i, j].CanWak);
            }
        }
    }

    public int XSize
    {
        get { return _xSize; }
    }

    public TileData this[int x, int y]
    {
        get {
            if (x < 0f || x >= _xSize)
                return null;
            if (y < 0f || x >= _ySize)
                return null;
            return _tiles[x, y]; 
        }
    }

    public int YSize
    {
        get { return _ySize; }
    }
}


public class LogicLayer
{
    public delegate void OnModifyTile(int x, int y, LogicTile lt);
    private int _zPosition;
    private LogicTile[,] _tiles;
    private int _xSize;
    private int _ySize;
    private OnModifyTile _notify;

    public LogicLayer(int xSize, int ySize, int zPosition)
    {
        CreateLayer(xSize, ySize);
        _zPosition = zPosition;
    }

    public static LogicLayer CreateFromJSon(string json)
    {
        LogicLayerJsonWrapper myObject = JsonUtility.FromJson<LogicLayerJsonWrapper>(json);
        LogicLayer ll = myObject.CreateLogicLayer();
        return ll;
    }

    public void ShowNotSecure()
    {
        string positions = "ShowNotSecure";
        for (int i = 0; i < _xSize; ++i)
        {
            for (int j = 0; j < _ySize; ++j)
            {
                if (_tiles[i, j] != null)
                {
                    if (_tiles[i, j].Type == LogicTileType.NOT_SECURE)
                        positions += "("+i+","+j+")";
                }
            }
        }
        Debug.Log(positions);
    }
    public string Serialize()
    {
        LogicLayerJsonWrapper l = new LogicLayerJsonWrapper(this);
        string json = JsonUtility.ToJson(l);
        return json;
    }

    public LogicTile Find(string TileName)
    {
        for(int i = 0; i < _xSize; ++i)
        {
            for (int j = 0; j < _ySize; ++j)
            {
                if (_tiles[i, j] != null)
                { 
                    if (_tiles[i, j].TileTransform != null)
                    {
                        if (_tiles[i, j].TileTransform.name == TileName)
                            return _tiles[i, j];
                    }
                }
            }
        }
        return null;
    }

    public void RegisterOnModify(OnModifyTile del)
    {
        _notify += del;
    }

    public void UnRegisterOnModify(OnModifyTile del)
    {
        _notify -= del;
    }

    

    public List<Vector2> GetNeighbors(int i, int j)
    {
        List<Vector2> list = new List<Vector2>();
        if ((j - 1) >= 0 && j - 1 <= (_ySize - 1))
            list.Add(new Vector2(i,j-1));
        if (j + 1 >= 0 && j + 1 <= (_ySize - 1))
            list.Add(new Vector2(i, j + 1));
        if (i - 1 >= 0 && i - 1 <= (_xSize - 1))
            list.Add(new Vector2(i-1,j));
        if (i + 1 >= 0 && i + 1 <= (_xSize - 1))
            list.Add(new Vector2(i+1, j));

        return list;
    }

    public void ModifyTileTrigger(int x, int y, LogicTileType type, bool canWalk)
    {
        LogicTile tile = _tiles[x, y];
        tile.Type = type;
        tile.CanWak = canWalk;
        if (_notify != null)
            _notify(x,y,tile);
    }

    public void ModifyTileTrigger(int x, int y, LogicTileType type, bool canWalk, Transform t)
    {
        LogicTile tile = _tiles[x, y];
        tile.TileTransform = t;
        ModifyTileTrigger(x,y,type, canWalk);
    }

    public void Add(int x, int y, int tipo)
    {
        Debug.Assert(x < _xSize && x >= 0, "Tile fuera de la layer x " + x + " y " + y + " xSize " + _xSize + " ySize " + YSize);
        Debug.Assert(y < _ySize && y >= 0, "Tile fuera de la layer x " + x + " y " + y + " xSize " + _xSize + " ySize " + YSize);
        _tiles[x, y].Type = (LogicTileType)tipo;
        if(_tiles[x, y].Type == LogicTileType.EMPTY || _tiles[x, y].Type == LogicTileType.WALKABLE || _tiles[x, y].Type == LogicTileType.TRANSPASSABLE)
            _tiles[x, y].CanWak = true;
        else
            _tiles[x, y].CanWak = false;
    }

    public void Add(int x, int y, Transform t, bool edge = false, bool transparent = false)
    {
        Debug.Assert(x < _xSize && x >= 0, "Tile fuera de la layer x " + x + " y " + y + " xSize "+ _xSize + " ySize "+ YSize);
        Debug.Assert(y < _ySize && y >= 0, "Tile fuera de la layer x " + x + " y " + y + " xSize " + _xSize + " ySize " + YSize);

        _tiles[x, y].TileTransform = t;
        //_tiles[x, y].Type = border ? LogicTileType.EDGE: LogicTileType.EMPTY;
        _tiles[x, y].Type = edge ? LogicTileType.EDGE : LogicTileType.EMPTY;
        _tiles[x, y].CanWak = edge ? false : true;
        _tiles[x, y].Transparent = transparent;
    }

    public LogicTile this[int x, int y]
    {
        get {
            if (x < 0 || y < 0 || x >= _xSize || y >= _ySize)
                return null;
            return _tiles[x, y]; 
        }
    }

    public LogicTile Get(int x, int y)
    {
        if (x < 0 || y < 0 || x >= _xSize || y >= _ySize)
            return null;
        return _tiles[x, y];
    }

    public int ZPosition
    {
        get { return _zPosition; }
    }

    public int XSize
    {
        get { return _xSize; }
    }

    public int YSize
    {
        get { return _ySize; }
    }

    public int[] GetTiles()
    {
        int[] tiles = new int[_xSize * _ySize];
        int index = 0;
        for(int i = 0; i < _xSize; ++i)
        {
            for (int j = 0; j < _ySize; ++j)
            {
                tiles[index] = (int)_tiles[i, j].Type;
                index++;
            }
        }
        return tiles;
    }

    

    

    public void MarkBondaries(float tileSize, ref List<Vector3> forbiddenPositions)
    {
        MarkPosition(tileSize, forbiddenPositions, LogicTileType.BONDARY);
    }

    public void MarkNotSecure(float tileSize, ref List<Vector3> forbiddenPositions)
    {
        MarkPosition(tileSize, forbiddenPositions, LogicTileType.NOT_SECURE);
    }

    public void MarkLogicWalls(float tileSize, ref List<Vector3> logicWalss)
    {
        MarkPosition(tileSize, logicWalss, LogicTileType.LOGIC_WALL);
    }

    public void MarkPosition(float tileSize, List<Vector3> positions, LogicTileType mark)
    {
        Vector3 positionLeft = new Vector3(0f, 0f, _zPosition);
        Vector3 positionRight = new Vector3(this._xSize, 0f, _zPosition);
        for (int j = 0; j < positions.Count; ++j)
        {
            int xTile = (int)((positions[j].x + 0.55f) / tileSize);
            int yTile = (int)((positions[j].y) / tileSize);
            _tiles[xTile, yTile].Type = mark;
            _tiles[xTile, yTile].CanWak = false;

        }
    }

    public bool PositionInMap(Vector3 position)
    {
        return position.x >= 0 && position.x < _xSize && position.y >= 0 && position.y < _ySize;
    }

    public void Clear()
    {
        CreateLayer(_xSize, _ySize);
    }

    private void CreateLayer(int xSize, int ySize)
    {
        _xSize = xSize;
        _ySize = ySize;
        _tiles = new LogicTile[xSize, ySize];
        for (int i = 0; i < xSize; ++i)
        {
            for (int j = 0; j < ySize; ++j)
            {
                _tiles[i, j] = new LogicTile();
            }
        }
    }
}

public class LogicMap
{
    private float _tileSize;
    private Dictionary<string, LogicLayer> _layer;
    private bool _isTutorial=false;

    public void CreateMap(float tileSize)
    {
        _layer = new Dictionary<string, LogicLayer>();
        _tileSize = tileSize;
    }

    public bool IsTutorial
    {
        get
        {
            return _isTutorial;
        }
        set
        {
            _isTutorial = value;
        }
    }

    public bool IsInitialized
    {
        get
        {
            return _layer != null && _layer.Count > 0;
        }
    }

    public void Serialize(string layer,string file)
    {
        string text = _layer[layer].Serialize();
        System.IO.File.WriteAllText(file, text);
#if UNITY_EDITOR
        AssetDatabase.Refresh(ImportAssetOptions.Default);
#endif
    }

    public void Load(string layer,string file)
    {
        string text = System.IO.File.ReadAllText(file);

        LogicLayer ll = LogicLayer.CreateFromJSon(text);
        _layer[layer] = ll;
    }

    public int CountLayer
    {
        get { return _layer.Count; }
    }

    public void CreateLayerFromJson(string layerID, string text)
    {
        _layer.Add(layerID, LogicLayer.CreateFromJSon(text));
    }

    public void CreateLayer(string layerID, int xSize, int ySize, int zPosition)
    {
        _layer.Add(layerID, new LogicLayer(xSize, ySize, zPosition));
    }

    public bool ExistLayer(string id)
    {
        return _layer!=null ? _layer.ContainsKey(id) : false;
    }

    public LogicLayer Layer(string id)
    {
        if (_layer == null)
            return null;
        if (_layer.ContainsKey(id))
            return _layer[id];
        else
            return null;
    }

    public void Add(string layerID, Transform t, bool transparent = false)
    {
        Vector3 tile = PositionToTile(t.position);
        int x = (int)tile.x;
        int y = (int)tile.y;

        _layer[layerID].Add(x, y, t, false, transparent);
    }

    public Vector3 PositionToTile(Vector3 position)
    {
        int x = (int)(position.x / _tileSize);
        int y = (int)(position.y / _tileSize);
        int z = (int)(position.z / _tileSize);
        Vector3 v = new Vector3(x, y, z);
        return v;
    }

    public void MarkBondaries(string layerID, ref List<Vector3> forbiddenPositions)
    {
        _layer[layerID].MarkBondaries( _tileSize, ref forbiddenPositions);
    }

    public void MarkNotSecure(string layerID, ref List<Vector3> forbiddenPositions)
    {
        _layer[layerID].MarkNotSecure(_tileSize, ref forbiddenPositions);
    }

    public void MarkLogicWalls(string layerID, ref List<Vector3> logicWalls)
    {
        _layer[layerID].MarkLogicWalls(_tileSize, ref logicWalls);
    }

    public void ClearLayer(string layerID)
    {
        _layer[layerID].Clear();
    }

    public void Clear()
    {
        _layer.Clear();
    }

    public float TileSize
    {
        get { return _tileSize; }
    }
}

[Serializable]
public class LogicLayerJsonWrapper
{
    public int x;
    public int y;
    public int z;
    public int[] _tiles;

    public LogicLayerJsonWrapper()
    {

    }

    public LogicLayer CreateLogicLayer()
    {
        LogicLayer ll = new LogicLayer(x, y, z);
        for(int index = 0; index < _tiles.Length; ++index)
        {
            int i = index / y;
            int j = index % y;
            ll.Add(i, j, _tiles[index]);
        }
        return ll;
    }

    public LogicLayerJsonWrapper(LogicLayer ll)
    {
        x = ll.XSize;
        y = ll.YSize;
        z = ll.ZPosition;
        _tiles = ll.GetTiles();
    }
}
