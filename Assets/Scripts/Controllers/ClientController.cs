using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClientController : MonoBehaviour
{
    // Start is called before the first frame update
    private ServerInput _serverInput;
    private TCPClient _tcpClient;
    private MsgManager _msgManager;
    private OpcodeManager _opcodeManager;

    private ClickManager _clickManager;

    private ClientState _clientState = ClientState.CSTATE_UNCONNECTED;
    private int _awaitingSubstate = 0;
    private int _id;
    private int _numPlayers;

    [SerializeField]
    private MainMenuController _mainMenuController;
    private GameController _gameController;

    private GameObject _player;
    private GameObject[] _otherPlayers;
    private GameObject[] _tiles;

    private GameObject _playerInfoManager;

    [SerializeField]
    private string _ip = "localhost";

    private float nextIdleTime = 0.0f;

    private Console _console;

    private int _loaded = 0;    
    void Start()
    {
        _serverInput = new ServerInput();
        _tcpClient = new TCPClient();
        _msgManager = new MsgManager(_tcpClient);
        _opcodeManager = new OpcodeManager(_msgManager, _serverInput);
        SetCState(ClientState.CSTATE_UNCONNECTED);
        _mainMenuController.GetComponent<MainMenuController>();
        _mainMenuController.ServerInput = _serverInput;
        _console = _mainMenuController.Console.GetComponent<Console>();
    }

    void InitializeGameObjects() 
    {
        _clickManager = GameObject.Find("ClickController").GetComponent<ClickManager>();
        _console = GameObject.Find("Console").GetComponent<Console>();
        _playerInfoManager = GameObject.Find("PlayerInfoManager");
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();
        _gameController.ServerInput = _serverInput;
    }
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
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

        switch (_clientState)
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
        if (_clientState == ClientState.CSTATE_UNCONNECTED && _mainMenuController.IsConnectButtonClicked())
        {
            _mainMenuController.SetConnectInteractable(false);
            if (_tcpClient.ConnectToTcpServer(_ip))
            {
                _mainMenuController.ShowLobbyJoin(true);
                _mainMenuController.ShowConnect(false);
                _clientState = ClientState.CSTATE_AWAITING;
            }
            else
            {
                _mainMenuController.SetConnectInteractable(true);
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

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendChatMessage();
        }

        UpdatePlayerPositions();

        if(_clickManager.MovePending())
        {
            ClickMove clickMove = (ClickMove)_clickManager.Handle();
            OutMove move = new OutMove((byte)clickMove.CityDest);
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
        _id = _serverInput.BeginGameHolder.PlayerId;
        _numPlayers = _serverInput.BeginGameHolder.NumPlayers;
        _clientState = ClientState.CSTATE_GAME;
        var playerInfoManager = _playerInfoManager.GetComponent<PlayerInfoManager>();

        for (int i = 0; i < _numPlayers; i++)
        {
            if (i == _id)
            {
                var player = Instantiate(_gameController.PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                player.GetComponent<Player>().SetId(i);
            }
            else
            {
                var otherPlayer = Instantiate(_gameController.OtherPlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                otherPlayer.GetComponent<OtherPlayer>().SetId(i);
            }

            playerInfoManager.InstantiatePlayerInfo();
            playerInfoManager.SetPlayerName(i, _serverInput.BeginGameHolder.PlayerNames[i]);
            playerInfoManager.SetPlayerRole(i, _serverInput.BeginGameHolder.PlayerRoles[i]);
        }

        _otherPlayers = GameObject.FindGameObjectsWithTag("OtherPlayer");
        _player = GameObject.FindGameObjectWithTag("Player");
        _tiles = GameObject.FindGameObjectsWithTag("Tile");
    }

    private void SendChatMessage()
    {
        if(_console != null)
        {
            if (_console.SendingText.text.Length > 0)
            {
                OutClientMessage msg = new OutClientMessage(ClientMessageType.CMESSAGE_CHAT, _console.GetSendingMessage());
                _opcodeManager.Send(msg);
                _console.ClearSendingMessage();
            }
        }
        else
        {
            Debug.Log("SendChatMessage(): console not initialized");
        }
    }

    private void UpdateLobby()
    {
        const float idleSendPeriod = 1.0f;
        _opcodeManager.ReceiveAll();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendChatMessage();
        }

        if (Time.time > nextIdleTime)
        {
            nextIdleTime += idleSendPeriod;
            OutIdle idle = new OutIdle();
            _opcodeManager.Send(idle);
        }

        if (_serverInput.BeginGameHolder.HasGameStarted())
        {
            if(_loaded == 0)
            {
                _loaded = 1;
                StartCoroutine(LoadMainScene());
            }
            else if(_loaded == 2)
            {
                _loaded = 3;
                InitializeGameObjects();
                BeginGame();
            }
        }
    }

    IEnumerator LoadMainScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Matemato");

        while (!asyncLoad.isDone)
        {   
            yield return null;
        }
        _loaded = 2;
    }

    private void UpdateAwaiting()
    {
        if (_awaitingSubstate == 0)
        {
            Debug.Log("waiting for 11");
            if (_msgManager.PendingInput())
            {
                if (_msgManager.ReadByte() == 11)
                {
                    Debug.Log("got 11 - setting substate to 1");
                    _awaitingSubstate = 1;
                }
            }
        }
        else if (_awaitingSubstate == 1)
        {
            var lobbyChoice = _mainMenuController.GetLobbyChoice();
            
            if (lobbyChoice != -1)
            {
                var name = _mainMenuController.GetName();
                _msgManager.WriteByte(12); //join lobby
                _msgManager.WriteByte((byte)lobbyChoice); //lobby id
                _msgManager.WriteByte((byte)name.Length);
                _msgManager.WriteString(name);
                _awaitingSubstate = 2;
                _mainMenuController.ClearLobbyChoice();
            }
        }
        else if (_awaitingSubstate == 2)
        {
            if (_msgManager.PendingInput())
            {
                var ret = _msgManager.ReadByte();

                switch(ret)
                {
                    case 0: //joined lobby
                    {
                        _clientState = ClientState.CSTATE_LOBBY;
                        _mainMenuController.Console.SetActive(true);
                        _mainMenuController.SetLobbyText("Lobby joined.");
                        _mainMenuController.ShowLobbyJoin(false);
                        break;
                    }
                    case 1: //game is full
                    {
                        _awaitingSubstate = 1;
                        _mainMenuController.SetLobbyText("Game is already full.");
                        break;
                    }
                    case 2: //game does not exist
                    {
                        _awaitingSubstate = 1;
                        _mainMenuController.SetLobbyText("Game does not exist.");
                        break;
                    }
                    default:
                    {
                        Debug.LogError("Invalid byte received from server during handshake");
                        break;
                    }
                }
            }
        }
    }
}
