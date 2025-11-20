using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkGoToOtherGo : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public GameObject visibility;
    // Start is called before the first frame update
    private void Start()
    {
        visibility.SetActive(target.gameObject.activeInHierarchy);
    }

    private void LateUpdate()
    {
        if (target.gameObject.activeInHierarchy)
        {
            this.transform.position = target.transform.position + offset;
            visibility.SetActive(true);
        }
        else
        {
            visibility.SetActive(false);
        }
            
        
    }
}
