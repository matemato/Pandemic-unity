using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayer : MonoBehaviour
{
    public Tile City;
    private int _id = 255;
    bool _lockId = false;

    private GameController _gameController;

    private GameObject[] _tiles;

    // Start is called before the first frame update
    void Start()
    {
        _tiles = GameObject.FindGameObjectsWithTag("Tile");
        _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    public void SetId(int newId)
    {
        if(!_lockId)
        {
            _id = newId;
            _lockId = true;
        }
        else
        {
            Debug.LogError("Tried to SetId on OtherPlayer when it was already set.");
        }
    }

	public void SetColor(int id)
	{
		switch (id)
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

	public int GetId()
    {
        return _id;
    }

	// Update is called once per frame
	void Update()
	{
		if (_gameController.ServerInput != null)
		{
			int newPosition = _gameController.ServerInput.PlayerUpdateHolder.Get(GetId());
			if (newPosition != -1)
			{
				int offset = 0;
				if (City != null)
					City.RemovePlayer(GetId());

				foreach (GameObject city in _tiles)
				{
					if (city.GetComponent<Tile>().GetId() == newPosition)
					{
						City = city.GetComponent<Tile>();
						offset = City.PutPlayer(GetId());
					}
				}

				if (City != null)
				{
					//transform.position = City.transform.position;
					//Debug.Log("offset: " + offset);
					float xOffset = 0.4f - 0.2f * offset;
					transform.position = new Vector3(City.transform.position.x - xOffset, City.transform.position.y - 0.3f, City.transform.position.z);
				}
			}
		}

	}
}
