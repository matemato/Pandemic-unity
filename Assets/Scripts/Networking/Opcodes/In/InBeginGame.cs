using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InBeginGame : OpcodeIn
{
    public InBeginGame() : base(2)
    {

    }

    public override void Receive(MsgManager msgManager, ServerInput serverInput)
    {
        var numPlayers = msgManager.ReadByte();
        var playerId = msgManager.ReadByte();
        serverInput.BeginGameHolder.BeginGame(numPlayers, playerId);
    }
}
