using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LocalizationTextStr : MonoBehaviour
{
    public LocalizeInspectorStr texto;
    //public string textoPrueba;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = LocalizationMgr.Instance.Translate(texto.key);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
