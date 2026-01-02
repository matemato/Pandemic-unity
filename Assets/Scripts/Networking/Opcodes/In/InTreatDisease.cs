using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InTreatDisease : OpcodeIn
{
	public InTreatDisease() : base(ServerOpcode.TREAT_DISEASE)
	{

	}

	public override void Receive(MsgManager msgManager, ServerInput serverInput)
	{
		var pid = msgManager.ReadByte(); // who treated the disease
		var type = (InfectionType)msgManager.ReadByte(); // which type of disease was treated
		var newCount = msgManager.ReadByte(); // updated count of diseases of that type on tile
		var loc = msgManager.ReadByte(); // which tile to update the count on

		serverInput.TreatDiseaseHolder.Add(pid, type, newCount, loc);

    }
}
