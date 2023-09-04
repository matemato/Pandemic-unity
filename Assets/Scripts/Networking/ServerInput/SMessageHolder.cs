using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMessageHolder
{
    private Queue<Tuple<ServerMessageType, string>> _messages;

    public SMessageHolder()
    {
        _messages = new Queue<Tuple<ServerMessageType, string>>();
    }

    public void Add(Tuple<ServerMessageType, string> msg)
    {
        _messages.Enqueue(msg);
    }

    public Tuple<ServerMessageType, string> GetNext()
    {
        if (_messages.Count > 0)
        {
            Debug.Log("dequeued");
            return _messages.Dequeue();
        }
        else
        {
            return null;
        }
    }
}

