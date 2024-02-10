using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InUpdateTurn : OpcodeIn
{
	public InUpdateTurn() : base(ServerOpcode.UPDATE_TURN)
	{

	}

	public override void Receive(MsgManager msgManager, ServerInput serverInput)
	{
		var type = (UpdateTurnType)msgManager.ReadByte();
		var pid = msgManager.ReadByte();
		var actions = msgManager.ReadByte();
	}
}
