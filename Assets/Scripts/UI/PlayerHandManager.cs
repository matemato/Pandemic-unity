using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerHandManager : MonoBehaviour
{
    private List<GameObject> _playerCards = new List<GameObject>();

    [SerializeField]
    private Sprite[] _cardPics;

    [SerializeField]
    private GameObject _playerCardPrefab;

    [SerializeField]
    private GameObject _playerInfoManager;

    [SerializeField]
    private GameObject _gameControllerObject;

    private GameController _gameController;

    private Dictionary<CityColor, int> PlayerHandCount = new Dictionary<CityColor, int>();

    void Start()
    {
        _gameController =_gameControllerObject.GetComponent<GameController>();

        PlayerHandCount[CityColor.CITY_COLOR_BLUE] = 0;
        PlayerHandCount[CityColor.CITY_COLOR_YELLOW] = 0;
        PlayerHandCount[CityColor.CITY_COLOR_BLACK] = 0;
        PlayerHandCount[CityColor.CITY_COLOR_RED] = 0;
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
            AddPlayerCard(0, PlayerCard.CCARD_ATLANTA);
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.Y))
        {
            AddPlayerCard(0, PlayerCard.CCARD_SAO_PAULO);
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.R))
        {
            var playerHand = GameObject.FindGameObjectsWithTag("PlayerCard");
            RemovePlayerCard(0, playerHand[0].GetComponent<PlayerCardScript>().GetPlayerCard());
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
        int playerHandSize = GameObject.FindGameObjectsWithTag("PlayerCard").Length;

        var cityTiles = GameObject.FindGameObjectsWithTag("Tile");
        CityColor playerCardColor = new CityColor();
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

        _playerInfoManager.GetComponent<PlayerInfoManager>()._playerInfos[id].GetComponent<PlayerInfo>().AddPlayerCard(playerCardName);
                
        if (playerId == id)
        {
            var newCard = Instantiate(_playerCardPrefab, new Vector3(-670 + (playerHandSize * 160), 0, 0), Quaternion.identity);
            newCard.transform.SetParent(gameObject.transform, false);
            newCard = newCard.transform.GetChild(0).gameObject;     
            newCard.GetComponent<PlayerCardScript>().SetPlayerCard(playerCard);
            newCard.GetComponent<PlayerCardScript>().SetCityColor(playerCardColor);
            PlayerHandCount[playerCardColor]++;
            foreach (Sprite cardPic in _cardPics)
            {
                if (cardPic.name == playerCardName)
                {
                    newCard.GetComponent<SpriteRenderer>().sprite = cardPic;
                }
            }
            ReorderPlayerHand();
        }
    }

    public void RemovePlayerCard(int id, PlayerCard playerCard)
    {
        var playerId = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().GetId();
        var playerHand = GameObject.FindGameObjectsWithTag("PlayerCard");

        var cardPosition = 0;
        if (playerId == id) { 
            foreach (GameObject card in playerHand)
            {
                if (card.GetComponent<PlayerCardScript>().GetPlayerCard() == playerCard)
                {
                    PlayerHandCount[card.GetComponent<PlayerCardScript>().GetCityColor()]--;
                    DestroyImmediate(card.transform.parent.gameObject);
                    break;
                }
                cardPosition++;
            }
            ReorderPlayerHand();
        }
        _playerInfoManager.GetComponent<PlayerInfoManager>()._playerInfos[id].GetComponent<PlayerInfo>().RemovePlayerCard(cardPosition);

        
    }

    public void ReorderPlayerHand()
    {
        var playerHand = GameObject.FindGameObjectsWithTag("PlayerCard");
        var colorCount = new int[4]; // blue, yellow, black, red 

        foreach(var card in playerHand)
        {
            Debug.Log("CARD NAME: " + card.GetComponent<PlayerCardScript>().GetPlayerCard());
        }
        
        foreach(var count in PlayerHandCount)
        {
            Debug.Log("PlayerHandCount: " + count);
        }

        foreach (var card in playerHand)
        {
            var color = card.GetComponent<PlayerCardScript>().GetCityColor();
            var parent = card.transform.parent.gameObject;

            switch (color) 
            {
                case CityColor.CITY_COLOR_BLUE:
                    parent.transform.localPosition = new Vector3(-670 + (colorCount[0] * 160), 0, 0);
                    colorCount[0]++;
                    break;

                case CityColor.CITY_COLOR_YELLOW:
                    parent.transform.localPosition = new Vector3(-670 + ((colorCount[1] + PlayerHandCount[CityColor.CITY_COLOR_BLUE]) * 160), 0, 0);
                    colorCount[1]++;
                    break;

                case CityColor.CITY_COLOR_BLACK:
                    parent.transform.localPosition = new Vector3(-670 + ((colorCount[2] + PlayerHandCount[CityColor.CITY_COLOR_BLUE] + PlayerHandCount[CityColor.CITY_COLOR_YELLOW]) * 160), 0, 0);
                    colorCount[2]++;
                    break;

                case CityColor.CITY_COLOR_RED:
                    parent.transform.localPosition = new Vector3(-670 + ((colorCount[3] + PlayerHandCount[CityColor.CITY_COLOR_BLUE] + PlayerHandCount[CityColor.CITY_COLOR_YELLOW] + PlayerHandCount[CityColor.CITY_COLOR_BLACK]) * 160), 0, 0);
                    colorCount[3]++;
                    break;

                default:
                    Debug.Log("Error with reordering player cards (City color error).");
                    break;
            }
        }
    }


    /*public void ReorderCards(int id)
    {
        string[] names = new string[_playerCards.Count];
        for (int i = 0; i < _playerCards.Count; i++)
        {
            names[i] = _playerCards[i].name;
        }

        Array.Sort(names);
        Array.Reverse(names);

        _playerInfoManager.GetComponent<PlayerInfoManager>()._playerInfos[id].GetComponent<PlayerInfo>().ReorderPlayerCards(names);

        for (int i = 0; i < _playerCards.Count; i++)
        {
            _playerCards[i].name = names[i];

            if (_playerCards[i].name == String.Empty)
            {
                _playerCards[i].SetActive(false);
            }
            else
            {
                foreach (Sprite cardPic in _cardPics)
                {
                    if (cardPic.name == _playerCards[i].name)
                    {
                        _playerCards[i].GetComponent<SpriteRenderer>().sprite = cardPic;
                    }
                }
                _playerCards[i].SetActive(true);
            }
        }

    }
    */
}
