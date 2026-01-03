using System;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using Color = UnityEngine.Color;

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
    private GameObject _cardTextPrefab;
	[SerializeField]
	private TMP_Text _actionText;
	[SerializeField]
	private GameObject _activeTurn;

	private List<TMP_Text> _cardTexts = new List<TMP_Text>();
    private List<PlayerCard> _playerCards = new List<PlayerCard>();

	public int Actions { get; private set; } = 0;

	private Dictionary<CityColor, int> _playerHandCount = new Dictionary<CityColor, int>
        {
            { CityColor.CITY_COLOR_BLUE, 0 },
            { CityColor.CITY_COLOR_YELLOW, 0 },
            { CityColor.CITY_COLOR_BLACK, 0 },
            { CityColor.CITY_COLOR_RED, 0 }
        };

    private void Start()
    {
		_activeTurn.SetActive(false);
		_actionText.text = "";
    }

    public void SetPlayerName(string playerName, int id)
    {
        _playerName.text = playerName;
		if (id == 0)
			_playerName.color = Color.yellow;
		else if (id == 1)
			_playerName.color = Color.violet;
		else if (id == 2)
			_playerName.color = Color.green;
		else if (id == 3)
			_playerName.color = Color.orange;
	}

    public string GetPlayerName()
    {
        return _playerName.text;
    }

    public void SetPlayerRole(string playerRole)
    {
        _playerRole.text = playerRole;
        playerRole = playerRole.Replace(" ", "_").ToLower();
        var playerRolePic = _playerRolePic.GetComponent<SpriteRenderer>();

        foreach (Sprite rolePic in _rolePics)
        {
            if (rolePic.name == playerRole)
            {
                playerRolePic.sprite = rolePic;
            }
        }
    }

    public void AddPlayerCard(PlayerCard playerCard)
    {
        _playerCards.Add(playerCard);
    }

    public void RemovePlayerCard(PlayerCard playerCard)
    {
        _playerCards.Remove(playerCard);
    }

    public void AddPlayerText(CityColor cityColor, string cardName)
    {
        var newText = Instantiate(_cardTextPrefab, new Vector3(125, 45, -1), Quaternion.identity);
        newText.transform.SetParent(gameObject.transform, false);
        var text = newText.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
        _cardTexts.Add(text);

        var color = GetCardColor(cityColor);

        text.text = cardName;
        text.color = color;
        _playerHandCount[cityColor]++;

        ReorderPlayerTexts();
    }

    public void RemovePlayerText(CityColor cityColor, string cardName)
    {
        foreach (var cardText in _cardTexts)
        {
            if (cardText.text == cardName)
            {
                _playerHandCount[cityColor]--;
                _cardTexts.Remove(cardText);
                DestroyImmediate(cardText.transform.parent.gameObject);
                break;
            }
        }

        ReorderPlayerTexts();
    }

    public void ReorderPlayerTexts()
    {
        var colorCount = new int[4]; // blue, yellow, black, red 

        foreach (TMP_Text cardText in _cardTexts)
        {
            var parent = cardText.transform.parent.gameObject;
            if (AreColorsEqual(cardText.color, UnityEngine.Color.blue))
            {
                parent.transform.localPosition = new Vector3(parent.transform.localPosition.x, 45 - 15 * (colorCount[0]), parent.transform.localPosition.z);
                colorCount[0]++;
            }
            else if (AreColorsEqual(cardText.color, UnityEngine.Color.yellow))
            {
                parent.transform.localPosition = new Vector3(parent.transform.localPosition.x, 45 - 15 * (colorCount[1] + _playerHandCount[CityColor.CITY_COLOR_BLUE]), parent.transform.localPosition.z);
                colorCount[1]++;
            }
            else if (AreColorsEqual(cardText.color, UnityEngine.Color.black))
            {
                parent.transform.localPosition = new Vector3(parent.transform.localPosition.x, 45 - 15 * (colorCount[2] + _playerHandCount[CityColor.CITY_COLOR_BLUE] + _playerHandCount[CityColor.CITY_COLOR_YELLOW]), parent.transform.localPosition.z);
                colorCount[2]++;
            }
            else if (AreColorsEqual(cardText.color, UnityEngine.Color.red))
            {
                parent.transform.localPosition = new Vector3(parent.transform.localPosition.x, 45 - 15 * (colorCount[3] + _playerHandCount[CityColor.CITY_COLOR_BLUE] + _playerHandCount[CityColor.CITY_COLOR_YELLOW] + _playerHandCount[CityColor.CITY_COLOR_BLACK]), parent.transform.localPosition.z);
                colorCount[3]++;
            }
        }
    }

    public bool AreColorsEqual(UnityEngine.Color color1, UnityEngine.Color color2)
    {
        return color1.r == color2.r && color1.g == color2.g && color1.b == color2.b && color1.a == color2.a;
    }

    public UnityEngine.Color GetCardColor(CityColor cityColor)
    {
        switch (cityColor)
        {
            case CityColor.CITY_COLOR_BLUE:
                return UnityEngine.Color.blue;

            case CityColor.CITY_COLOR_YELLOW:
                return UnityEngine.Color.yellow;

            case CityColor.CITY_COLOR_BLACK:
                return UnityEngine.Color.black;

            case CityColor.CITY_COLOR_RED:
                return UnityEngine.Color.red;

            default:
                Debug.Log("YO WRONG COLOR");
                return new UnityEngine.Color();
        }
    }

	internal void BeginTurn(int actions)
	{
		UpdateActions(actions);
		_activeTurn.SetActive(true);
		_activeTurn.GetComponent<SpriteRenderer>().color = Color.green;
	}
	internal void EndTurn()
	{
		UpdateActions(0);
		_activeTurn.SetActive(true);
		_activeTurn.GetComponent<SpriteRenderer>().color = Color.red;
	}
	internal void HideActiveSign()
	{
		_activeTurn.SetActive(false);
		_actionText.text = "";
	}
	internal void UpdateActions(int actions)
	{
		Actions = actions;
		_actionText.text = $"Actions: {actions}";
	}
}
