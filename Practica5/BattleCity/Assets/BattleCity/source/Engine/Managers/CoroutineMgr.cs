using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineMgr : MonoBehaviour
{
    //permite que la coroutina termine independientemente de que el objeto este pausado.
    public Coroutine StarSafeCoroutine(IEnumerator coCallback)
    {
        return StartCoroutine(coCallback);
    }
    
}
