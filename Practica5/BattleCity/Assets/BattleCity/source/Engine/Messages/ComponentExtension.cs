using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ComponentExtension {

    public static MessageMgr GetMessageMgr(this GameObject go)
    {
        MessageMgr msgMg = go.GetComponent<MessageMgr>();
        if (msgMg == null)
            msgMg = go.AddComponent<MessageMgr>();
        return msgMg;
    }

}
