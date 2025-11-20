using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boot : MonoBehaviour
{
    public string[] scenes;

    public string[] multiplayerScenes;
    CommandLineReader commandLine;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        commandLine = new CommandLineReader();
        string level = commandLine.GetCommandArgument("-level");
        string mode = commandLine.GetCommandArgument("-mode");
        if(mode != null || mode == "multi" || mode == "multiplayer" || mode == "training" || mode == "train")
        {
            ChangeScene(level, multiplayerScenes);
        }
        else if(mode == null || mode == "single" || mode == "singleplayer")
        {
            ChangeScene(level, scenes);
        }
    }

    public void ChangeScene(string level, string[] a_scenes)
    {
        if (level == null || level == "1")
        {
            GameMgr.Instance.GetServer<SceneMgr>().ChangeScene(a_scenes[0]);
        }
        else if (level != null && level == "-1") // random
        {
            string s = a_scenes[Random.Range(0, a_scenes.Length)];
            GameMgr.Instance.GetServer<SceneMgr>().ChangeScene(s);
        }
        else
        {
            int levelID = int.Parse(level.Trim());
            GameMgr.Instance.GetServer<SceneMgr>().ChangeScene(a_scenes[levelID - 1]);
        }
    }
}
