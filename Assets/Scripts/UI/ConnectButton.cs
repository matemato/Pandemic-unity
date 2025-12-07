using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectButton : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector]
    public bool IsConnectButtonClicked = false;
    public GameObject JoinLobby;
    public GameObject Dropdown;
    public GameObject InputField;
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(ConnectButtonClickedListener);
    }

    private void ConnectButtonClickedListener()
    {
        IsConnectButtonClicked = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
