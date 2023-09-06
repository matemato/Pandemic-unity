using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectNameComparer : IComparer<GameObject>
{
    public int Compare(GameObject x, GameObject y)
    {
        if (x == null || y == null)
        {
            return 0; // Handle null objects, you can change this behavior as needed.
        }

        return string.Compare(x.name, y.name);
    }
}

public class TileIdGenerator : MonoBehaviour
{
    private List<GameObject> _tiles;
    // Start is called before the first frame update
    void Start()
    {
        _tiles = new List<GameObject>();
        _tiles.AddRange(GameObject.FindGameObjectsWithTag("Tile"));
        _tiles.Sort(new GameObjectNameComparer());

        int id = 0;
        foreach(GameObject tile in _tiles)
        {
            tile.GetComponent<Tile>().SetId(id);
            id++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
