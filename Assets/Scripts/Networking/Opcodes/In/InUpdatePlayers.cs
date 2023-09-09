using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InUpdatePlayers : OpcodeIn
{
    public InUpdatePlayers() : base(ServerOpcode.UPDATE_PLAYERS)
    {

    }

    public override void Receive(MsgManager msgManager, ServerInput serverInput)
    {
        byte numPlayers = msgManager.ReadByte();
        byte[] positions = new byte[numPlayers];
        for(int i = 0; i < numPlayers; i++)
        {
            positions[i] = msgManager.ReadByte();
        }
        serverInput.PlayerUpdateHolder.Set(positions);
    }
}
