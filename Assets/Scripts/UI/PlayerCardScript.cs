using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCardScript : MonoBehaviour
{
    private PlayerCard _playerCard;
    private CityColor _cityColor;

	public GameController GameController;
	public PlayerHandManager PlayerHandManager;

    public void SetCityColor(CityColor cityColor)
    {
        _cityColor = cityColor;
    }

    public CityColor GetCityColor() 
    { 
        return _cityColor; 
    }
    public void SetPlayerCard(PlayerCard playerCard)
    { 
        _playerCard = playerCard;
    }

    public PlayerCard GetPlayerCard() 
    { 
        return _playerCard;
    }

	void OnMouseDown()
	{
		Debug.Log("ABC");
		if(GameController != null && PlayerHandManager != null)
		{
			Debug.Log("DEF");
			if (PlayerHandManager.TotalCardCount > 7)
			{
				GameController.OpcodeManager.Send(new OutDiscard((byte)GetPlayerCard()));
			}
		}
	}
}
