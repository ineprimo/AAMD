using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExportVersion : MonoBehaviour
{
    //[HideInInspector]
    [ReadOnlyInspector]
    public int version;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this);
    }
}
