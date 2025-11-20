using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCollisionEnterListener : MonoBehaviour
{
    public string[] tags;
    public UnityEvent<Collision, GameObject, bool> OnCollisionEnterAction;
    public bool throwEventWithoutTag = false;
    public bool falling = false;

    private HashSet<string> _tagSet;
    private void Start()
    {
        _tagSet = new HashSet<string>();
        for (int i = 0; i < tags.Length; ++i)
        {
            _tagSet.Add(tags[i]);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (OnCollisionEnterAction == null)
            return;
        if ((_tagSet == null || _tagSet.Count == 0) && throwEventWithoutTag)
            OnCollisionEnterAction?.Invoke(collision, this.gameObject, falling);
        else if ((_tagSet == null || _tagSet.Count == 0) && !throwEventWithoutTag)
            return;
        else if (_tagSet.Contains(collision.gameObject.tag) && OnCollisionEnterAction != null)
        {
            OnCollisionEnterAction?.Invoke(collision, this.gameObject, falling);
        }
    }
}
