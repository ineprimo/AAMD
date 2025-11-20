using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTickableUpdate : AutoTickable
{
    // Update is called once per frame
    void Update()
    {
        OnUpdateAction?.Invoke(Time.deltaTime);
        _Update();
    }
    protected virtual void _Update() { }
}
