using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutMove : OpcodeOut
{
    private byte _targetCity;
	private MovementType _moveType;
    public OutMove(byte target_city, MovementType moveType) : base(ClientOpcode.MOVE)
    {
        _targetCity = target_city;
		_moveType = moveType;
    }

    public override void WriteBody(MsgManager msgManager)
    {
        msgManager.WriteByte(_targetCity);
		msgManager.WriteInt((uint)_moveType);
    }
}
