using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageMgr : MonoBehaviour
{
    private Dictionary<System.Type, IMessage> _dictionary = new Dictionary<System.Type, IMessage>();

    public bool CreateMsg<T>() where T : IMessage, new()
    {
        System.Type t = typeof(T);
        if (!_dictionary.ContainsKey(t))
        {
            Create<T>();
        }
        return false;
    }

    public void AddListener<T, Q>(Message<Q>.MessageCallback c) where T : Message<Q>, new()
    {
        System.Type t = typeof(T);
        if (!_dictionary.ContainsKey(t))
            Create<T>();
        IMessage ms = _dictionary[t];
        Message<Q> m = (Message<Q>)ms;
        m.AddListener(c);
    }

    public bool SendMsg<T, Q>(GameObject go, Q data) where T : Message<Q>, new()
    {
        IMessage ms;
        if (_dictionary.TryGetValue(typeof(T),out ms))
        {
            Message<Q> m = (Message<Q>)ms;
            m.Send(go, data);
            return true;
        }
        return false;
    }

    private IMessage Create<T>() where T : IMessage, new()
    {
        IMessage m = new T();
        _dictionary.Add(typeof(T), m);
        return m;
    }
}
