using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyClicked : MonoBehaviour
{
    public TMP_Dropdown LobbyDropdown;
    public TMP_InputField InputField;
    public const int MAX_NAME_LEN = 29;

    private int _lobbyChoice = -1;
    private int _dropdownValue = 0;
    private string _name = "";
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(JoinButtonClicked);
        LobbyDropdown.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged(LobbyDropdown);
        });
    }

    void DropdownValueChanged(TMP_Dropdown change)
    {
        _dropdownValue = change.value;
    }

    private void JoinButtonClicked()
    {
        _name = InputField.text;
        if(_name.Length < 3 || _name.Length > 29)
        {
            Debug.Log("Invalid name input");
        }
        else
        {
            _lobbyChoice = _dropdownValue;
        }
    }

    public string GetName()
    {
        return _name;
    }

    public int GetLobbyChoice() 
    {
        return _lobbyChoice;
    }

    public void ClearLobbyChoice()
    {
        _lobbyChoice = -1;
    }
}
