using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutIdle : OpcodeOut
{
    public OutIdle() : base(0)
    {

    }

    public override void Send(MsgManager msgManager)
    {
        base.Send(msgManager);
    }
}
