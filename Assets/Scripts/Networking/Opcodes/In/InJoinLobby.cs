using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InJoinLobby : OpcodeIn
{
	public InJoinLobby() : base(ServerOpcode.JOIN_LOBBY)
	{

	}

	public override void Receive(MsgManager msgManager, ServerInput serverInput)
	{
		var response = msgManager.ReadByte();
		serverInput.JoinLobbyHolder = new(response);
	}
}
