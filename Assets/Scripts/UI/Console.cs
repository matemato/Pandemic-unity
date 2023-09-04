using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Console : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _consoleText;

    public TMP_InputField SendingText;

    public void Start()
    {
        // _consoleText.text = "YOYOO ROZMAN IN THE HOUSE\n";
        AddText(ServerMessageType.SMESSAGE_CHAT, "yoyo staniè is trash");
        AddText(ServerMessageType.SMESSAGE_CHAT, "yoyo 123 is trash");
    }

    public void AddText(ServerMessageType serverMessageType, string newText) 
    {
        string cleanText = newText + '\n';
        _consoleText.text += cleanText;
    }

    public string GetSendingMessage()
    {
        return SendingText.text;
    }

    public void ClearSendingMessage()
    {
        SendingText.Select();
        SendingText.text = "";
    }

}
