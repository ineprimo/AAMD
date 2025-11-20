using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerEnterListener : MonoBehaviour
{
    public string[] tags;
    public System.Action<Collider,GameObject, bool> OnTriggerEnterAction;
    public bool throwEventWithoutTag = false;
    public bool falling = false;

    private HashSet<string> _tagSet;
    private void Start()
    {
        _tagSet = new HashSet<string>();
        for(int i = 0; i < tags.Length; ++i)
        {
            _tagSet.Add(tags[i]);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (OnTriggerEnterAction == null)
            return;
        if ((_tagSet == null || _tagSet.Count == 0) && throwEventWithoutTag)
            OnTriggerEnterAction(other, this.gameObject, falling);
        else if ((_tagSet == null || _tagSet.Count == 0) && !throwEventWithoutTag)
            return;
        else if (_tagSet.Contains(other.tag) && OnTriggerEnterAction != null)
        {
            OnTriggerEnterAction(other, this.gameObject, falling);
        }
    }
}
