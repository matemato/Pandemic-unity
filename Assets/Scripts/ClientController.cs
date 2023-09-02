using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientController : MonoBehaviour
{
    // Start is called before the first frame update
    private ServerInput _serverInput;
    private TCPClient _tcpClient;
    private MsgManager _msgManager;
    private OpcodeManager _opcodeManager;
    private ClientState _clientState = ClientState.CSTATE_UNCONNECTED;
    private int _awaitingSubstate = 0;
    private string test = "sadfsdfdsfdsf";

    [SerializeField]
    private Text _debugText;

    void Start()
    {
        _serverInput = new ServerInput();
        _tcpClient = new TCPClient();
        _msgManager = new MsgManager(_tcpClient);
        _opcodeManager = new OpcodeManager(_msgManager,_serverInput);
        SetCState(ClientState.CSTATE_UNCONNECTED);
    }

    public void SetCState(ClientState newState)
    {
        _clientState = newState;
    }

    public ClientState GetCState()
    {
        return _clientState;
    }

    // Update is called once per frame
    void Update()
    {
        if (_clientState != ClientState.CSTATE_UNCONNECTED)
            _tcpClient.ReadInput();

        switch(_clientState)
        {
            case ClientState.CSTATE_UNCONNECTED:
            {
                UpdateUnconnected();
                break;
            }
            case ClientState.CSTATE_AWAITING:
            {
                UpdateAwaiting();
                break;
            }
            case ClientState.CSTATE_LOBBY:
            {
                UpdateLobby();
                break;
            }
            case ClientState.CSTATE_GAME:
            {
                UpdateIngame();
                break;
            }
            default:
            {
                Debug.LogError("Invalid cstate");
                break;
            }
        }
        if (_clientState != ClientState.CSTATE_UNCONNECTED)
            _tcpClient.SendOutput();
    }

    private void UpdateUnconnected()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("abc");
            if (_tcpClient.ConnectToTcpServer())
            {
                _clientState = ClientState.CSTATE_AWAITING;
            }
            else
            {
                Debug.LogError("Failed to connect to server");
            }
        }
    }

    private void UpdateIngame()
    {
        throw new NotImplementedException();
    }

    private void UpdateLobby()
    {
        _opcodeManager.ReceiveAll();

        string serverText = _serverInput.MessageHolder.GetNext();
        if (serverText != null)
        {
            Debug.Log(serverText);
            _debugText.text = serverText;
        }
    }

    private void UpdateAwaiting()
    {
        if(_awaitingSubstate == 0)
        {
            Debug.Log("waiting for 17");
            if(_msgManager.PendingInput())
            {
                if(_msgManager.ReadByte() == 17)
                {
                    Debug.Log("got 17 - setting substate to 1");
                    _awaitingSubstate = 1;
                }
            }
        }
        else if(_awaitingSubstate == 1)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                _msgManager.WriteByte(14);
                _clientState = ClientState.CSTATE_LOBBY;
            }
        }
    }
}
