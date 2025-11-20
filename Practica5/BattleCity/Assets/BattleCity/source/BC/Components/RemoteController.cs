using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteController : MonoBehaviour
{
    private bool isPause=false;
    public void Pause()
    {
        enabled = false;
        isPause = true;
    }

    public void Init()
    {
        isPause = false;
        enabled = true;
    }
}
