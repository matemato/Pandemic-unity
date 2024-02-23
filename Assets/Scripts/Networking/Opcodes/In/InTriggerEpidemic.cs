using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InTriggerEpidemic : OpcodeIn
{
	public InTriggerEpidemic() : base(ServerOpcode.TRIGGER_EPIDEMIC)
	{

	}

	public override void Receive(MsgManager msgManager, ServerInput serverInput)
	{
		serverInput.EpidemicHolder.Add();
	}
}
