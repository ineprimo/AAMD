using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faces : MonoBehaviour
{
    public GameObject front;
    public GameObject up;
    public GameObject down;
    public GameObject left;
    public GameObject right;

    public void Clean()
    {
        GraphicPackable gp = GetComponent<GraphicPackable>();
        if (gp != null)
            DestroyImmediate(gp);
        DestroyImmediate(this);
    }
}
