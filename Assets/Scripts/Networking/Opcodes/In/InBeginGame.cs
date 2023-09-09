using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InBeginGame : OpcodeIn
{
    public InBeginGame() : base(ServerOpcode.BEGIN_GAME)
    {

    }

    public override void Receive(MsgManager msgManager, ServerInput serverInput)
    {
        List<string> playerNames = new List<string>();
        List<PlayerRole> playerRoles = new List<PlayerRole>();
        var numPlayers = msgManager.ReadByte();
        var playerId = msgManager.ReadByte();
        for(int i = 0; i < numPlayers;i++)
        {
            var player_role = msgManager.ReadByte();
            var name_len = msgManager.ReadByte();
            var name = msgManager.ReadString((ushort)name_len);

            playerRoles.Add((PlayerRole)player_role);
            playerNames.Add(name);
        }
        serverInput.BeginGameHolder.BeginGame(numPlayers, playerId, playerNames, playerRoles);
    }
}
