using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    private List<Tile> _tiles;
    private int _delay = 0;
    private string _loc;
    public bool run = false;
    void Start()
    {
        if (run)
        {
            var tiles = GameObject.FindGameObjectsWithTag("Tile");
            _loc = Application.dataPath + "/map_data.json";
            _tiles = new List<Tile>();
            foreach (var it in tiles)
            {
                _tiles.Add(it.GetComponent<Tile>());
            }
            Debug.Log(Application.dataPath);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(run)
        {
            _delay++;
            if (_delay == 100)
            {
                int i = 0;
                string[] stringData = new string[_tiles.Count];
                foreach (var tile in _tiles)
                {
                    tile.CreateTileData();
                    var output = JsonUtility.ToJson(tile.TileData);
                    stringData[i] = output;
                    Debug.Log(output);
                    i++;
                }
                File.WriteAllLines(_loc, stringData);
            }
        }
        
    }
}
