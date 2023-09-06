using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginGameHolder
{
    private bool _gameStarted = false;
    public byte _numPlayers;
    public byte _playerId;
    public List<string> _playerNames;
    public List<PlayerRole> _playerRoles;

    public BeginGameHolder()
    {

    }

    public bool HasGameStarted()
    {
        return _gameStarted;
    }

    public void BeginGame(byte numPlayers, byte playerId, List<string> playerNames, List<PlayerRole> playerRoles)
    {
        _gameStarted = true;
        _numPlayers = numPlayers;
        _playerId = playerId;
        _playerNames = playerNames;
        _playerRoles = playerRoles;
    }
}
