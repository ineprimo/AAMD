using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//
[RequireComponent(typeof(Text))]
public class LocalizeText : MonoBehaviour
{
    [Header("Texto")]
    public LocalizeInspectorPopup texto;
    public bool refreshEachFrame = false;
    [Helper]
    [Tooltip("Este atributo no tiene utilidad a nivel logico, solo sirve para buscar el contenido en el pop")]
    public string key;
    private Text text;
    //public string textoPrueba;


    public Text GetText
    {
        get
        {
            if(text == null)
                text = GetComponent<Text>();
            return text;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        text = GetText;
    }

    // Update is called once per frame
    void Update()
    {
        Refresh();
        if(!refreshEachFrame)
            this.enabled = false;
    }

    public void Refresh()
    {
        string s = LocalizationMgr.Instance.Translate(texto.key);
        GetText.text = s;
    }
}
