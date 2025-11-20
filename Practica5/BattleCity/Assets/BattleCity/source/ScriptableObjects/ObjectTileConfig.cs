using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New ObjectTileConfig", menuName = "8Picaros/ObjectTileConfig")]
public class ObjectTileConfig : ScriptableObject
{
    [Tooltip("Collección de objetos que se instancian en la escena")]
    public ObjectTile[] objectTiles;
    public TextAsset[] objectConfig;
}
