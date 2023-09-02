using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpcodeOut
{
    private readonly byte id;

    public OpcodeOut(byte id)
    {
        this.id = id;
    }

    public virtual void Send(MsgManager msgManager)
    {
        msgManager.WriteByte(id);
    }
}
