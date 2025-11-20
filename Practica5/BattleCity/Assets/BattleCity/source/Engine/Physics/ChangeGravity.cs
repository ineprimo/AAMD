using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGravity : MonoBehaviour
{
    [SerializeField]
    protected Vector3 gravity;
    public bool startInit=true;
    [SerializeField]
    [ReadOnlyInspector]
    private Vector3 defaultGravity;
    // Start is called before the first frame update

    public Vector3 Gravity
    {
        get
        {
            return gravity;
        }

        set
        {
            gravity = value;
            Physics.gravity = gravity;
        }
    }

    public void Restore()
    {
        Physics.gravity = defaultGravity;
    }

    public void SetNewGravity()
    {
        Physics.gravity = gravity;
    }
    void Start()
    {
        defaultGravity = Physics.gravity;
        if(startInit)
        SetNewGravity();
    }
}
