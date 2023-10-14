using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpdateHolder 
{
    private bool _changed;
    private byte[] _positions;

    public PlayerUpdateHolder()
    {
        _changed = false;
    }

    public void Set(byte[] positions)
    {
        _positions = positions;
        _changed = true;
    }

    public byte[] Get()
    {
        if(!_changed || _positions == null)
        {
            return null;
        }
        else
        {
            _changed = false;
            return _positions;
        }
    }
}
