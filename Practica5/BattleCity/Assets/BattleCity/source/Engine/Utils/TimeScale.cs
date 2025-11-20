using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeScale {

    public static float deltaTime
    {
        get { return Time.deltaTime; }
    }

    public static float fixedDeltaTime
    {
        get { return Time.fixedDeltaTime; }
    }
}
