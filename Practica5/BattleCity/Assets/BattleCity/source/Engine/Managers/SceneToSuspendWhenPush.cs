using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneToSuspendWhenPush : MonoBehaviour
{
    public string sceneName;
    // Start is called before the first frame update
    void Awake()
    {
        sceneName = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
