using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerInput
{
    
    public SMessageHolder MessageHolder;
    public PlayerUpdateHolder PlayerUpdateHolder;
    public bool InvalidOpcode;
    
    public ServerInput()
    {
        InvalidOpcode = false;
        MessageHolder = new SMessageHolder();
        PlayerUpdateHolder = new PlayerUpdateHolder();
    }
}
