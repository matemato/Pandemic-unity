using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyClicked : MonoBehaviour
{
    public TMP_Dropdown LobbyDropdown;

    private int _lobbyChoice = -1;
    private int _dropdownValue = 0;
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
        _lobbyChoice = _dropdownValue;
    }

    public int GetLobbyChoice() 
    {
        return _lobbyChoice;
    }
}
