using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayer : MonoBehaviour
{
    public Tile City;
    private int _id = 255;
    bool _lockId = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetId(int newId)
    {
        if(!_lockId)
        {
            _id = newId;
            _lockId = true;
        }
        else
        {
            Debug.LogError("Tried to SetId on OtherPlayer when it was already set.");
        }
    }

    public int GetId()
    {
        return _id;
    }

    // Update is called once per frame
    void Update()
    {
        if (City != null)
        {
            transform.position = City.transform.position;
        }
    }
}
