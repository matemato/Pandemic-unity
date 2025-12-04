using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OutClientMessage : OpcodeOut
{
    private ClientMessageType _messageType;
    private string _chatMessage;
    public OutClientMessage(ClientMessageType message_type, string chat_message) : base(ClientOpcode.CLIENT_MESSAGE)
    {
        _messageType = message_type;
        _chatMessage = chat_message;
    }

    public override void WriteBody(MsgManager msgManager)
    {
        msgManager.WriteByte((byte)_messageType);
        msgManager.WriteShort((ushort) _chatMessage.Length);
        msgManager.WriteString(_chatMessage);
    }
}
