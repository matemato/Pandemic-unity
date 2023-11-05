using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectionHolder
{
    private Queue<Tuple<InfectionCard, Queue<Tuple<InfectionType, int>>>> _infectionQueue = new(); // ok that's a bit cringe
    
    public void Add(InfectionCard card_id, Queue<Tuple<InfectionType, int>> infectionData)
    {
        _infectionQueue.Enqueue(Tuple.Create(card_id, infectionData));
    }

    public Tuple<InfectionCard, Queue<Tuple<InfectionType, int>>> getNext()
    {
        if(_infectionQueue.Count == 0)
        {
            return null;
        }

        return _infectionQueue.Dequeue();
    }
}
