using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public string Color;
    public Tile[] Neighbours;

    static private int ID = 0;
    private int Id;
    // Start is called before the first frame update
    void Start()
    {
        Id = ID++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
