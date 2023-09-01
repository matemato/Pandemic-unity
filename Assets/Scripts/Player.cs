using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
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
        foreach (GameObject city in GameObject.FindGameObjectsWithTag("Tile")) {
            city.GetComponent<Tile>().Highlight = false;
        }

        foreach (Tile city in City.Neighbours) {
            city.Highlight = true;
        }


        transform.position = City.transform.position;
    }
}
