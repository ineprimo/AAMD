using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOverlapListener : MonoBehaviour
{
    public string[] tags;
    public System.Action<Collider, GameObject> OnOverlapAction;
    public bool throwEventWithoutTag = false;
    public BoxCollider _collider;

    private HashSet<string> _tagSet;
    private void Start()
    {
        _tagSet = new HashSet<string>();
        for (int i = 0; i < tags.Length; ++i)
        {
            _tagSet.Add(tags[i]);
        }
    }
    private void Update()
    {
        Collider[] colliders = Physics.OverlapBox(_collider.transform.position+_collider.center, _collider.size/2f);
        for (int i = 0; i < colliders.Length; i++)
        {
            if(!colliders[i].isTrigger && colliders[i] != _collider && colliders[i].gameObject.layer != Globals.CAMERA_LAYER)
            {
                Collider other = colliders[i];
                if (OnOverlapAction == null)
                    return;
                if ((_tagSet == null || _tagSet.Count == 0) && throwEventWithoutTag)
                    OnOverlapAction(other, this.gameObject);
                else if ((_tagSet == null || _tagSet.Count == 0) && !throwEventWithoutTag)
                    return;
                else if (_tagSet.Contains(other.tag) && OnOverlapAction != null)
                {
                    OnOverlapAction(other, this.gameObject);
                }
            }
        }

    }

}
