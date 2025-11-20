using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PatternSet
{
    [Tooltip("Nombre del conjunto de patrones")]
    public string name;
    [Tooltip("Collección de tiles de este conjunto de patrones")]
    public GraphicTile[] graphicTiles;
    [Tooltip("Offset a partir de la cual se calcula el id de esta capa de tiles en Tiled")]
    public int offset;
}
[CreateAssetMenu(fileName = "New Biome", menuName = "8Picaros/Biome")]
public class Biome : ScriptableObject
{
    [Tooltip("Collección de tiles gráficos del Bioma")]
    public PatternSet[] patters;
    [Tooltip("Collección de tiles de probs")]
    public PatternSet[] patternProps;
}
