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
    public GameObject Input;

    public void Start()
    {
    }

    public void AddText(ServerMessageType serverMessageType, string newText, string color) 
    {
        newText = newText + '\n';

        // < color = green > green </ color >

        if (serverMessageType == ServerMessageType.SMESSAGE_CHAT)
        {
            newText = "<color=" + color + ">" + newText;
            int indexOfColon = newText.IndexOf(":");
            //Debug.Log(indexOfColon);
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
        Input.GetComponent<TMP_InputField>().ActivateInputField();
        Input.GetComponent<TMP_InputField>().Select();
    }

}
