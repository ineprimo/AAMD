using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocalizeTextPro : MonoBehaviour
{
    [Header("Texto")]
    public LocalizeInspectorPopup texto;
    [Tooltip("Esta clave es la misma que el desplegable, pero en modo texto")]
    public string key;
    public bool preferKey = false;
    public bool refreshEachFrame = false;
    [HideInInspector]
    public int dirty;
    private TMP_Text text;
    //public string textoPrueba;


    public TMP_Text GetText
    {
        get
        {
            if (text == null)
                text = GetComponent<TMP_Text>();
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
        if (!refreshEachFrame)
            this.enabled = false;
    }

    public void Refresh()
    {
        if (key != null && key != "" && key != texto.key && preferKey)
            texto.key = key;

        string s = LocalizationMgr.Instance.Translate(texto.key);
        GetText.text = s;
    }
}

