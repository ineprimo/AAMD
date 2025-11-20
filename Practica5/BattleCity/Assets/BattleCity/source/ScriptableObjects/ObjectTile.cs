using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectTile
{
    [Tooltip("Tipo en el tiled")]
    public string type;
    [Tooltip("Prefab del object que instanciamos")]
    public GameObject[] prefab;
    [Tooltip("Rotación del tile para corregir errores en la exportación")]
    public Vector3 rotation;
    [Tooltip("Marcamos si queremos que este tile se visualice o no")]
    public bool visible;
    [Tooltip("Algunos tiles necesitan ser postprocesaados. Por ejemplo los tiles moviles. Si es así marca esta casilla")]
    public bool needPostprocessing;
    [Tooltip("Objetos únicos que una vez cogidos no vuelven a aparecer")]
    public bool collectable;
}

[System.Serializable]
public class ObjectData
{
    [Tooltip("Tipo en el tiled")]
    public string type;
    [Tooltip("Identificador del tile")]
    public int idTile;
    [Tooltip("Propiedades del tile")]
    public ObjectProperty[] objectProperties;
    [Tooltip("tileset del que proviene")]
    public string teleset;
}

[System.Serializable]
public class ObjectProperty
{
    [Tooltip("nombre de la property")]
    public string name;
    [Tooltip("Tipo del a property")]
    public string type;
    [Tooltip("Valor de la property")]
    public string value;
}
