using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Labels : MonoBehaviour
{
    public string[] labels;
    public bool toLower;
    private HashSet<string> _hashLabels;

    void Awake()
    {
        _hashLabels = new HashSet<string>();
        for(int i = 0; i < labels.Length; i++)
        {
            _hashLabels.Add(toLower ? labels[i].ToLower() : labels[i]);
        }
    }

    public bool ContainLabel(string label)
    {
        return _hashLabels.Contains(toLower ? label.ToLower() : label);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
