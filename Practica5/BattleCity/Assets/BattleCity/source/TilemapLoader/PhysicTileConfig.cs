using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New PhysicTileConfig", menuName = "8Picaros/PhysicTileConfig")]
public class PhysicTileConfig : ScriptableObject
{
    [Tooltip("Colección de tiles físicos")]
    public PhysicTile[] physicTiles;
    [Tooltip("Tamaño del tile fisico")]
    public float physicTileSize;
    [Tooltip("Profundidad de la capa")]
    public float Zposition;

    //[Header("Sección donde añadiremos configuraciones especiales a los tiles")]

}
