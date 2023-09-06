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
    private GameObject _playerPrefab;
    [SerializeField]
    private GameObject _otherPlayerPrefab;

    private GameObject _player;
    private GameObject[] _otherPlayers;
    private GameObject[] _tiles;

    [SerializeField]
    private GameObject _connectButton;
    [SerializeField]
    private GameObject _joinLobbyButton;
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

        _connectButton.GetComponent<Button>().onClick.AddListener(ConnectButtonClicked);
    }

    void InitilizeGameObjects() 
    {
        _clickManager = GameObject.Find("ClickController").GetComponent<ClickManager>();
        _console = GameObject.Find("Console").GetComponent<Console>();
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

    private void ConnectButtonClicked()
    {
        if (_clientState == ClientState.CSTATE_UNCONNECTED)
        {
            Debug.Log("abc");
            if (_tcpClient.ConnectToTcpServer(_ip))
            {
                _connectButton.GetComponent<Button>().interactable = false;
                _connectButton.GetComponentInChildren<TMP_Text>().text = "Connected";
                _joinLobbyButton.SetActive(true);
                _clientState = ClientState.CSTATE_AWAITING;
            }
            else
            {
                Debug.LogError("Failed to connect to server");
            }
        }
    }

    private void UpdateUnconnected()
    {

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

        var serverText = _serverInput.MessageHolder.GetNext();
        if (serverText != null)
        {
            Debug.Log(serverText.Item2);
            _console.AddText(serverText.Item1, serverText.Item2, "yellow");
        }

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

    private void SendChatMessage()
    {
        if(_console.SendingText.text.Length > 0)
        {
             OutClientMessage msg = new OutClientMessage(ClientMessageType.CMESSAGE_CHAT, _console.GetSendingMessage());
            _opcodeManager.Send(msg);
            _console.ClearSendingMessage();
        }
    }

    private void UpdateLobby()
    {
        const float idleSendPeriod = 1.0f;
        _opcodeManager.ReceiveAll();

        var serverText = _serverInput.MessageHolder.GetNext();
        if (serverText != null)
        {
            Debug.Log(serverText.Item2);
            _console.AddText(serverText.Item1, serverText.Item2, "yellow");
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendChatMessage();
        }

        if (Input.GetKeyDown(KeyCode.F5)) //exit lobby, need to add opcode for disconnect
        {
            _msgManager.WriteByte(255);
            //_clientState = ClientState.CSTATE_UNCONNECTED;
            //_awaitingSubstate = 0;
            return;
        }

        if (Time.time > nextIdleTime)
        {
            nextIdleTime += idleSendPeriod;
            OutIdle idle = new OutIdle();
            _opcodeManager.Send(idle);
        }

        if (_serverInput.BeginGameHolder.HasGameStarted())
        {
            BeginGame();
        }
    }

    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Matemato");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {   
            yield return null;
        }

        _loaded = 2;

        Debug.Log("Scene changed.");
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
            if (Input.GetKeyDown(KeyCode.F1))
            {
                _msgManager.WriteByte(12); //join lobby
                _msgManager.WriteByte(0); //lobby id
                _awaitingSubstate = 2;
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                _msgManager.WriteByte(12);
                _msgManager.WriteByte(1); //lobby id
                _awaitingSubstate = 2;
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                _msgManager.WriteByte(12);
                _msgManager.WriteByte(2); //lobby id
                _awaitingSubstate = 2;
            }
            else if (Input.GetKeyDown(KeyCode.F4))
            {
                _msgManager.WriteByte(12);
                _msgManager.WriteByte(3); //lobby id
                _awaitingSubstate = 2;
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
                        if (_loaded == 0)
                        {
                            _awaitingSubstate = 3;
                            _loaded = 1;
                            StartCoroutine(LoadYourAsyncScene());
                        }
                        
                        break;
                    }
                    case 1: //game is full
                    {
                        _awaitingSubstate = 1;
                        break;
                    }
                    case 2: //game does not exist
                    {
                        _awaitingSubstate = 1;
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
        else if (_awaitingSubstate == 3)
        {
            if (_loaded == 2)
            {
                _loaded = 3;
                _clientState = ClientState.CSTATE_LOBBY;
                InitilizeGameObjects();
                Debug.Log("Ne izvede");
            }
        }
    }
}
