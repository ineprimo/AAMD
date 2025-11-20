using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandLineReader
{
    private string[] _args;
    private Dictionary<string, int> _indexArgs;
    public CommandLineReader()
    {
        _args = System.Environment.GetCommandLineArgs();
        _indexArgs = new Dictionary<string, int>();
        for (int i = 0; i < _args.Length; i++)
        {
            _indexArgs.Add(_args[i],i);
        }
    }

    public bool ExistCommand(string command)
    {
        return _indexArgs.ContainsKey(command);
    }

    public string GetCommandArgument(string command)
    {
        if(ExistCommand(command))
        {
            int index = _indexArgs[command];
            return _args[index+1];
        }
        return null;
    }

    public string[] GetCommandArgument(string command, int numArguments)
    {
        
        if (ExistCommand(command))
        {
            string[] output = new string[numArguments];
            int index = _indexArgs[command];
            int count = 0;
            for(int i = index+1; i < (index+numArguments); i++)
            {
                output[count++] = _args[i];
            }
            return output;
        }
        return null;
    }

    public string[] AllComands
    {
        get
        {
            return _args;
        }
    }
}
