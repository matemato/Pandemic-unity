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

    [SerializeField]
    private GameObject _playerPrefab;
    [SerializeField]
    private GameObject _otherPlayerPrefab;

    private GameObject _player;
    private GameObject[] _otherPlayers;
    private GameObject[] _tiles;

    [SerializeField]
    private Text _debugText;

    private float nextIdleTime = 0.0f;

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

    private void UpdatePlayerPositions()
    {
        byte[] positions = _serverInput.PlayerUpdateHolder.Get();
        if (positions != null)
        {
            foreach (GameObject city in _tiles)
            {
                if (city.GetComponent<Tile>().GetId() == positions[_id])
                {
                    _player.GetComponent<Player>().City = city.GetComponent<Tile>();
                }

                foreach (GameObject otherPlayer in _otherPlayers)
                {
                    if (city.GetComponent<Tile>().GetId() == positions[otherPlayer.GetComponent<OtherPlayer>().GetId()])
                    {
                        otherPlayer.GetComponent<OtherPlayer>().City = city.GetComponent<Tile>();
                    }
                }

            }
        }
    }

    private void UpdateIngame()
    {
        const float idleSendPeriod = 1.0f;

        _opcodeManager.ReceiveAll();

        string serverText = _serverInput.MessageHolder.GetNext();
        if (serverText != null)
        {
            Debug.Log(serverText);
            _debugText.text = serverText;
        }

        UpdatePlayerPositions();

        if(_player.GetComponent<Player>().Click != null)
        {
            OutMove move = new OutMove((byte)_player.GetComponent<Player>().Click._cityId);
            _opcodeManager.Send(move);
            _player.GetComponent<Player>().Click = null;
        }

        if (Time.time > nextIdleTime)
        {
            nextIdleTime += idleSendPeriod;
            OutIdle idle = new OutIdle();
            _opcodeManager.Send(idle);
        }
    }

    private void BeginGame()
    {
        _id = _serverInput.BeginGameHolder._playerId;
        _numPlayers = _serverInput.BeginGameHolder._numPlayers;
        _clientState = ClientState.CSTATE_GAME;

        for (int i = 0; i < _numPlayers; i++)
        {
            if (i == _id)
            {
                var player = Instantiate(_playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                player.GetComponent<Player>().SetId(i);
            }
            else
            {
                var otherPlayer = Instantiate(_otherPlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                otherPlayer.GetComponent<OtherPlayer>().SetId(i);
            }
        }

        _otherPlayers = GameObject.FindGameObjectsWithTag("OtherPlayer");
        _player = GameObject.FindGameObjectWithTag("Player");
        _tiles = GameObject.FindGameObjectsWithTag("Tile");
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
            BeginGame();
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
