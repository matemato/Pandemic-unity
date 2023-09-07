using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    // main menu
    [SerializeField]
    private GameObject _connectButton;
    [SerializeField]
    private GameObject _joinLobbyButton;
    [SerializeField]
    private GameObject _usernameInput;
    [SerializeField]
    private GameObject _dropdownLobby;
    [SerializeField]
    private TMP_Text _lobbyText;

    public bool IsConnectButtonClicked()
    {
        var connectButton = _connectButton.GetComponent<ConnectButtonClicked>();
        bool isClicked = connectButton.IsConnectButtonClicked;
        connectButton.IsConnectButtonClicked = false;
        return isClicked;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetLobbyText("");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetName()
    {
        return _joinLobbyButton.GetComponent<JoinLobbyClicked>().GetName();
    }

    public void SetLobbyText(string str)
    {
        _lobbyText.text = str;
    }

    public void ClearLobbyChoice()
    {
        _joinLobbyButton.GetComponent<JoinLobbyClicked>().ClearLobbyChoice();
    }

    public int GetLobbyChoice()
    {
        return _joinLobbyButton.GetComponent<JoinLobbyClicked>().GetLobbyChoice();
    }

    public void ShowLobby(bool v)
    {
        _joinLobbyButton.SetActive(v);
        _usernameInput.SetActive(v);
        _dropdownLobby.SetActive(v);
    }

    public void ShowConnect(bool v)
    {
        _connectButton.SetActive(v);
    }
}
