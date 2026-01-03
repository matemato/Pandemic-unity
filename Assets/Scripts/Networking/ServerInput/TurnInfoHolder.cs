using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TurnInfoHolder
{
	private Queue<(int, int)> _activeTurnChange = new();
	private Queue<int> _endTurnChange = new();
	private Queue<(int, int)> _actionChange = new();

	public void SetActive(int pid, int actions)
	{
		_activeTurnChange.Enqueue((pid,actions));
	}

	public (int,int) GetActive()
	{
		if (_activeTurnChange.Count == 0)
		{
			return (-1,-1);
		}
		else
		{
			return _activeTurnChange.Dequeue();
		}
	}

	public void SetEnd(int pid)
	{
		_endTurnChange.Enqueue(pid);
	}
	public void SetActions(int pid, int actions)
	{
		_actionChange.Enqueue((pid,actions));
	}

	public (int, int) GetActionChange()
	{
		if (_actionChange.Count == 0)
		{
			return (-1, -1);
		}
		else
		{
			return _actionChange.Dequeue();
		}
	}

	public int GetEndTurnChange()
	{
		if (_endTurnChange.Count == 0)
		{
			return -1;
		}
		else
		{
			return _endTurnChange.Dequeue();
		}
	}
}
