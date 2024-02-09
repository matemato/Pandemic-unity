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
    [HideInInspector]
    public PlayerCard _playerCard;
    [HideInInspector]
    public InfectionCard _infectionCard;

    public TileData(int id, string name, CityColor cityColor, int[] neighbourIds, PlayerCard playerCard, InfectionCard infectionCard)
    {
        _id = id;
        _name = name;
        _cityColor = cityColor;
        _neighbourIds = neighbourIds;
        _playerCard = playerCard;
        _infectionCard = infectionCard;
    }
}

public class Tile : MonoBehaviour
{
    public string Color; 
    public PlayerCard PlayerCard;
    public InfectionCard InfectionCard;
    public Tile[] Neighbours;
    public bool Highlight;
	public int NumPlayers;

    [HideInInspector]
    public TileData TileData;

    [HideInInspector]
    public CityColor CityColor;

    private int _id;
    [HideInInspector]
    public string Name;

	private int[] PlayerSlots;

    private SpriteRenderer _spriteRenderer;

    private List<GameObject> _virusCubes = new List<GameObject>();

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
		NumPlayers = 0;
		PlayerSlots = new int[4];
		for (int i = 0; i < PlayerSlots.Length; i++)
			PlayerSlots[i] = -1;
    }

	public int PutPlayer(int id)
	{
		int i;
		for (i = 0; i < PlayerSlots.Length; i++)
		{
			if (PlayerSlots[i] == -1)
			{
				PlayerSlots[i] = id;
				break;
			}
				
		}
		return i;
	}

	public void RemovePlayer(int id)
	{
		for (int i = 0; i < PlayerSlots.Length; i++)
		{
			if (PlayerSlots[i] == id)
			{
				PlayerSlots[i] = -1;
				break;
			}
				
		}
	}

    // Update is called once per frame
    void Update()
    {
        Color currentColor = _spriteRenderer.color;
        if (Highlight)
        {
            currentColor.a = 1f;
        }
        else
        {
            currentColor.a = 0f;
        }
        _spriteRenderer.color = currentColor;
    }

    public void CreateTileData()
    {
        int[] neighbourIds = new int[Neighbours.Length];
        for(int i = 0; i < neighbourIds.Length; i++)
        {
            neighbourIds[i] = Neighbours[i].GetId();
        }
        TileData = new TileData(_id, Name, CityColor, neighbourIds, PlayerCard, InfectionCard);
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

    public void AddVirusCube(GameObject virusCube)
    {
        _virusCubes.Add(virusCube);
    }

    public List<GameObject> GetVirusCubes() { return  _virusCubes; }
}
