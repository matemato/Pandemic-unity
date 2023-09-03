using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    // Start is called before the first frame update
    private bool _handled = true;
    private Click _click = null;

    public bool MovePending()
    {
        if(_handled == false && _click != null && _click is ClickMove)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Create(Click newClick)
    {
        _handled = false;
        _click = newClick;
    }

    public Click Handle()
    {
        if(_handled == true)
        {
            return null;
        }
        else
        {
            var temp = _click;
            _click = null;
            _handled = true;
            return temp;
        }
    }
}
