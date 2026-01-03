using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Tile CurrentCity;

    private int _id;
    bool _lockId = false;

    private List<GameObject> _playerHand = new List<GameObject>();

    private GameObject[] _tiles;

    [SerializeField]
    private GameController _gameController;

	private GameObject _playerInfoManager;

	// private int _playerTurns = 0;

	// Start is called before the first frame update
	void Start()
    {
        _tiles = GameObject.FindGameObjectsWithTag("Tile");
        _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		_playerInfoManager = GameObject.Find("PlayerInfoManager");
	}

	public void SetColor(int id)
	{
		switch(id)
		{
			case 3:
			{
				var sp = Resources.Load<Sprite>("Meeples/orange");
				gameObject.GetComponent<SpriteRenderer>().sprite = sp;
				break;
			}
			case 2:
			{
				var sp = Resources.Load<Sprite>("Meeples/green");
				gameObject.GetComponent<SpriteRenderer>().sprite = sp;
				break;
			}
			case 1:
			{
				var sp = Resources.Load<Sprite>("Meeples/violet");
				gameObject.GetComponent<SpriteRenderer>().sprite = sp;
				break;
			}
			case 0:
			default:
			{
				var sp = Resources.Load<Sprite>("Meeples/yellow");
				gameObject.GetComponent<SpriteRenderer>().sprite = sp;
				break;
			}
		}
	}

    public void SetId(int newId)
    {
        if (!_lockId)
        {
            _id = newId;
            _lockId = true;
        }
        else
        {
            Debug.LogError("Tried to SetId on OtherPlayer when it was already set.");
        }
    }

    public int GetId()
    {
        return _id;
    }

	public int GetActions()
	{
		var playerInfoManager = _playerInfoManager.GetComponent<PlayerInfoManager>();
		return playerInfoManager.GetActions(GetId());
	}

    public void AddCardToHand(GameObject card)
    {
        _playerHand.Add(card);
    }

    public void RemoveCardFromHand(GameObject card)
    {
        _playerHand.Remove(card);
    }

    public List<GameObject> GetPlayerHand()
    {
        return _playerHand;
    }

    // Update is called once per frame
    void Update()
    {
        if(_gameController.ServerInput != null)
        {
            int newPosition = _gameController.ServerInput.PlayerUpdateHolder.Get(GetId());
            if (newPosition != -1)
            {
				int offset = 0;
				if (CurrentCity != null)
					CurrentCity.RemovePlayer(GetId());

                foreach (GameObject city in _tiles)
                {
                    city.GetComponent<Tile>().Highlight = false;
                    if (city.GetComponent<Tile>().GetId() == newPosition)
                    {
                        CurrentCity = city.GetComponent<Tile>();
						offset = CurrentCity.PutPlayer(GetId());
                    }
                }

                if (CurrentCity != null)
                {
                    foreach (Tile city in CurrentCity.Neighbours)
                    {
                        city.Highlight = true;
                    }
					//transform.position = City.transform.position;
					//Debug.Log("offset: " + offset);
					float xOffset = 0.4f - 0.2f * offset;
					transform.position = CurrentCity.transform.position - new Vector3(xOffset, 0.3f, 0);
				}
            }
        }
        
    }
}
