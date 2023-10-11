using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginGameHolder
{
    private bool _gameStarted = false;
    public byte NumPlayers;
    public byte PlayerId;
    public List<string> PlayerNames;
    public List<PlayerRole> PlayerRoles;

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
        NumPlayers = numPlayers;
        PlayerId = playerId;
        PlayerNames = playerNames;
        PlayerRoles = playerRoles;
    }
}
