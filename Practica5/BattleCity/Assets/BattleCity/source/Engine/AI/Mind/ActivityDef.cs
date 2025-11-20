using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActionDescriptionAttribute : ActivityDescriptionAttribute
{
    private string[] _output;
    public string[] Output
    {
        get { return _output; }
        set { _output = value; }
    }

    public ActionDescriptionAttribute() : base()
    {

    }

    public ActionDescriptionAttribute(Type linked) : base(linked)
    {

    }
}

public class ConditionDescriptionAttribute : ActivityDescriptionAttribute
{
    public ConditionDescriptionAttribute() : base()
    {

    }

    public ConditionDescriptionAttribute(Type linked) : base(linked)
    {

    }
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public abstract class ActivityDescriptionAttribute: Attribute
{
    private Type _linked;
    private string[] _input;

    public ActivityDescriptionAttribute()
    {

    }

    public ActivityDescriptionAttribute(Type linked)
    {
        _linked = linked;
    }

    public string[] Input
    {
        get { return _input; }
        set { _input = value; }
    }

    public Type LinkedTo
    {
        get { return _linked; }
        set { _linked = value; }
    }

}




