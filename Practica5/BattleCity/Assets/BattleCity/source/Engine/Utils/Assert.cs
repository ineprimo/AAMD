#define HOT_PROOF
using UnityEngine;
using System.Collections;
using System;
//Imitacion del assert de c++
public class Assert
{

    public static void AbortIfNot(bool condition, string msg)
    {
        #if DEBUG || HOT_PROOF
        if (!condition) 
		{
			Debug.LogError("Assert Error! :"+msg);
			throw new Exception(msg);
		}
        #endif
    }
}
