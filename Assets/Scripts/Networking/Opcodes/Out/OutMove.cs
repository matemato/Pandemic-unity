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

    public override void WriteBody(MsgManager msgManager)
    {
        msgManager.WriteByte(_targetCity);
    }
}
