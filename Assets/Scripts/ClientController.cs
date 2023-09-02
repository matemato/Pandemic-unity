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
    private int _id;
    private int _numPlayers;

    private GameObject _player;
    private GameObject _otherPlayer;
    private int _otherId;

    [SerializeField]
    private Text _debugText;

    void Start()
    {
        _serverInput = new ServerInput();
        _tcpClient = new TCPClient();
        _msgManager = new MsgManager(_tcpClient);
        _opcodeManager = new OpcodeManager(_msgManager,_serverInput);
        SetCState(ClientState.CSTATE_UNCONNECTED);

        _player = GameObject.Find("Player");
        _otherPlayer = GameObject.Find("OtherPlayer");
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

    private float nextActionTime = 0.0f;
    public float period = 1.0f;

    private void UpdateIngame()
    {
        _opcodeManager.ReceiveAll();

        string serverText = _serverInput.MessageHolder.GetNext();
        if (serverText != null)
        {
            Debug.Log(serverText);
            _debugText.text = serverText;
        }

        byte[] positions = _serverInput.PlayerUpdateHolder.Get();
        if(positions != null)
        {
            if(positions.Length == 2)
            {
                Debug.Log("positions length == 2");
            }
            foreach (GameObject city in GameObject.FindGameObjectsWithTag("Tile"))
            {
                if(city.GetComponent<Tile>().GetId() == positions[_id])
                {
                    _player.GetComponent<Player>().City = city.GetComponent<Tile>();
                }

                if (city.GetComponent<Tile>().GetId() == positions[_otherId])
                {
                    _otherPlayer.GetComponent<OtherPlayer>().City = city.GetComponent<Tile>();
                }
            }
        }

        if(_player.GetComponent<Player>().Click != null)
        {
            OutMove move = new OutMove((byte)_player.GetComponent<Player>().Click._cityId);
            _opcodeManager.Send(move);
            _player.GetComponent<Player>().Click = null;
        }

        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            Debug.Log("test");
            OutIdle idle = new OutIdle();
            _opcodeManager.Send(idle);
        }
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

        if(_serverInput.BeginGameHolder.HasGameStarted())
        {
            _id = _serverInput.BeginGameHolder._playerId;
            _numPlayers = _serverInput.BeginGameHolder._numPlayers;
            _clientState = ClientState.CSTATE_GAME;
            _otherId = 1 - _id;
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
