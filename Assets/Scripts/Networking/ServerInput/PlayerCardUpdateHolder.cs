using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCardUpdateHolder
{
    private Queue<Tuple<byte, bool, PlayerCard>> _playerCardUpdateQueue;

    public PlayerCardUpdateHolder()
    {
        _playerCardUpdateQueue = new Queue<Tuple<byte, bool, PlayerCard>>();
    }

    public void Add(byte playerId, bool remove, PlayerCard card)
    {
        _playerCardUpdateQueue.Enqueue(new Tuple<byte, bool, PlayerCard>(playerId, remove, card));
    }

    public Tuple<byte, bool, PlayerCard> GetNext()
    {
        if(_playerCardUpdateQueue.Count == 0)
        {
            return null;
        }
        else
        {
            return _playerCardUpdateQueue.Dequeue();
        }
    }
}