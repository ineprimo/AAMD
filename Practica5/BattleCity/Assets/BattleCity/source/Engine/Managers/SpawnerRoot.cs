using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerRoot : MonoBehaviour
{
    static SpawnerRoot spawnerRoot;

    public static SpawnerRoot Get()
    {
        return Utils.GetSecureStaticGet<SpawnerRoot>(ref spawnerRoot, Globals.SpawnerRoot);
    }

    private void Awake()
    {
        spawnerRoot = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
