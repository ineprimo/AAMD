using UnityEngine;
using System.Collections.Generic;

public class Exit : MonoBehaviour
{
    public List<string> tags;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (tags == null || tags.Contains(other.tag))
        {
            GameLogic.Get().TryToWin();
        }
    }
}
