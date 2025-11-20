using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachableToPhysic : MonoBehaviour
{
    private bool _assigned;

    public bool IsAssigned
    {
        get
        {
            return _assigned;
        }

        set
        {
            _assigned = value;
        }
    }
}
