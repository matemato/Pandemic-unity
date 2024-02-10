using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectionHolder
{
    private Queue<Tuple<InfectionCard, Queue<Tuple<InfectionType, int>>>> _infectionQueue = new(); // ok that's a bit cringe - ne ti si cringe
    
    public void Add(InfectionCard card_id, Queue<Tuple<InfectionType, int>> infectionData)
    {
        _infectionQueue.Enqueue(Tuple.Create(card_id, infectionData));
    }

    public Tuple<InfectionCard, Queue<Tuple<InfectionType, int>>> GetNext()
    {
        if(_infectionQueue.Count == 0)
        {
            return null;
        }

        return _infectionQueue.Dequeue();
    }

	public bool IsQueueEmpty()
	{
		if (_infectionQueue.Count == 0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
