using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerInput
{
    
    public SMessageHolder MessageHolder;
    public PlayerUpdateHolder PlayerUpdateHolder;
    public BeginGameHolder BeginGameHolder;
    public PlayerCardUpdateHolder PlayerCardUpdateHolder;
    public bool InvalidOpcode;

    public ServerInput()
    {
        InvalidOpcode = false;
        MessageHolder = new SMessageHolder();
        PlayerUpdateHolder = new PlayerUpdateHolder();
        BeginGameHolder = new BeginGameHolder();
        PlayerCardUpdateHolder = new PlayerCardUpdateHolder();
    }
}
