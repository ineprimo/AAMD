using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static TMPro.TMP_Dropdown;

public class SetIntVolatileByDropdown : SetIntVolatileByUI
{
    public TMP_Dropdown dropdown;
    public bool setVolatile = false;
    public override void ChangeInValue(int v)
    {
        OptionData od = dropdown.options[v];
        string option = od.text.ToLower();
        if (option == "default")
            setVolatile = false;
        else
        {
            setVolatile = true;
            intData = int.Parse(option);
        }
    }

    public override void Apply()
    {
        if (setVolatile)
            base.Apply();
    }
}
