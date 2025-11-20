using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tickable : MonoBehaviour {

    [Tooltip("Selecciona donde debe ejecutarse el tick del componente")]
    public Globals.TickType _tickUpdate = Globals.TickType.FIXED_UPDATE;

    // Update is called once per frame
    protected virtual void Update()
    {
        if (_tickUpdate == Globals.TickType.UPDATE)
            Tick(TimeScale.deltaTime);
    }

    protected virtual void FixedUpdate()
    {
        if (_tickUpdate == Globals.TickType.FIXED_UPDATE)
            Tick(TimeScale.fixedDeltaTime);
    }

    protected virtual void LateUpdate()
    {
        if (_tickUpdate == Globals.TickType.LATE_UPDATE)
            Tick(TimeScale.deltaTime);

    }

    protected abstract void Tick(float time);

}
