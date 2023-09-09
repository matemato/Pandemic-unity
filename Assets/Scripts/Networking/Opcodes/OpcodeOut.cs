using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpcodeOut
{
    private readonly ClientOpcode id;

    public OpcodeOut(ClientOpcode id)
    {
        this.id = id;
    }

    public virtual void Send(MsgManager msgManager)
    {
        msgManager.WriteByte((byte)id);
    }
}
