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
    private TMP_Text _consoleText;

    public TMP_InputField SendingText;
    public GameObject Container;
    public GameObject Input;

    public void Start()
    {
        // _consoleText.text = "YOYOO ROZMAN IN THE HOUSE\n";
        AddText(ServerMessageType.SMESSAGE_CHAT, "stani�: you nigga", "green");
        AddText(ServerMessageType.SMESSAGE_CHAT, "mi�o: you gay", "yellow");
    }

    public void AddText(ServerMessageType serverMessageType, string newText, string color) 
    {
        string cleanText = newText + '\n';

        // < color = green > green </ color >

        if (serverMessageType == ServerMessageType.SMESSAGE_CHAT)
        {
            cleanText = "<color=" + color + ">" + cleanText;
            int indexOfColon = cleanText.IndexOf(":");
            Debug.Log(indexOfColon);
            if (indexOfColon >= 0)
            {
                cleanText = cleanText.Insert(indexOfColon, "</color>");
            }      
        }

        _consoleText.text += cleanText;
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
