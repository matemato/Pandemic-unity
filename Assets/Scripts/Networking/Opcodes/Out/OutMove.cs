using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutMove : OpcodeOut
{
    private byte _targetCity;
    public OutMove(byte target_city) : base(ClientOpcode.MOVE)
    {
        _targetCity = target_city;
    }

    public override void Send(MsgManager msgManager)
    {
        base.Send(msgManager);
        msgManager.WriteByte(_targetCity);
    }
}
