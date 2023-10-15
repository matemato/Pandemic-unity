using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField]
    public GameObject Console;

    public ServerInput ServerInput;

    public bool IsConnectButtonClicked()
    {
        var connectButton = _connectButton.GetComponent<ConnectButton>();
        bool isClicked = connectButton.IsConnectButtonClicked;
        connectButton.IsConnectButtonClicked = false;
        return isClicked;
    }

    public string GetIp()
    {
        var connectButton = _connectButton.GetComponent<ConnectButton>();
        if (connectButton.Toggle.GetComponent<Toggle>().isOn)
        {
            return "localhost";
        }
        else
        {
            return "93.103.52.248";
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetLobbyText("");
    }

    // Update is called once per frame
    void Update()
    {
        if (ServerInput != null)
        {
            var serverText = ServerInput.MessageHolder.GetNextLobbyText();
            if (serverText != null)
            {
                SetLobbyText(serverText);
            }
        }
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

    public void ShowLobbyJoin(bool v)
    {
        _joinLobbyButton.SetActive(v);
        _usernameInput.SetActive(v);
        _dropdownLobby.SetActive(v);
    }



    public void ShowConnect(bool v)
    {
        _connectButton.SetActive(v);
        _connectButton.GetComponent<ConnectButton>().Toggle.SetActive(v);
    }

    public void SetConnectInteractable(bool v)
    {
        _connectButton.GetComponent<Button>().interactable = v;
    }
}
