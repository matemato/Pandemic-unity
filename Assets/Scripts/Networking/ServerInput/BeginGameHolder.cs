using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginGameHolder
{
    private bool _gameStarted = false;
    public byte _numPlayers;
    public byte _playerId;

    public BeginGameHolder()
    {

    }

    public bool HasGameStarted()
    {
        return _gameStarted;
    }

    public void BeginGame(byte numPlayers, byte playerId)
    {
        _gameStarted = true;
        _numPlayers = numPlayers;
        _playerId = playerId;
    }
}
