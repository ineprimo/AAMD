using System.Collections.Generic;
using UnityEngine;
using System;

public class AStar<T> where T : AINode<T>
{

    private IAIProblem<T> _problem;
    private OrderList<T> _open;
    private HashSet<T> _precessed;
    List<T> l;



    public AStar()
    {
        _open = new OrderList<T>();
        _precessed = new HashSet<T>();
    }

    public void SetTheProblemToSolve(IAIProblem<T> problem)
    {
        _problem = problem;
    }

    public bool CalculateSolution(out List<T> path, out T closets)
    {
        bool findGoal = false;
        closets = _problem.Init;
        float minCost = float.MaxValue;
        _open.Clear();
        _precessed.Clear();
        path = null;
        Debug.Assert(_problem.Init != null, "Error: el nodo inicial debe ser distinto de null");
        SetAINodeCosts(_problem.Init, null, 0f);
        AppendIntoOpenList(_problem.Init);
        try
        {
            while (_open.Count > 0 && !findGoal)
            {
                T current = ExtractFromOpenList();

                if (!_problem.IsASolution(current))
                {
                    if (!current.IsClose) //si ya ha sido procesado, lo ignoramos.
                    {
                        current.SetClose(); //si no lo marcamos como procesado
                        List<T> neighbors = _problem.GetNeighbors(current); //Generamos los sucesores.
                        for (int i = 0; i < neighbors.Count; ++i)
                        {
                            T child = neighbors[i];
                            float tentativeGCost = current.GCost + _problem.GetGCostBetween(current, child);
                            if (tentativeGCost < child.GCost) //Este camino es mejor que el anterior modificamos su coste
                            {
                                SetAINodeCosts(child, current, tentativeGCost);

                                if (current.HCost < minCost)
                                {
                                    minCost = current.HCost;
                                    closets = current;
                                }
                            }
                            //no puedo insertar hasta no tener el coste calculado porque los inserto ordenadamente.
                            if (child.IsUndiscovered) //si el nodo no estaba en abierta ni en cerrada lo añadimos a abierta
                                AppendIntoOpenList(child);
                        }
                    }
                }
                else //si es una solución
                {
                    current.SetClose(); //lo marccamos como procesado (mera formalidad)
                    findGoal = true;
                    closets = current;
                    path = ReconstructPath(current);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError(" Error en la ejecución dle hilo "+e.Message);
        }

        return findGoal;
    }

    public void ResetTheProblem()
    {
        foreach(AINode<T> node in _precessed)
        {
            node.Reset();
        }
    }


    protected List<T> ReconstructPath(T target)
    {
        List<T> path = new List<T>();
        T next = target;
        do
        {
            path.Add(next);
            next = (T)next.Parent;
        } while (next != null);
        path.Reverse();
        return path;
    }

    protected void AppendIntoOpenList(T node)
    {
        Debug.Assert(!node.IsClose, "Error, se ha intentado añadir a la lista de abiertas un nodo ya cerrado");
        _open.InsertInOrder(node);
        _precessed.Add(node);
        node.SetOpen();
    }

    protected T ExtractFromOpenList()
    {
        T current = _open.ExtractFirst();
        return current;
    }

    protected void SetAINodeCosts(T node, T parent, float g)
    {
        node.GCost = g;
        node.HCost = _problem.Heuristic(node);
        node.Parent = parent;
    }
}
