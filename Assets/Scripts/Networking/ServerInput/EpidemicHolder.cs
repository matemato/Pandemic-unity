using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpidemicHolder
{
	private Queue<bool> _epidemicQueue = new();
	private Queue<bool> _clearInfectionDiscardPile = new();
    
    public void Add()
    {
		_epidemicQueue.Enqueue(true);
		_clearInfectionDiscardPile.Enqueue(true);
	}

	public bool PendingEpidemic()
	{
		if (_epidemicQueue.Count == 0)
		{
			return false;
		}
		else
		{
			_epidemicQueue.Dequeue();
			return true;
		}
	}

	public bool PendingClear()
	{
		if (_clearInfectionDiscardPile.Count == 0)
		{
			return false;
		}
		else
		{
			_clearInfectionDiscardPile.Dequeue();
			return true;
		}
	}
}
