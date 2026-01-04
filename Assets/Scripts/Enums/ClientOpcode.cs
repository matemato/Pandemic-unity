using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClientOpcode
{
	IDLE,
	MOVE,
	CLIENT_MESSAGE,
	READY,
	DISCARD,
	JOIN_LOBBY,
	TREAT_DISEASE,
	RESEARCH
}
