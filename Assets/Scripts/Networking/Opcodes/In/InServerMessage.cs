using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InServerMessage : OpcodeIn
{
    public InServerMessage() : base(ServerOpcode.SERVER_MESSAGE)
    {

    }

    public override void Receive(MsgManager msgManager, ServerInput serverInput)
    {
        byte serverMessageType = msgManager.ReadByte();
        ushort length = msgManager.ReadShort();
        serverInput.MessageHolder.Add(new System.Tuple<ServerMessageType, string>((ServerMessageType) serverMessageType, msgManager.ReadString(length)));
    }
}
