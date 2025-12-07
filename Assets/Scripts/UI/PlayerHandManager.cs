using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;
using static BoardComponentPositions;

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

    [SerializeField]
    private GameObject _playerDiscardPileObject;

    [SerializeField]
    private GameObject _infectionManagerObject;


    private GameController _gameController;
    private AnimationController _animationController;
    private PlayerCardDiscardPileController _playerDiscardPileController;
    private InfectionManager _infectionManager;
    private int discardCardOnTop = 0;

    public int TotalCardCount = 0;
    private Console _console;

    private Dictionary<CityColor, int> _playerHandCount = new Dictionary<CityColor, int>
        {
            { CityColor.CITY_COLOR_BLUE, 0 },
            { CityColor.CITY_COLOR_YELLOW, 0 },
            { CityColor.CITY_COLOR_BLACK, 0 },
            { CityColor.CITY_COLOR_RED, 0 }
        };

    void Start()
    {
        _console = GameObject.FindGameObjectWithTag("Console").GetComponent<Console>();
        _gameController =_gameControllerObject.GetComponent<GameController>();
        _animationController =_animationControllerObject.GetComponent<AnimationController>();
        _playerDiscardPileController = _playerDiscardPileObject.GetComponent<PlayerCardDiscardPileController>();
        _infectionManager = _infectionManagerObject.GetComponent<InfectionManager>();
    }

    void Update()
    {
        if (_gameController.ServerInput != null)
        {
            // Process player card updates
            var cardRequest = _gameController.ServerInput.PlayerCardUpdateHolder.GetNext();

            if (cardRequest != null)
            {
                int id = (int)cardRequest.Item1;
                bool remove = cardRequest.Item2;
                PlayerCard playerCard = cardRequest.Item3;

                if (remove)
                {
                    RemovePlayerCard(id, playerCard);
                }
                else
                {
                    AddPlayerCard(id, playerCard);
                }
            }

            // Process epidemic draw requests
            var epidemicRequest = _gameController.ServerInput.EpidemicHolder.PendingEpidemic();
            if (epidemicRequest)
            {
                TriggerEpidemicDraw();
            }
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.E))
        {
            TriggerEpidemicDraw();
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

    public CityColor? GetCityColor(PlayerCard playerCard) 
    {
        var cityTiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (var tile in cityTiles)
        {
            Tile tileScript = tile.GetComponent<Tile>();
            if (tileScript.PlayerCard == playerCard)
            {
                return tileScript.CityColor;
            }
        }
        return null;
    }

    public GameObject CreatePlayerCard(PlayerCard playerCard, Vector3 position, CityColor? cityColor) 
    {
        string cityName = EnumToString(playerCard);
        var newCard = Instantiate(_playerCardPrefab, position, Quaternion.identity);
        newCard.transform.SetParent(gameObject.transform, false);
        newCard = newCard.transform.GetChild(0).gameObject;
        newCard.GetComponent<PlayerCardScript>().SetPlayerCard(playerCard);
        if (cityColor != null)
        {
            newCard.GetComponent<PlayerCardScript>().SetCityColor((CityColor)cityColor);
        }
        newCard.GetComponent<PlayerCardScript>().GameController = _gameControllerObject.GetComponent<GameController>();
        newCard.GetComponent<PlayerCardScript>().PlayerHandManager = this;
        foreach (Sprite cardPic in _cardPics)
        {
            if (cardPic.name == cityName)
            {
                newCard.GetComponent<SpriteRenderer>().sprite = cardPic;
                break;
            }
        }
        return newCard;
    }

    public void TriggerEpidemicDraw()
    {
        StartCoroutine(TriggerEpidemicDrawRoutine());
        _infectionManager.TriggerEpidemicSequence();
    }

    public IEnumerator TriggerEpidemicDrawRoutine()
    {
        // create epidemic card and add to discard pile
        var epidemicCard = CreatePlayerCard(PlayerCard.CCARD_EPIDEMIC, PlayerCardDeckPosition, null);
        epidemicCard.tag = "DiscardedPlayerCard";
        epidemicCard.GetComponent<BoxCollider2D>().enabled = false;
        _playerDiscardPileController.AddToDiscardPile(PlayerCard.CCARD_EPIDEMIC);
        discardCardOnTop--;
        _console.AddText(ServerMessageType.SMESSAGE_INFO, "Epidemic card has been drawn!");

        // animate epidemic card
        _animationController.MoveToTarget(epidemicCard.transform.parent.gameObject, null, BoardCenterPosition - PlayerHandPosition, 1f, null, PlayerCardEnlargedScale);
        yield return new WaitForSeconds(1.5f);
        _animationController.MoveToTarget(epidemicCard.transform.parent.gameObject, null, new Vector3(PlayerCardDiscardPilePosition.x, PlayerCardDiscardPilePosition.y, discardCardOnTop), 0.5f, PlayerCardEnlargedScale, PlayerCardScale);

    }


    public void AddPlayerCard(int id, PlayerCard playerCard)
    {
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        CityColor? cityColor = GetCityColor(playerCard);
        if (cityColor == null)
        {
            Debug.Log("No cityColor found.");
            return;
        }
        string cityName = EnumToString(playerCard);
             
        if (player.GetId() == id)
        {
            var newCard = CreatePlayerCard(playerCard, PlayerCardDeckPosition, (CityColor)cityColor);
            _playerHandCount[(CityColor)cityColor]++;
			TotalCardCount++;
            player.AddCardToHand(newCard);
            ReorderPlayerHand();
			if(TotalCardCount > 7)
			{
				var console = GameObject.FindGameObjectWithTag("Console").GetComponent<Console>();
				console.AddText(ServerMessageType.SMESSAGE_INFO, "You have over 7 cards. Discard a card to proceed.");
			}
        }
        _playerInfoManager.GetComponent<PlayerInfoManager>()._playerInfos[id].GetComponent<PlayerInfo>().AddPlayerText((CityColor)cityColor, cityName);
        _playerInfoManager.GetComponent<PlayerInfoManager>()._playerInfos[id].GetComponent<PlayerInfo>().AddPlayerCard(playerCard);
    }

    public void RemovePlayerCard(int id, PlayerCard playerCard)
    {
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        var playerHand = player.GetPlayerHand();
        CityColor? cityColor = GetCityColor(playerCard);
        if (cityColor == null)
        {
            Debug.Log("No cityColor found.");
            return;
        }
        string cityName = EnumToString(playerCard);

        if (player.GetId() == id) { 
            foreach (GameObject card in playerHand)
            {
                if (card.GetComponent<PlayerCardScript>().GetPlayerCard() == playerCard)
                {
                    _playerHandCount[(CityColor)cityColor]--;
					TotalCardCount--;
                    card.tag = "DiscardedPlayerCard";
                    var targetPosition = new Vector3(PlayerCardDiscardPilePosition.x, PlayerCardDiscardPilePosition.y, discardCardOnTop);
                    _animationController.MoveToTarget(card.transform.parent.gameObject, null, targetPosition, 0.5f);
                    discardCardOnTop--;
                    player.RemoveCardFromHand(card);
                    // remove box collider for inspecting discard pile
                    card.GetComponent<BoxCollider2D>().enabled = false;
                    break;
                }
            }
            ReorderPlayerHand();
        }
        else
        {
            // Create the card and add it to the discard pile visually for other players
            var newCard = CreatePlayerCard(playerCard, new Vector3(PlayerCardDiscardPilePosition.x, PlayerCardDiscardPilePosition.y, discardCardOnTop), (CityColor)cityColor);
            newCard.GetComponent<BoxCollider2D>().enabled = false;
            discardCardOnTop--;
        }
        _playerDiscardPileController.AddToDiscardPile(playerCard);
        _playerInfoManager.GetComponent<PlayerInfoManager>()._playerInfos[id].GetComponent<PlayerInfo>().RemovePlayerText((CityColor)cityColor, cityName);
        _playerInfoManager.GetComponent<PlayerInfoManager>()._playerInfos[id].GetComponent<PlayerInfo>().RemovePlayerCard(playerCard);
    }

    public void ReorderPlayerHand()
    {
        var playerHand = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().GetPlayerHand();
        var colorCount = new int[4]; // blue, yellow, black, red 

        foreach (var card in playerHand)
        {
            var color = card.GetComponent<PlayerCardScript>().GetCityColor();
            var targetPosition = new Vector3();
            switch (color) 
            {
                case CityColor.CITY_COLOR_BLUE:
                    targetPosition = PlayerHandStartingPosition + colorCount[0] * PlayerHandCardOffset;
                    colorCount[0]++;
                    break;

                case CityColor.CITY_COLOR_YELLOW:
                    targetPosition = PlayerHandStartingPosition + ((colorCount[1] + _playerHandCount[CityColor.CITY_COLOR_BLUE]) * PlayerHandCardOffset);
                    colorCount[1]++;
                    break;

                case CityColor.CITY_COLOR_BLACK:
                    targetPosition = PlayerHandStartingPosition + ((colorCount[2] + _playerHandCount[CityColor.CITY_COLOR_BLUE] + _playerHandCount[CityColor.CITY_COLOR_YELLOW]) * PlayerHandCardOffset);
                    colorCount[2]++;
                    break;

                case CityColor.CITY_COLOR_RED:
                    targetPosition = PlayerHandStartingPosition + ((colorCount[3] + _playerHandCount[CityColor.CITY_COLOR_BLUE] + _playerHandCount[CityColor.CITY_COLOR_YELLOW] + _playerHandCount[CityColor.CITY_COLOR_BLACK]) * PlayerHandCardOffset);
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
