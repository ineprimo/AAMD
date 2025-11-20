using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pruebas : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Vector3 v = Vector3.forward;

        Vector3 vopp = Vector3.up;

        float f = Vector3.Dot(v.normalized, vopp.normalized);
        Debug.Log("dot product " + f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
