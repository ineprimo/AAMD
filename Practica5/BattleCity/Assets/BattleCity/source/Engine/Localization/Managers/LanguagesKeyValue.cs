using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguagesKeyValue 
{
    private string key;
    private string value;

    public LanguagesKeyValue(string k, string v)
    {
        key = k;
        value = v;
    }

    public string Key { get => key; set => key = value; }
    public string Value { get => value; set => this.value = value; }
}
