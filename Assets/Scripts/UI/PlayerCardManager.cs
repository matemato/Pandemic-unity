using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerCardManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _playerCards = new List<GameObject>();

    [SerializeField]
    private Sprite[] _cardPics;

    private int _numOfCards = 0;

    void Start()
    {
        foreach (GameObject card in _playerCards)
        {
            card.name = String.Empty;
            card.SetActive(false);
        }

        AddPlayerCard(0, PlayerCard.CCARD_ATLANTA);
        AddPlayerCard(0, PlayerCard.CCARD_BANGKOK);
        AddPlayerCard(0, PlayerCard.CCARD_LAGOS);
    }

    void Update()
    {

        if (UnityEngine.Input.GetKeyDown(KeyCode.A))
        {
            AddPlayerCard(0, PlayerCard.CCARD_ATLANTA);
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.R))
        {
            RemovePlayerCard(0, PlayerCard.CCARD_ATLANTA);
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
        string playerCardName = EnumToString(playerCard);

        _playerCards[_numOfCards].name = playerCardName;

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

    public void RemovePlayerCard(int id, PlayerCard playerCard)
    {
        string playerCardName = EnumToString(playerCard);

        foreach (GameObject card in _playerCards)
        {
            if (card.name == playerCardName)
            {
                card.name = String.Empty;
                card.SetActive(false);
            } 
        }
        _numOfCards--;
        ReorderCards();
    }

    public void ReorderCards()
    {
        string[] names = new string[_playerCards.Count];
        for (int i = 0; i < _playerCards.Count; i++)
        {
            names[i] = _playerCards[i].name;
        }

        Array.Sort(names);
        Array.Reverse(names);

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
