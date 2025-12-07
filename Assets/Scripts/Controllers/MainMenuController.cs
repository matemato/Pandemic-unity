using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    // main menu
    
    [SerializeField]
    private GameObject _joinLobbyButton;
    [SerializeField]
    private GameObject _usernameInput;
    [SerializeField]
    private GameObject _dropdownLobby;
    [SerializeField]
    private TMP_Text _lobbyText;
    [SerializeField]
    public GameObject Console;
	[SerializeField]
	private GameObject _ipSelectorPanel;

	public ServerInput ServerInput;
    public OpcodeManager OpcodeManager;

	private List<CancellationTokenSource> _lastCts = new();
	private string _lastSelectedIp = "localhost";
	private GameObject _refreshIpsButton;
	private GameObject _connectButton;

	public bool IsConnectButtonClicked()
    {
        var connectButton = _connectButton.GetComponent<ConnectButton>();
        bool isClicked = connectButton.IsConnectButtonClicked;
		if (isClicked)
			Debug.Log("Connect button clicked");
		connectButton.IsConnectButtonClicked = false;
        return isClicked;
    }
	public bool IsRefreshButtonClicked()
	{
		var refreshButton = _refreshIpsButton.GetComponent<RefreshButton>();
		bool isClicked = refreshButton.IsRefreshButtonClicked;
		refreshButton.IsRefreshButtonClicked = false;
		return isClicked;
	}

	public string GetIp()
    {
        return _lastSelectedIp;
    }

    // Start is called before the first frame update
    void Start()
    {
		_refreshIpsButton = _ipSelectorPanel.transform.Find("RefreshButton").gameObject;
		_connectButton = _ipSelectorPanel.transform.Find("ConnectButton").gameObject;
		SetLobbyText("");
		GetServerList();
	}

	public static List<IPEndPoint> ReadIpListWithFixedPort(string filePath, bool skipInvalid = true)
	{
		const int FixedPort = 43595;
		var endpoints = new List<IPEndPoint>();

		if (string.IsNullOrWhiteSpace(filePath))
			return endpoints;

		if (!File.Exists(filePath))
			return endpoints;

		var server_local = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 43594 + 1);
		endpoints.Add(server_local);
		var lines = File.ReadAllLines(filePath);
		int lineNumber = 0;

		foreach (var rawLine in lines)
		{
			lineNumber++;
			var ipText = rawLine.Trim();

			// Skip empty lines and common comment markers
			if (string.IsNullOrWhiteSpace(ipText) ||
				ipText.StartsWith("#") ||
				ipText.StartsWith("//") ||
				ipText.StartsWith(";"))
				continue;

			Debug.Log($"parsing {ipText}");

			if (IPAddress.TryParse(ipText, out var ipAddress))
			{
				endpoints.Add(new IPEndPoint(ipAddress, FixedPort));
			}
			else if (!skipInvalid)
			{
				throw new FormatException($"Invalid IP address on line {lineNumber}: '{rawLine}'");
			}
			// else: skipInvalid == true → silently ignore bad IPs
		}

		return endpoints;
	}

	public async void GetServerList()
	{
		using var client = new UdpClient();
		Debug.Log("Calling QuickPing");
		var ipPanel = _ipSelectorPanel.GetComponent<PopulateScrollView>();
		var loc = Application.dataPath + "/ip_list.txt";
		var servers = ReadIpListWithFixedPort(loc);
		var serversString = new List<string>();
		ipPanel.ClearList();

		foreach (var cts in _lastCts)
			cts.Cancel();

		foreach (var server in servers)
		{
			Debug.Log($"Sending PING to {server.Address.ToString()}");
			await client.SendAsync(Encoding.UTF8.GetBytes("PING"), 4, server);

			try
			{
				var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
				_lastCts.Add(cts);
				var udpReceive = client.ReceiveAsync();
				var result = await Task.WhenAny(udpReceive, Task.Delay(Timeout.InfiniteTimeSpan, cts.Token));
				if(result == udpReceive)
				{
					string reply = Encoding.UTF8.GetString(udpReceive.Result.Buffer);
					Debug.Log(reply == "PONG" ? $"Server {server.Address.ToString()} is ONLINE!" : "Wrong reply");
					serversString.Add(server.Address.ToString());
					ipPanel.AddToList(server.Address.ToString());
				}
				else
				{
					Debug.Log($"Server {server.Address.ToString()} is offline or not responding");
				}
			}
			catch
			{
				Debug.Log($"Server {server.Address.ToString()} is offline or not responding");
			}
		}
	}

	// Update is called once per frame
	void Update()
    {
        if (ServerInput != null)
        {
            var serverText = ServerInput.MessageHolder.GetNextLobbyText();
            if (serverText != null)
            {
                SetLobbyText(serverText);
            }
        }

		if(IsRefreshButtonClicked())
		{
			GetServerList();
		}

		var ipButtons = GameObject.FindGameObjectsWithTag("IpText");
		foreach(var ipButton in ipButtons)
		{
			var script = ipButton.GetComponent<IpTxtButton>();
			if (_lastSelectedIp == ipButton.GetComponent<TMP_Text>().text)
				continue;

			if (script.IsIpClicked)
			{
				script.SetClickedColor();
				_lastSelectedIp = ipButton.GetComponent<TMP_Text>().text;
				script.IsIpClicked = false;
			}
			else
			{
				script.SetNormalColor();
			}
		}
    }

    public string GetName()
    {
        return _joinLobbyButton.GetComponent<JoinLobbyClicked>().GetName();
    }

    public void SetLobbyText(string str)
    {
        _lobbyText.text = str;
    }

    public void ClearLobbyChoice()
    {
        _joinLobbyButton.GetComponent<JoinLobbyClicked>().ClearLobbyChoice();
    }

    public int GetLobbyChoice()
    {
        int choice = _joinLobbyButton.GetComponent<JoinLobbyClicked>().GetLobbyChoice();
		return choice;
    }

    public void ShowLobbyJoin(bool v)
    {
        _joinLobbyButton.SetActive(v);
        _usernameInput.SetActive(v);
        _dropdownLobby.SetActive(v);
    }

    public void ShowConnect(bool v)
    {
        _connectButton.SetActive(v);
		_ipSelectorPanel.SetActive(v);
    }

    public void SetConnectInteractable(bool v)
    {
        _connectButton.GetComponent<Button>().interactable = v;
    }
}
