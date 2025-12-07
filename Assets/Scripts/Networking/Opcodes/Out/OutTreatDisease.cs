using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutTreatDisease : OpcodeOut
{
	private InfectionType _treatDiseaseType;
	public OutTreatDisease(InfectionType treatDiseaseType) : base(ClientOpcode.TREAT_DISEASE)
	{
		_treatDiseaseType = treatDiseaseType;
	}

	public override void WriteBody(MsgManager msgManager)
	{
		msgManager.WriteByte((byte)_treatDiseaseType);
	}
}
