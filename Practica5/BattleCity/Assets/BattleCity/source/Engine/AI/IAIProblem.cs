using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAIProblem<T> where T : AINode<T>
{
    T Init
    {
        get; set;
    }

    bool IsASolution(T node);
    float Heuristic(T node);
    List<T> GetNeighbors(T node);
    float GetGCostBetween(T from, T to);
}
