using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TileData is used only to generate JSON
public class TileData
{
    [HideInInspector]
    public int _id;
    [HideInInspector]
    public string _name;
    [HideInInspector]
    public int[] _neighbourIds;

    public TileData(int id, string name, int[] neighbourIds)
    {
        _id = id;
        _name = name;
        _neighbourIds = neighbourIds;
    }
}

public class Tile : MonoBehaviour
{
    public string Color;
    public Tile[] Neighbours;
    public bool Highlight;

    [HideInInspector]
    public TileData TileData;

    static private int ID = 0;

    [HideInInspector]
    public int _id;
    [HideInInspector]
    public string _name;

    private SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        _id = ID++;
        _name = new string(gameObject.name);
        Highlight = false;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void CreateTileData()
    {
        int[] neighbourIds = new int[Neighbours.Length];
        for(int i = 0; i < neighbourIds.Length;i++)
        {
            neighbourIds[i] = Neighbours[i].GetId();
        }
        TileData = new TileData(_id, _name, neighbourIds);
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
