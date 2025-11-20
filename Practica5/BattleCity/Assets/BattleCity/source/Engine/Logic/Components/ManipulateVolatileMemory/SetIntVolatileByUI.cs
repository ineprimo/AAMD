using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetIntVolatileByUI : SetIntVolatile
{
    public virtual void ChangeInValue(int v)
    {
        this.intData = v;
    }
}
