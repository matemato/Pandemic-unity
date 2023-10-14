using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpdateHolder 
{
    private bool[] _changed;
    private byte[] _positions;

    public PlayerUpdateHolder()
    {
        _changed = new bool[4];
        _positions = new byte[4];
    }

    public void Set(int id, byte position)
    {
        _positions[id] = position;
        _changed[id] = true;
    }

    public int Get(int id)
    {
        if(!_changed[id])
        {
            return -1;
        }
        else
        {
            _changed[id] = false;
            return _positions[id];
        }
    }
}
