using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SetVolatile : MonoBehaviour
{
    public string section;
    public string subsection;

    public abstract void Apply();
}
