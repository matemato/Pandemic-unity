using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutIdle : OpcodeOut
{
    public OutIdle() : base(ClientOpcode.IDLE)
    {

    }

    public override void WriteBody(MsgManager msgManager)
    {
    }
}
