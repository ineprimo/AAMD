using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//
[RequireComponent(typeof(Text))]
public class LocalizeTextParams : MonoBehaviour
{
    public LocalizeInspectorStr texto;
    public string[] parameters;
    //public string textoPrueba;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = LocalizationMgr.Instance.Translate(texto.key, parameters);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
