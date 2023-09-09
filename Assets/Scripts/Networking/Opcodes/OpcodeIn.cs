using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpcodeIn
{
    private readonly ServerOpcode _id;

    public OpcodeIn(ServerOpcode id)
    {
        _id = id;
    }

    public virtual void Receive(MsgManager msgManager, ServerInput serverInput)
    {

    }
}
