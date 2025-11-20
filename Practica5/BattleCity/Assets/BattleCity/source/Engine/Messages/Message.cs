using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message<T> : IMessage
{
    public delegate void MessageCallback(GameObject sender, T data);
    protected MessageCallback _message;

    public void Send(GameObject sender, T data)
    {
        _message(sender, data);
    }

    public void AddListener(MessageCallback c)
    {
        _message += c;
    }

    public void DeleteListener(MessageCallback c)
    {
        _message -= c;
    }
}

