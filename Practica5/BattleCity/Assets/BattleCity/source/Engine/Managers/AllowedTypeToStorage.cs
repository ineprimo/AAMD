using UnityEngine;
using System;
using System.Linq;
using System.Text;

/// <summary>
/// Tipos permitidos en el alamcenamiento persistente.
/// </summary>

[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public class AllowedTypeToStorage : System.Attribute
{
    public readonly System.Type m_type;

    public AllowedTypeToStorage(System.Type type)
    {
        m_type = type;
    }
}
