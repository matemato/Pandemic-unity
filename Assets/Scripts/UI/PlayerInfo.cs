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
    }

    public void UpdatePlayerCard(int position, string name, string color)
    {
        if (name == String.Empty)
        {
            _playerCards[position].text = String.Empty;
        }
        else
        {
            _playerCards[position].text = "<color=" + color + ">" + name + "</color>";
        }
    }
}
