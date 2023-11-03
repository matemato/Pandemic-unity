using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectionCardScript : MonoBehaviour
{
    // Start is called before the first frame update
    private InfectionCard _infectionCard;
    private InfectionType _infectionType;

    public void SetInfectionType(InfectionType infectionType)
    {
        _infectionType = infectionType;
    }

    public InfectionType GetInfectionType()
    {
        return _infectionType;
    }
    public void SetInfectionCard(InfectionCard infectionCard)
    {
        _infectionCard = infectionCard;
    }

    public InfectionCard GetInfectionCard()
    {
        return _infectionCard;
    }
}
