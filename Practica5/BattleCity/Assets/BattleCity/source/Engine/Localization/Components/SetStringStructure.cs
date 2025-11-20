using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStringStructure : MonoBehaviour
{
    public LocalizeTextPro[] StringStructure;


    // Start is called before the first frame update
    void Start()
    {
        for (int i=0; i < StringStructure.Length; ++i)
        {
            string text = StringStructure[i].GetText.text;
            
            if (text == null || text == "")
            {
                StringStructure[i].gameObject.SetActive(false);
            }
        }
    }

}
