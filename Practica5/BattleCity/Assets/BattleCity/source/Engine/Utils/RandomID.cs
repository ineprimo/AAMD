using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomID
{
    static HashSet<int> _randomsIDCreated = new HashSet<int>();

    public static int GetID
    {
        get
        {
            bool completed = false;
            int id = 0;
            do
            {
                id = Random.Range(int.MinValue, int.MaxValue);
                completed = !_randomsIDCreated.Add(id);
            } while (!completed);
            return id;
        }
    }

    public static void Reset()
    {
        _randomsIDCreated.Clear();
    }

    public static bool IsIDGenerated(int id)
    {
        return _randomsIDCreated.Contains(id);
    }
}
