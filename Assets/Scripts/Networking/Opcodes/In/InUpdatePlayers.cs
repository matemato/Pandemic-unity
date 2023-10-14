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
        for(int i = 0; i < numPlayers; i++)
        {
            var position = msgManager.ReadByte();
            serverInput.PlayerUpdateHolder.Set(i, position);
        }
        
    }
}
