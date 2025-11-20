using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStringVolatile : SetVolatile
{
    public string strData;
    public override void Apply()
    {
        GameMgr.Instance.GetStorageMgr().SetVolatile(section, subsection, strData);
    }
}
