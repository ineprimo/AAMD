using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFloatVolatile : SetVolatile
{
    public float floatData;
    public override void Apply()
    {
        GameMgr.Instance.GetStorageMgr().SetVolatile(section, subsection, floatData);
    }
}
