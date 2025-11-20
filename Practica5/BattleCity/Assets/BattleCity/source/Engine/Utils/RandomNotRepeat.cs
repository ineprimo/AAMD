using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNotRepeat
{
    private int _start;
    private int _end;
    private List<int> randomRange;
    private int lastRandom;
    public RandomNotRepeat(int start, int end)
    {
        if (end <= start)
            throw new Exception("en parámetro end debe ser mayor que start");
        _start = start;
        _end = end;
        randomRange = new List<int>();
        for (int i = start; i < end; i++)
        {
            randomRange.Add(i);
        }
        lastRandom = int.MinValue;
    }

    public void ExcludeValue(int n)
    {
        randomRange.RemoveAt(n);
    }

    public int NextRandom(bool onlyLast = false)
    {
        int n = UnityEngine.Random.Range(0, randomRange.Count);
        int next = randomRange[n];
        if(onlyLast)
        {
            if(lastRandom != int.MinValue)
            {
                randomRange.Add(lastRandom);
            }
            lastRandom=n;
        }

        randomRange.RemoveAt(n);
        return next;
    }

    public int AvailableRandomNumbers
    {
        get { return randomRange.Count; }
    }

    public void Reset()
    {
        randomRange.Clear();
        randomRange = new List<int>();
        for (int i = _start; i < _end; i++)
        {
            randomRange.Add(i);
        }
        lastRandom = int.MinValue;
    }
}
