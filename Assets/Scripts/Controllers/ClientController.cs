using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientController : MonoBehaviour
{
    // Start is called before the first frame update
    private ServerInput _serverInput;
    private TCPClient _tcpClient;
    private MsgManager _msgManager;
    private OpcodeManager _opcodeManager;

    private ClientState _clientState = ClientState.CSTATE_UNCONNECTED;
    private int _id;
    private int _numPlayers;

    [SerializeField]
    private MainMenuController _mainMenuController;
    private GameController _gameController;

    private GameObject _player;

    private GameObject _playerInfoManager;

    private float nextIdleTime = 0.0f;

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
        _mainMenuController.OpcodeManager = _opcodeManager;
    }

    void InitializeGameObjects() 
    {
        _playerInfoManager = GameObject.Find("PlayerInfoManager");
        _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        _gameController.ServerInput = _serverInput;
        _gameController.OpcodeManager = _opcodeManager;
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
    void LateUpdate()
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
            if (_tcpClient.ConnectToTcpServer(_mainMenuController.GetIp()))
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

    private void UpdateIngame()
    {
        const float idleSendPeriod = 1.0f;
		var playerInfoManager = _playerInfoManager.GetComponent<PlayerInfoManager>();

		_opcodeManager.ReceiveAll();

        if (Time.time > nextIdleTime)
        {
            nextIdleTime += idleSendPeriod;
            OutIdle idle = new OutIdle();
            _opcodeManager.Send(idle);
        }

		var newActivePlayer = _serverInput.TurnInfoHolder.GetActive();
		if (newActivePlayer.Item1 > -1)
		{
			playerInfoManager.SetTurnBegin(newActivePlayer.Item1, newActivePlayer.Item2);
		}

		var newEndTurn = _serverInput.TurnInfoHolder.GetEndTurnChange();
		if (newEndTurn > -1)
		{
			playerInfoManager.SetTurnEnd(newEndTurn);
		}

		var changeActions = _serverInput.TurnInfoHolder.GetActionChange();
		if (changeActions.Item1 > -1)
		{
			playerInfoManager.UpdateActions(changeActions.Item1, changeActions.Item2);
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
            playerInfoManager.InstantiatePlayerInfo();
            playerInfoManager.SetPlayerName(i, _serverInput.BeginGameHolder.PlayerNames[i]);
            playerInfoManager.SetPlayerRole(i, _serverInput.BeginGameHolder.PlayerRoles[i]);
			
            if (i == _id)
            {
                var player = Instantiate(_gameController.PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                player.GetComponent<Player>().SetId(i);
				player.GetComponent<Player>().SetColor(i);
			}
            else
            {
                var otherPlayer = Instantiate(_gameController.OtherPlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                otherPlayer.GetComponent<OtherPlayer>().SetId(i);
				otherPlayer.GetComponent<OtherPlayer>().SetColor(i);
			}
			
        }

        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void UpdateLobby()
    {
        const float idleSendPeriod = 1.0f;
        _opcodeManager.ReceiveAll();

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
		const float idleSendPeriod = 1.0f;
		_opcodeManager.ReceiveAll();

		if (Time.time > nextIdleTime)
		{
			nextIdleTime += idleSendPeriod;
			OutIdle idle = new OutIdle();
			_opcodeManager.Send(idle);
		}

		var lobbyChoice = _mainMenuController.GetLobbyChoice();
		if (lobbyChoice != -1)
		{
			OutJoinLobby joinLobby = new OutJoinLobby(lobbyChoice, _mainMenuController.GetName());
			_opcodeManager.Send(joinLobby);
			_mainMenuController.ClearLobbyChoice();
		}

		var rsp = _serverInput.JoinLobbyHolder.GetResponse();
		if (rsp != JoinLobbyResponse.LOBBY_PENDING)
		{
			switch (rsp)
			{
				case JoinLobbyResponse.LOBBY_OK:
				{
					_clientState = ClientState.CSTATE_LOBBY;
					_mainMenuController.Console.SetActive(true);
					_mainMenuController.SetLobbyText("Lobby joined.");
					_mainMenuController.ShowLobbyJoin(false);
					break;
				}
				case JoinLobbyResponse.LOBBY_FULL:
				{
					_mainMenuController.SetLobbyText("Game is already full.");
					break;
				}
				case JoinLobbyResponse.LOBBY_NOEXIST:
				{
					_mainMenuController.SetLobbyText("Game does not exist.");
					break;
				}
			}

		}
	}
}
