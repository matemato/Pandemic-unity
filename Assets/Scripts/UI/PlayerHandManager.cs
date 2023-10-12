using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerHandManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _playerCards = new List<GameObject>();

    [SerializeField]
    private Sprite[] _cardPics;

    [SerializeField]
    private GameObject _playerInfoManager;

    [SerializeField]
    private GameObject _gameControllerObject;

    private GameController _gameController;

    private int _numOfCards = 0;

    void Start()
    {
        foreach (GameObject card in _playerCards)
        {
            card.name = String.Empty;
            card.SetActive(false);
        }

        _gameController =_gameControllerObject.GetComponent<GameController>();

        //AddPlayerCard(0, PlayerCard.CCARD_ATLANTA);
        //AddPlayerCard(0, PlayerCard.CCARD_BANGKOK);
        //AddPlayerCard(0, PlayerCard.CCARD_LAGOS);
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

        if (UnityEngine.Input.GetKeyDown(KeyCode.A))
        {
            AddPlayerCard(0, PlayerCard.CCARD_ATLANTA);
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.R))
        {
            //RemovePlayerCard(0, PlayerCard.CCARD_ATLANTA);
            RemoveTest(0);
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

        string playerCardName = EnumToString(playerCard);
        _playerCards[_numOfCards].name = playerCardName;
        _playerInfoManager.GetComponent<PlayerInfoManager>()._playerInfos[id].GetComponent<PlayerInfo>().AddPlayerCard(_numOfCards, playerCardName);

        if (playerId == id)
        {
            foreach (Sprite cardPic in _cardPics)
            {
                if (cardPic.name == playerCardName)
                {
                    _playerCards[_numOfCards].GetComponent<SpriteRenderer>().sprite = cardPic;
                }
            }

            _playerCards[_numOfCards].SetActive(true);
            _numOfCards++;
        }
    }

    public void RemovePlayerCard(int id, PlayerCard playerCard)
    {
        var playerId = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().GetId();

        string playerCardName = EnumToString(playerCard);

        var i = 0;
        foreach (GameObject card in _playerCards)
        {
            if (card.name == playerCardName)
            {
                card.name = String.Empty;
                card.SetActive(false);

                _playerInfoManager.GetComponent<PlayerInfoManager>()._playerInfos[id].GetComponent<PlayerInfo>().RemovePlayerCard(i);
            } 
            i++;
        }
        _numOfCards--;
        ReorderCards(id);
    }

    public void RemoveTest(int id)
    {
        _numOfCards--;
        _playerCards[0].name = String.Empty;
        _playerCards[0].SetActive(false);

        ReorderCards(0);
    }

    public void ReorderCards(int id)
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

}
