using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class ClassTileMapAsignableAttributeAttribute : System.Attribute {


}


[AttributeUsage(AttributeTargets.Field, Inherited = true)]
public class FieldTileMapAsignableIDAttribute : System.Attribute
{

}

[AttributeUsage(AttributeTargets.Field, Inherited = true)]
public class FieldTileMapAsignableAttributeAttribute : System.Attribute
{

    private string _name;
    private bool _required = false;

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public bool Required
    {
        get { return _required; }
        set { _required = value; }
    }
}