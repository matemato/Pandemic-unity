using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _playerName;
    [SerializeField]
    private TMP_Text _playerRole;
    [SerializeField]
    private GameObject _playerRolePic;
    [SerializeField]
    private Sprite[] _rolePics;
    [SerializeField]
    private TMP_Text[] _playerCards;

    private int _numberOfPlayerCards = 0;

    private void Start()
    {

    }

    public void SetPlayerName(string playerName)
    {
        _playerName.text = playerName;
    }
    
    public void SetPlayerRole(string playerRole)
    {
        _playerRole.text = playerRole;
        playerRole = playerRole.Replace(" ", "_").ToLower();
        var playerRolePic = _playerRolePic.GetComponent<SpriteRenderer>();

        foreach(Sprite rolePic in  _rolePics) 
        {
            if (rolePic.name == playerRole)
            {
                playerRolePic.sprite = rolePic;
            }
        }

        //_playerRolePic.sprite = 

    }

    public void AddPlayerCard(string cardName)
    {
        _playerCards[_numberOfPlayerCards].text = cardName;
        _numberOfPlayerCards++;
    }

    public void RemovePlayerCard(int cardNumber)
    {
        _playerCards[cardNumber].text = String.Empty;
        _numberOfPlayerCards--;
    }

    public void ReorderPlayerCards(string[] names)
    {
        for (int i = 0; i < names.Length; i++)
        {
            _playerCards[i].text = names[i];
        }
    }
}
