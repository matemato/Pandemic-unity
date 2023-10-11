using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerInfoPrefab;

    public List<GameObject> _playerInfos = new List<GameObject>();

    private int _numOfPlayers = 0;

    Dictionary<PlayerRole, string> RoleDict = new Dictionary<PlayerRole, string>();

    // Start is called before the first frame update
    void Start()
    {
        RoleDict[PlayerRole.ROLE_MEDIC] = "Medic";
        RoleDict[PlayerRole.ROLE_EXPERT] = "Operations expert";
        RoleDict[PlayerRole.ROLE_RESEARCHER] = "Researcher";
        RoleDict[PlayerRole.ROLE_DISPATCHER] = "Dispatcher";
        RoleDict[PlayerRole.ROLE_SCIENTIST] = "Scientist";
        RoleDict[PlayerRole.ROLE_SPECIALIST] = "Quarantine specialist";
        RoleDict[PlayerRole.ROLE_PLANNER] = "Contingency planner";

        InstantiatePlayerInfo();
        SetPlayerName(0, "5Keni5");
        SetPlayerRole(0, PlayerRole.ROLE_PLANNER);
    }

    // Update is called once per frame
    public void InstantiatePlayerInfo()
    {
        var playerInfo = Instantiate(_playerInfoPrefab, new Vector3(-795 + (_numOfPlayers * 330), 400, 0), Quaternion.identity);
        playerInfo.transform.SetParent(gameObject.transform, false);
        _playerInfos.Add(playerInfo);
        _numOfPlayers++;
    }

    public void SetPlayerName(int id, string name)
    {
        _playerInfos[id].GetComponent<PlayerInfo>().SetPlayerName(name);
    }
    public void SetPlayerRole(int id, PlayerRole playerRole)
    {
        _playerInfos[id].GetComponent<PlayerInfo>().SetPlayerRole(RoleDict[playerRole]);
    }

}
