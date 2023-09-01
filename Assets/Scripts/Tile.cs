using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public string Color;
    public Tile[] Neighbours;
    public bool Neighbour;

    static private int ID = 0;
    private int Id;

    private SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        Id = ID++;

        Neighbour = false;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Neighbour) {
            Color currentColor = _spriteRenderer.color;
            currentColor.a = 1f;
            _spriteRenderer.color = currentColor;
        }
    }
}
