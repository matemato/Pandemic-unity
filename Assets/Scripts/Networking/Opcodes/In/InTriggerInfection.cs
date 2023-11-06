using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InTriggerInfection : OpcodeIn
{
    public InTriggerInfection() : base(ServerOpcode.TRIGGER_INFECTION)
    {

    }

    public override void Receive(MsgManager msgManager, ServerInput serverInput)
    {
        Queue<Tuple<InfectionType, int>> infectionData = new();
        byte cardId = msgManager.ReadByte();
        byte length = msgManager.ReadByte();

        for(int i = 0; i < length; i++)
        {
            InfectionType infectionType = (InfectionType)msgManager.ReadByte();
            int cityId = msgManager.ReadByte();
            infectionData.Enqueue(new Tuple<InfectionType, int>(infectionType, cityId));
        }

        serverInput.InfectionHolder.Add((InfectionCard)cardId, infectionData);
    }
}
