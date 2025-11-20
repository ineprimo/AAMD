using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapProperties {


    protected abstract Properties[] Properties { get; set; }
    protected Dictionary<string, Properties> _propertiesDict;

    public int getPropertyInt(string name)
    {
        Dictionary<string, Properties> dictionary = GetDictionary();
        Debug.Assert(dictionary != null, "El elemento no tiene propiedades asociadas, falló al intentar encontrar la propiedad " + name);
        if (dictionary.ContainsKey(name))
        {
            Properties p = dictionary[name];
            Debug.Assert(p != null, "Property " + name + " not found ");
            Debug.Assert(p.type == "int", "Property " + name + " is not int ");
            return Convert.ToInt32(p.value);
        }
        else // buscar e nlso atributos por defecto
            Debug.LogError("El atributo "+name+" no se encuentra en el diccionario");
        return -1;
    }

    public float getPropertyFloat(string name)
    {
        Properties p = GetDictionary()[name];
        Debug.Assert(p != null, "Property " + name + " not found ");
        Debug.Assert(p.type == "float", "Property " + name + " is not float ");
        string strVal = p.value;
        if (strVal.Contains("."))
        {
            strVal = strVal.Replace(".", ",");
        }
        float v = (float) Convert.ToDouble(strVal);
        return v;
    }

    public string getProperty(string name)
    {
        Properties p = GetDictionary()[name];
        Debug.Assert(p != null, "Property " + name + " not found ");
        Debug.Assert(p.type == "string", "Property " + name + " is not string ");
        return p.value;
    }

    public bool getPropertyBool(string name)
    {
        Properties p = GetDictionary()[name];
        Debug.Assert(p != null, "Property " + name + " not found ");
        Debug.Assert(p.type == "bool", "Property " + name + " is not bool ");
        return p.value.ToLower() == "true" ? true : false;
    }

    public object getProperty(System.Type t, string name)
    {
        if (t.Equals(typeof(int)))
            return (object)getPropertyInt(name);
        else if (t.Equals(typeof(float)))
            return (object)getPropertyFloat(name);
        else if (t.Equals(typeof(string)))
            return (object)getProperty(name);
        else if (t.Equals(typeof(bool)))
            return (object)getPropertyBool(name);
        else
        {
            Debug.LogError("The type " + t + " is not allowed ");
            return null;
        }

    }

    public bool PropertyExist(String name)
    {
        if (GetDictionary() == null)
            return false;
        return GetDictionary().ContainsKey(name);
    }

    public int NumProperties
    {
        get { return Properties == null ? 0 :  Properties.Length; }
    }

    protected Dictionary<string, Properties> GetDictionary()
    {
        if(_propertiesDict == null)
        {
            _propertiesDict = CreateDictionary();
        }

        return _propertiesDict;
    }

    protected Dictionary<string, Properties> CreateDictionary()
    {
        if (Properties == null)
            return null;

        Dictionary<string, Properties> dictionary = new Dictionary<string, Properties>();
        for (int i = 0; i < Properties.Length; ++i)
        {
            dictionary.Add(Properties[i].name, Properties[i]);
        }

        return dictionary;
    }
}
