using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMessageHolder
{
    private Queue<string> _messages;

    public SMessageHolder()
    {
        _messages = new Queue<string>();
    }

    public void Add(string msg)
    {
        _messages.Enqueue(msg);
    }

    public string GetNext()
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

