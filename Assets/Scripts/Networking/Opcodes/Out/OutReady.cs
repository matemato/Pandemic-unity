using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutReady : OpcodeOut
{
	public OutReady() : base(ClientOpcode.READY)
	{

	}

	public override void Send(MsgManager msgManager)
	{
		base.Send(msgManager);
	}
}
