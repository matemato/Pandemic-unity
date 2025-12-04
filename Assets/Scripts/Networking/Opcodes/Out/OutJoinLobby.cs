using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OutJoinLobby : OpcodeOut
{
	private int _lobbyId;
	private string _name;
	public OutJoinLobby(int lobbyId, string name) : base(ClientOpcode.JOIN_LOBBY)
	{
		_lobbyId = lobbyId;
		_name = name;
	}

	public override void WriteBody(MsgManager msgManager)
	{
		msgManager.WriteInt((byte)_lobbyId);
		msgManager.WriteShort((ushort)_name.Length);
		msgManager.WriteString(_name);
	}
}
