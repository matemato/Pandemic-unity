using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class Console : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _allText; 
    [SerializeField]
    private TMP_Text _chatText;    
    [SerializeField]
    private TMP_Text _consoleText;

    public TMP_InputField SendingText;
    public GameObject Container;
    public GameObject InputField;

    [SerializeField]
    private GameObject _controller;

    private MainMenuController _mainMenuController;
    private GameController _gameController;

    void Start()
    {
        if(_controller.GetComponent<GameController>() != null)
        {
            _gameController = _controller.GetComponent<GameController>();
        }
        else
        {
            _mainMenuController = _controller.GetComponent<MainMenuController>();
        }
    }

    void Update()
    {
        ServerInput serverInput = null;
        if(_mainMenuController != null)
        {
            serverInput = _mainMenuController.ServerInput;
        }
        else if(_gameController != null)
        {
            serverInput = _gameController.ServerInput;
        }

        if(serverInput != null)
        {
            var serverText = serverInput.MessageHolder.GetNextConsoleMessage();
            if (serverText != null)
            {
                if (serverText.Item1 == ServerMessageType.SMESSAGE_CHAT || serverText.Item1 == ServerMessageType.SMESSAGE_INFO)
                {
                    AddText(serverText.Item1, serverText.Item2, "yellow");
                }
            }
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Return))
        {
            SendChatMessage();
        }
        

    }

    private void SendChatMessage()
    {
        if (SendingText.text.Length > 0)
        {
            OpcodeManager opcodeManager;

            if (_mainMenuController != null)
            {
                opcodeManager = _mainMenuController.OpcodeManager;
            }
            else
            {
                opcodeManager = _gameController.OpcodeManager;
            }

            OutClientMessage msg = new OutClientMessage(ClientMessageType.CMESSAGE_CHAT, GetSendingMessage());
            opcodeManager.Send(msg);
            ClearSendingMessage();
        }
    }

    public void AddText(ServerMessageType serverMessageType, string newText, string color = "") 
    {
        newText = newText + '\n';

        // < color = green > green </ color >

        if (serverMessageType == ServerMessageType.SMESSAGE_CHAT)
        {
            newText = "<color=" + color + ">" + newText;
            int indexOfColon = newText.IndexOf(":");
            if (indexOfColon >= 0)
            {
                newText = newText.Insert(indexOfColon, "</color>");
            }
            _chatText.text += newText;
        }
        else if (serverMessageType == ServerMessageType.SMESSAGE_INFO) 
        {
            _consoleText.text += newText;
        } 

        _allText.text += newText;
        Container.GetComponent<ScrollRect>().velocity = new Vector2(0f, 1000f);
    }

    public string GetSendingMessage()
    {
        return SendingText.text;
    }

    public void ClearSendingMessage()
    {
        SendingText.Select();
        SendingText.text = "";
        InputField.GetComponent<TMP_InputField>().ActivateInputField();
        InputField.GetComponent<TMP_InputField>().Select();
    }

}
