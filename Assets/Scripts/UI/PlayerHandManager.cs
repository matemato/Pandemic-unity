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

        if (UnityEngine.Input.GetKeyDown(KeyCode.B))
        {
            AddPlayerCard(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().GetId(), PlayerCard.CCARD_ATLANTA);
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.Y))
        {
            AddPlayerCard(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().GetId(), PlayerCard.CCARD_SAO_PAULO);
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.R))
        {
            var playerHand = GameObject.FindGameObjectsWithTag("PlayerCard");
            RemovePlayerCard(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().GetId(), playerHand[0].GetComponent<PlayerCardScript>().GetPlayerCard());
        }
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
            _playerHandCount[playerCardColor]++;
            foreach (Sprite cardPic in _cardPics)
            {
                if (cardPic.name == playerCardName)
                {
                    newCard.GetComponent<SpriteRenderer>().sprite = cardPic;
                    break;
                }
            }
            ReorderPlayerHand();
        }
        _playerInfoManager.GetComponent<PlayerInfoManager>()._playerInfos[id].GetComponent<PlayerInfo>().AddPlayerText(playerCardColor, playerCardName);
    }

    public void RemovePlayerCard(int id, PlayerCard playerCard)
    {
        var playerId = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().GetId();
        var playerHand = GameObject.FindGameObjectsWithTag("PlayerCard");
        var cityColor = new CityColor();
        var cityName = "";

        var cardPosition = 0;
        if (playerId == id) { 
            foreach (GameObject card in playerHand)
            {
                if (card.GetComponent<PlayerCardScript>().GetPlayerCard() == playerCard)
                {
                    cityColor = card.GetComponent<PlayerCardScript>().GetCityColor();
                    cityName = EnumToString(playerCard);
                    _playerHandCount[cityColor]--;
                    card.tag = "DiscardedPlayerCard";
                    var targetPosition = new Vector3(-740, 520, discardCardOnTop);
                    _animationController.MoveToTarget(card.transform.parent.gameObject, null, targetPosition, 0.5f);
                    discardCardOnTop--;
                    //DestroyImmediate(card.transform.parent.gameObject);
                    break;
                }
                cardPosition++;
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
            switch (color) 
            {
                case CityColor.CITY_COLOR_BLUE:
                    targetPosition = new Vector3(-670 + (colorCount[0] * 160), 0, -1);       
                    colorCount[0]++;
                    break;

                case CityColor.CITY_COLOR_YELLOW:
                    targetPosition = new Vector3(-670 + ((colorCount[1] + _playerHandCount[CityColor.CITY_COLOR_BLUE]) * 160), 0, -1);
                    colorCount[1]++;
                    break;

                case CityColor.CITY_COLOR_BLACK:
                    targetPosition = new Vector3(-670 + ((colorCount[2] + _playerHandCount[CityColor.CITY_COLOR_BLUE] + _playerHandCount[CityColor.CITY_COLOR_YELLOW]) * 160), 0, -1);
                    colorCount[2]++;
                    break;

                case CityColor.CITY_COLOR_RED:
                    targetPosition = new Vector3(-670 + ((colorCount[3] + _playerHandCount[CityColor.CITY_COLOR_BLUE] + _playerHandCount[CityColor.CITY_COLOR_YELLOW] + _playerHandCount[CityColor.CITY_COLOR_BLACK]) * 160), 0, -1);
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
