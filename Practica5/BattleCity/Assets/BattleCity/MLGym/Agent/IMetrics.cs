using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMetrics
{
    object GetData();
    void Init();
    void End();
}
