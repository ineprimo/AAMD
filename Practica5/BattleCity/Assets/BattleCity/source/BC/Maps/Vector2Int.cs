using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Vector2Int 
{
    public int x;
    public int y;
    public Vector2Int(int aX, int aY)
    {
        x = aX;
        y = aY;
    }

    public static Vector2Int operator +(Vector2Int a) => a;
    public static Vector2Int operator -(Vector2Int a) => new Vector2Int(-a.x, -a.y);

    public static Vector2Int operator +(Vector2Int a, Vector2Int b)
        => new Vector2Int(a.x + b.x, a.y + b.y);

    public static Vector2Int operator -(Vector2Int a, Vector2Int b)
        => new Vector2Int(a.x - b.x, a.y - b.y);

    public static Vector2Int operator *(Vector2Int a, Vector2Int b)
        => new Vector2Int(a.x * b.x, a.y * b.y);

    public static Vector2Int operator /(Vector2Int a, Vector2Int b)
    {
        if (b.x == 0 || b.y == 0)
        {
            throw new DivideByZeroException();
        }
        return new Vector2Int(a.x / b.x, a.y / b.y);
    }

    public static Vector2Int operator *(Vector2Int a, int b)
    => new Vector2Int(a.x * b, a.y * b);

    public static Vector2Int operator /(Vector2Int a, int b)
    {
        if (b == 0)
        {
            throw new DivideByZeroException();
        }
        return new Vector2Int(a.x / b, a.y / b);
    }

    public static Vector2Int Zero
    {
        get
        {
            return new Vector2Int(0, 0);
        }
    }

    public float Magnitude
    {
        get
        {
            return Mathf.Sqrt(x * x + y * y);
        }
    }

    public static float Distance(Vector2Int a, Vector2Int b)
    {
        Vector2Int direction = b - a;
        return direction.Magnitude;
    }
}
