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

		if(type == UpdateTurnType.TURN_BEGIN)
		{
			serverInput.TurnInfoHolder.SetActive(pid, actions);
		}
		else if (type == UpdateTurnType.TURN_END)
		{
			serverInput.TurnInfoHolder.SetEnd(pid);
		}
		else if (type == UpdateTurnType.UPDATE_ACTIONS)
		{
			serverInput.TurnInfoHolder.SetActions(pid, actions);
		}
	}
}
