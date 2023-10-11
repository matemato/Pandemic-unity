using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InUpdatePlayerCard : OpcodeIn
{
    public InUpdatePlayerCard() : base(ServerOpcode.UPDATE_PLAYER_CARD)
    {

    }

    public override void Receive(MsgManager msgManager, ServerInput serverInput)
    {
        byte pid = msgManager.ReadByte();
        byte remove = msgManager.ReadByte();
        byte numCards = msgManager.ReadByte();
        byte[] cards = new byte[numCards];
        for (int i = 0; i < numCards; i++)
        {
            cards[i] = msgManager.ReadByte();
            serverInput.PlayerCardUpdateHolder.Add(pid, Convert.ToBoolean(remove), (PlayerCard)cards[i]);
        }
        
    }
}
