using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerStayListener : MonoBehaviour
{
    public string[] tags;
    public System.Action<Collider, GameObject> OnTriggerStayAction;

    private HashSet<string> _tagSet;
    private void Start()
    {
        _tagSet = new HashSet<string>();
        for (int i = 0; i < tags.Length; ++i)
        {
            _tagSet.Add(tags[i]);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (_tagSet == null)
            return;
        if (_tagSet.Contains(other.tag) && OnTriggerStayAction != null)
        {
            OnTriggerStayAction(other, this.gameObject);
        }
    }
}
