using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    // Start is called before the first frame update
    int n;
    public GameObject JoinLobbyButton;
    public void OnConnectPress()
    {
        n++;
        Debug.Log("Button clicked " + n + " times.");

        JoinLobbyButton.SetActive(true);
    }
}
