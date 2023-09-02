using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InError : OpcodeIn
{
    public InError() : base(0)
    {

    }

    public override void Receive(MsgManager msgManager, ServerInput serverInput)
    {
        serverInput.InvalidOpcode = true;
    }
}
