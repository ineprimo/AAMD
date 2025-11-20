using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class VersionInfo : MonoBehaviour
{
    public TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        String.Format(text.text, Application.version + " " + Application.buildGUID, " [Fecha aun no establecida] ");
    }

}
