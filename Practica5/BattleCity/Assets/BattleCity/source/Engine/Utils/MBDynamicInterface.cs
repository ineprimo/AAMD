using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IEnemyExplosive
{
    void Explosion();
}


public abstract class MBDynamicInterface<T> : MonoBehaviour
{
    public abstract T GetInterface();
}
