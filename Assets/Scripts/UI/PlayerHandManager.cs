using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerHandManager : MonoBehaviour
{
    [SerializeField]
    private Sprite[] _cardPics;

    [SerializeField]
    private GameObject _playerCardPrefab;

    [SerializeField]
    private GameObject _playerInfoManager;

    [SerializeField]
    private GameObject _gameControllerObject;

    [SerializeField]
    private GameObject _animationControllerObject;

    private GameController _gameController;
    private AnimationController _animationController;
    private int discardCardOnTop = 0;
	public int TotalCardCount = 0;

    private Dictionary<CityColor, int> _playerHandCount = new Dictionary<CityColor, int>
        {
            { CityColor.CITY_COLOR_BLUE, 0 },
            { CityColor.CITY_COLOR_YELLOW, 0 },
            { CityColor.CITY_COLOR_BLACK, 0 },
            { CityColor.CITY_COLOR_RED, 0 }
        };

    void Start()
    {
        _gameController =_gameControllerObject.GetComponent<GameController>();
        _animationController =_animationControllerObject.GetComponent<AnimationController>();

    }

    void Update()
    {
        if (_gameController.ServerInput != null)
        {
            var request = _gameController.ServerInput.PlayerCardUpdateHolder.GetNext();

            if (request != null)
            {
                int id = (int)request.Item1;
                bool remove = request.Item2;
                PlayerCard playerCard = request.Item3;

                if (remove)
                {
                    RemovePlayerCard(id, playerCard);
                }
                else
                {
                    AddPlayerCard(id, playerCard);
                }
            }
        }

		/*
        if (UnityEngine.Input.GetKeyDown(KeyCode.B))
        {
            AddPlayerCard(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().GetId(), PlayerCard.CCARD_ATLANTA);
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.Y))
        {
            AddPlayerCard(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().GetId(), PlayerCard.CCARD_SAO_PAULO);
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.T))
        {
            AddPlayerCard(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().GetId(), PlayerCard.CCARD_BEIJING);
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.R))
        {
            var playerHand = GameObject.FindGameObjectsWithTag("PlayerCard");
            RemovePlayerCard(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().GetId(), playerHand[0].GetComponent<PlayerCardScript>().GetPlayerCard());
        }
        */
		


    }

    public string EnumToString(PlayerCard playerCard)
    {
        if (playerCard == PlayerCard.CCARD_ST_PETERSBURG) return "St. Petersburg";

        string cardName = playerCard.ToString().Substring(6);
        cardName = cardName.Replace("_", " ").ToLower();

        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(cardName.ToLower());
    }



    public void AddPlayerCard(int id, PlayerCard playerCard)
    {
        var playerId = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().GetId();
        var cityTiles = GameObject.FindGameObjectsWithTag("Tile");
        CityColor playerCardColor = new();
        foreach (var tile in cityTiles)
        {
            Tile tileScript= tile.GetComponent<Tile>();
            if (tileScript.PlayerCard == playerCard)
            {
                playerCardColor = tileScript.CityColor;
                break;
            }
        }

        string playerCardName = EnumToString(playerCard);
             
        if (playerId == id)
        {
            var newCard = Instantiate(_playerCardPrefab, new Vector3(-740, 250, -1), Quaternion.identity);
            newCard.transform.SetParent(gameObject.transform, false);
            newCard = newCard.transform.GetChild(0).gameObject;     
            newCard.GetComponent<PlayerCardScript>().SetPlayerCard(playerCard);
            newCard.GetComponent<PlayerCardScript>().SetCityColor(playerCardColor);
			newCard.GetComponent<PlayerCardScript>().GameController = _gameControllerObject.GetComponent<GameController>();
			newCard.GetComponent<PlayerCardScript>().PlayerHandManager = this;
			_playerHandCount[playerCardColor]++;
			TotalCardCount++;
			foreach (Sprite cardPic in _cardPics)
            {
                if (cardPic.name == playerCardName)
                {
                    newCard.GetComponent<SpriteRenderer>().sprite = cardPic;
                    break;
                }
            }
            ReorderPlayerHand();
			if(TotalCardCount > 7)
			{
				var console = GameObject.FindGameObjectWithTag("Console").GetComponent<Console>();
				console.AddText(ServerMessageType.SMESSAGE_INFO, "You have over 7 cards. Discard a card to proceed.");
			}
        }
        _playerInfoManager.GetComponent<PlayerInfoManager>()._playerInfos[id].GetComponent<PlayerInfo>().AddPlayerText(playerCardColor, playerCardName);
    }

    public void RemovePlayerCard(int id, PlayerCard playerCard)
    {
        var playerId = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().GetId();
        var playerHand = GameObject.FindGameObjectsWithTag("PlayerCard");
        var cityColor = new CityColor();
        var cityName = "";

        if (playerId == id) { 
            foreach (GameObject card in playerHand)
            {
                if (card.GetComponent<PlayerCardScript>().GetPlayerCard() == playerCard)
                {
                    
                    cityColor = card.GetComponent<PlayerCardScript>().GetCityColor();
                    cityName = EnumToString(playerCard);
                    _playerHandCount[cityColor]--;
					TotalCardCount--;
                    card.tag = "DiscardedPlayerCard";
                    var targetPosition = new Vector3(-740, 520, discardCardOnTop);
                    _animationController.MoveToTarget(card.transform.parent.gameObject, null, targetPosition, 0.5f);
                    discardCardOnTop--;
                    // remove box collider for inspecting discard pile
                    card.GetComponent<BoxCollider2D>().enabled = false;
                    break;
                }
            }
            ReorderPlayerHand();
        }
        _playerInfoManager.GetComponent<PlayerInfoManager>()._playerInfos[id].GetComponent<PlayerInfo>().RemovePlayerText(cityColor, cityName);
    }

    public void ReorderPlayerHand()
    {
        var playerHand = GameObject.FindGameObjectsWithTag("PlayerCard");
        var colorCount = new int[4]; // blue, yellow, black, red 

        foreach (var card in playerHand)
        {
            var color = card.GetComponent<PlayerCardScript>().GetCityColor();
            var targetPosition = new Vector3();
            Debug.Log("Logging _playerHandCount: "  + _playerHandCount[CityColor.CITY_COLOR_BLUE] + " " + _playerHandCount[CityColor.CITY_COLOR_YELLOW] + " " + _playerHandCount[CityColor.CITY_COLOR_BLACK] + " " + _playerHandCount[CityColor.CITY_COLOR_RED]);
            Debug.Log("colorCount: " + colorCount[0] + " " + colorCount[1] + " " + colorCount[2] + " " + colorCount[3]);
            switch (color) 
            {
                case CityColor.CITY_COLOR_BLUE:
                    Debug.Log("DRAWING BLUE: " + colorCount[0]);
                    targetPosition = new Vector3(-770 + (colorCount[0] * 160), 0, -1);       
                    colorCount[0]++;
                    break;

                case CityColor.CITY_COLOR_YELLOW:
                    Debug.Log("DRAWING YELLOW: " + colorCount[1] + " " + _playerHandCount[CityColor.CITY_COLOR_BLUE]);
                    targetPosition = new Vector3(-770 + ((colorCount[1] + _playerHandCount[CityColor.CITY_COLOR_BLUE]) * 160), 0, -1);
                    colorCount[1]++;
                    break;

                case CityColor.CITY_COLOR_BLACK:
                    Debug.Log("DRAWING BLACK: " + colorCount[2] + " " + _playerHandCount[CityColor.CITY_COLOR_BLUE] + " " + _playerHandCount[CityColor.CITY_COLOR_YELLOW]);
                    targetPosition = new Vector3(-770 + ((colorCount[2] + _playerHandCount[CityColor.CITY_COLOR_BLUE] + _playerHandCount[CityColor.CITY_COLOR_YELLOW]) * 160), 0, -1);
                    colorCount[2]++;
                    break;

                case CityColor.CITY_COLOR_RED:
                    Debug.Log("DRAWING RED: " + colorCount[3] + " " + _playerHandCount[CityColor.CITY_COLOR_BLUE] + " " + _playerHandCount[CityColor.CITY_COLOR_YELLOW] + " " + _playerHandCount[CityColor.CITY_COLOR_BLACK]);
                    targetPosition = new Vector3(-770 + ((colorCount[3] + _playerHandCount[CityColor.CITY_COLOR_BLUE] + _playerHandCount[CityColor.CITY_COLOR_YELLOW] + _playerHandCount[CityColor.CITY_COLOR_BLACK]) * 160), 0, -1);
                    colorCount[3]++;
                    break;

                default:
                    Debug.Log("Error with reordering player cards (City color error).");
                    break;
            }
            _animationController.MoveToTarget(card.transform.parent.gameObject, null, targetPosition, 0.5f);
        }
    }
}
