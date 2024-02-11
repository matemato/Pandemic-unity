using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutDiscard : OpcodeOut
{
	private byte _cardId;
	public OutDiscard(byte cardId) : base(ClientOpcode.DISCARD)
	{
		_cardId = cardId;
	}

	public override void Send(MsgManager msgManager)
	{
		base.Send(msgManager);
		msgManager.WriteByte(_cardId);
	}
}
