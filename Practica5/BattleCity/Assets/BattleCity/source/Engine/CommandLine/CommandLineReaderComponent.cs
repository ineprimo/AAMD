using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CommandLineReaderComponent : MonoBehaviour
{
    CommandLineReader commandLine;
    public string[] comands;
    public string[] descriptions;
    // Start is called before the first frame update
    void Start()
    {
        /*
# QualitySettings.vSyncCount=1
# QualitySettings.vSyncCount=0 es libre pero podemos establecer un framerate fijo.
# Application.targetFrameRate=60
        */

    }
   

    // Update is called once per frame
    void Update()
    {
        commandLine = new CommandLineReader();
        string help = commandLine.GetCommandArgument("-help");
        if (help != null && help.ToLower() == "on")
        {
            for (int i = 0; i < comands.Length; i++)
            {
                string des = "";
                if (i < descriptions.Length)
                    des = descriptions[i];
                GameLogic.Get().Log(comands[i] + ": " + des);
            }
        }
        enabled = false;
    }
}


