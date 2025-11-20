using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTickable : MonoBehaviour
{
    protected System.Action<float> OnUpdateAction;

    public void AddUpdate(System.Action<float> on)
    {
        OnUpdateAction += on;
    }

    public void RemoveUpdate(System.Action<float> on)
    {
        OnUpdateAction += on;
    }
}
