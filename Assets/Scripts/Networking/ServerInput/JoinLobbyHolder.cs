using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinLobbyHolder
{
	private JoinLobbyResponse _response = JoinLobbyResponse.LOBBY_PENDING;

	public JoinLobbyHolder(byte response)
	{
		_response = (JoinLobbyResponse)response;
	}

	public JoinLobbyHolder()
	{
		_response = JoinLobbyResponse.LOBBY_PENDING;
	}

	public JoinLobbyResponse GetResponse()
	{
		var rsp = _response;
		_response = JoinLobbyResponse.LOBBY_PENDING;
		return rsp;
	}
}
