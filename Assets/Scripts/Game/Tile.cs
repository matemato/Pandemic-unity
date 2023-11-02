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
    [HideInInspector]
    public CityColor _cityColor;
    
    public TileData(int id, string name, CityColor cityColor, int[] neighbourIds)
    {
        _id = id;
        _name = name;
        _cityColor = cityColor;
        _neighbourIds = neighbourIds;
    }
}

public class Tile : MonoBehaviour
{
    public string Color; 
    public PlayerCard PlayerCard;
    public Tile[] Neighbours;
    public bool Highlight;

    [HideInInspector]
    public TileData TileData;

    [HideInInspector]
    public CityColor CityColor;

    private int _id;
    [HideInInspector]
    public string Name;

    private SpriteRenderer _spriteRenderer;

    private Dictionary<string, CityColor> CityColorDict = new Dictionary<string, CityColor>()
    {
        { "Blue", CityColor.CITY_COLOR_BLUE },
        { "Yellow", CityColor.CITY_COLOR_YELLOW },
        { "Black", CityColor.CITY_COLOR_BLACK },
        { "Red", CityColor.CITY_COLOR_RED }
    };

    public Dictionary<InfectionType, int> _infectionCount = new Dictionary<InfectionType, int>()
    {
        { InfectionType.VIRUS_BLUE, 0 },
        { InfectionType.VIRUS_RED, 0 },
        { InfectionType.VIRUS_YELLOW, 0 },
        { InfectionType.VIRUS_BLACK, 0 }
    };

    // Start is called before the first frame update
    void Start()
    {
        Name = new string(gameObject.name);
        CityColor = CityColorDict[Color];
        Highlight = false;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void CreateTileData()
    {
        int[] neighbourIds = new int[Neighbours.Length];
        for(int i = 0; i < neighbourIds.Length; i++)
        {
            neighbourIds[i] = Neighbours[i].GetId();
        }
        TileData = new TileData(_id, Name, CityColor, neighbourIds);
    }

    public int GetId()
    {
        return _id;
    }

    public void SetId(int id)
    {
        _id = id;
    }

    public int GetInfectionCount(InfectionType infectionType)
    {
        return _infectionCount[infectionType];
    }
    public void SetInfectionCount(InfectionType infectionType, int infectionCount)
    {
        _infectionCount[infectionType] = infectionCount;
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
