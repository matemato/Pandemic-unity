using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectButton : MonoBehaviour
{
    int n;
    public GameObject JoinLobbyButton;
    public void OnButtonPress()
    {
        n++;
        Debug.Log("Button clicked " + n + " times.");

        JoinLobbyButton.SetActive(true);
    }
}
