using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Tile City;
    public Click Click;

    private int _id;
    bool _lockId = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetId(int newId)
    {
        if (!_lockId)
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
        foreach (GameObject city in GameObject.FindGameObjectsWithTag("Tile")) {
            city.GetComponent<Tile>().Highlight = false;
        }

        if (City != null)
        {
            foreach (Tile city in City.Neighbours) {
                city.Highlight = true;
            }
            transform.position = City.transform.position;
        }
    }
}
