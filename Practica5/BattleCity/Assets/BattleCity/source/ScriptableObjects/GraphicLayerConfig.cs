using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GraphicLayer
{
    public string name;
    public int position;
    public bool addToLogic;
    public bool optimizeFaces;
    public int layerDepth = 3;
}

[CreateAssetMenu(fileName = "New GraphicLayerConfig", menuName = "8Picaros/GraphicLayerConfig")]
public class GraphicLayerConfig : ScriptableObject
{
    public GraphicLayer[] graphicLayers;
}
