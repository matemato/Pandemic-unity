using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMove : Click
{
    public int CityDest;

    public ClickMove(int cityDest) : base()
    {
        CityDest = cityDest;
    }
}
