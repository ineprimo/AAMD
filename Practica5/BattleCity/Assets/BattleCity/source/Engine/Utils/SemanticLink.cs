using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Componente que conecta dos gameobjects de forma semántica. Es decir, que si ambos gameobjects tienen un componente SemanticLink del mismo tipo
/// esto nos indica que estan vinculados al mismo objeto o a la misma funcionalidad. Se ha utilizado para identificar en Fulvinter devices que están 
/// asociados a la misma mecánica.
/// </summary>
public class SemanticLink : MonoBehaviour
{
    public string semanticID;
    public int semanticIDInt;
}
