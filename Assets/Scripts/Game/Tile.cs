using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public string Color;
    public Tile[] Neighbours;
    public bool Highlight;

    static private int ID = 0;
    private int _id;

    private SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        _id = ID++;

        Highlight = false;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public int GetId()
    {
        return _id;
    }

    // Update is called once per frame
    void Update()
    {
        Color currentColor = _spriteRenderer.color;
        if (Highlight) {    
            currentColor.a = 1f;
        }
        else {
            currentColor.a = 0f;
        }
        _spriteRenderer.color = currentColor;
    }
}
