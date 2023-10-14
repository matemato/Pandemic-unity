using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMessageHolder
{
    private Queue<Tuple<ServerMessageType, string>> _messages;
    private Queue<string> _lobbyText;

    public SMessageHolder()
    {
        _messages = new Queue<Tuple<ServerMessageType, string>>();
        _lobbyText = new Queue<string>();
    }

    public void Add(Tuple<ServerMessageType, string> msg)
    {
        if (msg.Item1 == ServerMessageType.SMESSAGE_CHAT || msg.Item1 == ServerMessageType.SMESSAGE_INFO)
            _messages.Enqueue(msg);
        else if (msg.Item1 == ServerMessageType.SMESSAGE_LOBBY)
            _lobbyText.Enqueue(msg.Item2);
    }

    public Tuple<ServerMessageType, string> GetNextConsoleMessage()
    {
        if (_messages.Count > 0)
        {
            return _messages.Dequeue();
        }
        else
        {
            return null;
        }
    }

    public string GetNextLobbyText()
    {
        if (_lobbyText.Count > 0)
        {
            Debug.Log("dequeued");
            return _lobbyText.Dequeue();
        }
        else
        {
            return null;
        }
    }
}

