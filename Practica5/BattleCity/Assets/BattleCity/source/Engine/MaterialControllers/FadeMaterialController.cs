using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeMaterialController : MonoBehaviour
{
    public Renderer rendererComp;
    private Color defaultColor;

    private void Awake()
    {
        defaultColor = rendererComp.material.color;
    }

    public void ChangeLayer(int layer, out int originalLayer)
    {
        originalLayer = rendererComp.gameObject.layer;
        rendererComp.gameObject.layer = layer;
    }


    public float Fade
    {
        set
        {
            Color c = rendererComp.material.color;
            c.a = value;
            rendererComp.material.color = c;
        }

        get
        {
            Color c = rendererComp.material.color;
            return c.a;
        }
    }

    public void ResetColor()
    {
        rendererComp.material.color = defaultColor;
    }

}
