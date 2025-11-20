using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GraphicTile
{
    [Tooltip("Nombre para idenificar el tile pero que no aporta nada en la creación del mismo")]
    public string Name;
    [Tooltip("Modelo del tile")]
    public GameObject[] Prefab;
    [Tooltip("Identificador en el mapa")]
    public int id;
    [Tooltip("Rotación encesaria para que el tile quede correctamente (HACK por problemas de exportación)")]
    public Vector3 rotation;
    [Tooltip("Si queremos que el tile se vea o no (Ocultar tiles de suelo y otras posibles funcionalidades)")]
    public bool visible=false;
    [Tooltip("Algunos tiles son más pequeños o tiene partes transparentes, esto complica el optimizado de caras. Marcar aquellos tiles probleáticos para tenerlo en cuenta en la optimización de caras")]
    public bool transparent = false;
}
