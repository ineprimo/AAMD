using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetIntVolatile : SetVolatile
{
    public int intData;
    public override void Apply()
    {
        GameMgr.Instance.GetStorageMgr().SetVolatile(section, subsection, intData);
    }
}
