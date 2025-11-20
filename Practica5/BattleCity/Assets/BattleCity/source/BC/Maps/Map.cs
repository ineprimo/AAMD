using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : IMap
{
    const int EMPTY = 0;
    const int BRICK = 1;
    const int UNBREKABLE_1 = 2;
    const int UNBREKABLE_2 = 41;
    const int COMMAN_CENTER = 3;
    const int SEMI_BREKABLE_MIN = 4;
    const int SEMI_BREKABLE_MAX = 7;
    const int SEMI_UNBREKABLE_MIN = 8;
    const int SEMI_UNBREKABLE_MAX = 11;

    const int PLAYER = 49;
    const int AGENT = 50;
    const int NATIVE_AGENT_1 = 57;
    const int NATIVE_AGENT_2 = 58;

    TankPerception.INPUT_TYPE[] map;
    int xSize;
    int ySize;
    int xSizeInEnvironment;
    int ySizeInEnvironment;
    int playerSpawnTile;
    HashSet<int> agentSpawnTiles;
    HashSet<int> nativeAgentSpawnTiles;


    VisualMap visualMap;

    public Map(TileMap tilemap, string layerName, int xSizeEnv, int ySizeEnv)
    {
        agentSpawnTiles = new HashSet<int>();
        nativeAgentSpawnTiles = new HashSet<int>();
        xSize = tilemap.Width;
        ySize = tilemap.Height;
        xSizeInEnvironment = xSizeEnv;
        ySizeInEnvironment = ySizeEnv;
        map = new TankPerception.INPUT_TYPE[xSize* ySize];
        int[] layerData = tilemap[layerName].Data;
        for (int i = 0; i < layerData.Length; ++i)
        {
            if(layerData[i] == EMPTY)
            {
                map[i] = TankPerception.INPUT_TYPE.NOTHING;
            }
            else if(layerData[i] == BRICK)
            {
                map[i] = TankPerception.INPUT_TYPE.BRICK;
            }
            else if(layerData[i] == UNBREKABLE_1 || layerData[i] == UNBREKABLE_2)
            {
                map[i] = TankPerception.INPUT_TYPE.UNBREAKABLE;
            }
            else if (layerData[i] == COMMAN_CENTER)
            {
                map[i] = TankPerception.INPUT_TYPE.COMMAND_CENTER;
            }
            else if (layerData[i] >= SEMI_BREKABLE_MIN && layerData[i] <= SEMI_BREKABLE_MAX)
            {
                map[i] = TankPerception.INPUT_TYPE.SEMI_BREKABLE;
            }
            else if (layerData[i] >= SEMI_UNBREKABLE_MIN && layerData[i] <= SEMI_UNBREKABLE_MAX)
            {
                map[i] = TankPerception.INPUT_TYPE.SEMI_UNBREKABLE;
            }
            else
            {
                if (layerData[i] == PLAYER)
                    playerSpawnTile = i;
                else if (layerData[i] == AGENT)
                    agentSpawnTiles.Add(i);
                else if (layerData[i] == NATIVE_AGENT_1 || layerData[i] == NATIVE_AGENT_2)
                    nativeAgentSpawnTiles.Add(i);
                map[i] = TankPerception.INPUT_TYPE.NOTHING;
            }
        }

    }

    public int PlayerSpawnTile
    {
        get
        {
            return playerSpawnTile;
        }
    }

    public bool IsTileAAgentSpawnPosition(int i)
    {
        return agentSpawnTiles.Contains(i);
    }

    public bool IsTileANativeAgentSpawnPosition(int i)
    {
        return nativeAgentSpawnTiles.Contains(i);
    }

    public int Lenght
    {
        get
        {
            return map.Length;
        }
    }

    public int XSize
    {
        get
        {
            return xSize;
        }
    }

    public int YSize
    {
        get
        {
            return ySize;
        }
    }

    public int XSizeEnv
    {
        get
        {
            return xSizeInEnvironment;
        }
    }

    public int ySizeEnv
    {
        get
        {
            return ySizeInEnvironment;
        }
    }

    public void UpdateTile(int x, int y, TankPerception.INPUT_TYPE newType)
    {
        int position = ConvertPosition(x,y);
        map[position] = newType;
    }

    public void UpdateTile(Vector2Int pos, TankPerception.INPUT_TYPE newType, bool updateVisualMap = true)
    {
        int position = ConvertPosition(pos.x, pos.y);
        map[position] = newType;
        if(updateVisualMap)
            UpdateVisualMap();
    }

    public Vector2Int GetTile(Vector3 position)
    {
        int x = (int) (position.x / xSizeInEnvironment);
        int y = (int) (position.y / ySizeInEnvironment);
        y = ySize - y - 1;
        return new Vector2Int(x,y);
    }

    public Vector3 TileToWorldPosition(Vector2Int position, float z)
    {
        float x = position.x * xSizeInEnvironment;
        float y = (ySize - position.y - 1) * ySizeInEnvironment;
        //y = ySize - y - 1;
        return new Vector3(x, y,z) +new Vector3(1f,1f,0F);
    }

    public TankPerception.INPUT_TYPE this[int x, int y]
    {
        get
        {
            int pos = ConvertPosition(x,y);
            return map[pos];
        }
    }

    public TankPerception.INPUT_TYPE this[int pos]
    {
        get
        {
            return map[pos];
        }
    }

    public int ConvertPosition(int x, int y)
    {
        return y * xSize + x;
    }

    public int ConvertPosition(Vector2Int pos)
    {
        return pos.y * xSize + pos.x;
    }

    public Vector2Int ConvertPosition(int pos)
    {
        int x = pos % xSize; // 3x3 => 1;  4 debe ser 1x1:  4 % 3 = 1
        int y = pos / ySize; // 4 / 3 = 1
        return new Vector2Int(x, y);
    }

    public void SetVisualMap(VisualMap v)
    {
        visualMap = v;
        visualMap.UpdateGrid(this);
    }

    public VisualMap VisualMap
    {
        get
        {
            return visualMap;
        }
    }

    public void UpdateVisualMap()
    {
        visualMap.UpdateGrid(this);
    }

    public string Serialize()
    {
        string s = "";
        for(int i = 0; i < map.Length-1; i++)
        {
            s += "" + ((int)map[i]) + ";";
        }
        s += "" + ((int)map[map.Length - 1]);
        return s;
    }
}
