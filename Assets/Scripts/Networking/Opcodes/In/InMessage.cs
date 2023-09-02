using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InMessage : OpcodeIn
{
    public InMessage() : base(0)
    {

    }

    public override void Receive(MsgManager msgManager, ServerInput serverInput)
    {
        byte length = msgManager.ReadByte();
        serverInput.MessageHolder.Add(msgManager.ReadString(length));
    }
}
