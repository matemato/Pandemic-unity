using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayer : MonoBehaviour
{
    public Tile City;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = City.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = City.transform.position;
    }
}
