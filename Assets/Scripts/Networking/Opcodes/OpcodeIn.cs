using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpcodeIn
{
    private readonly byte _id;

    public OpcodeIn(byte id)
    {
        _id = id;
    }

    public virtual void Receive(MsgManager msgManager, ServerInput serverInput)
    {

    }
}
