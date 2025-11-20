using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
public class ReadOnlyInspectorAttribute : PropertyAttribute
{
}



[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
public class ColorInspectorAttribute : PropertyAttribute
{
    public enum AllowedColors { white, red, blue, yellow, grey, magenta, cyan, black, green}
    private UnityEngine.Color color;

    public ColorInspectorAttribute(AllowedColors c)
    {
        color = ConvertColor(c);
    }

    public ColorInspectorAttribute(float r, float g, float b)
    {
        color = new Color(r, g, b);
    }

    public UnityEngine.Color GetColor()
    {
        return color;
    }

    public static UnityEngine.Color ConvertColor(AllowedColors c)
    {
        switch(c)
        {
            case AllowedColors.white:
                return Color.white;
            case AllowedColors.red:
                return new Color(1f,0.4f,0.4f);
            case AllowedColors.blue:
                return Color.blue;
            case AllowedColors.yellow:
                return Color.yellow;
            case AllowedColors.magenta:
                return Color.magenta;
            case AllowedColors.cyan:
                return Color.cyan;
            case AllowedColors.black:
                return Color.black;
            case AllowedColors.green:
                return Color.green;
            default:
                return Color.white;
        }
    }
}

[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
public class DemoAttribute : ColorInspectorAttribute
{
    public DemoAttribute() : base(ColorInspectorAttribute.AllowedColors.yellow)
    {

    }
}

[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
public class ProgrammingAttribute : ColorInspectorAttribute
{
    public ProgrammingAttribute() : base(ColorInspectorAttribute.AllowedColors.red)
    {

    }
}

[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
public class HelperAttribute : ColorInspectorAttribute
{
    public HelperAttribute() : base(ColorInspectorAttribute.AllowedColors.green)
    {

    }
}