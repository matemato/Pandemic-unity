using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActionManager : MonoBehaviour
{
    public static ActionManager Instance;
    private Player _player = null;
    private GameObject[] _selectedCubes;
    private bool _wasMyTurn = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _selectedCubes = GameObject.FindGameObjectsWithTag("HoverCube");
    }

    private void Update()
    {
        if (Player.Instance == null)
        {
            Debug.LogWarning("Player instance is null. Cannot check turn status.");
            return;
        }
        else if (_player == null)
        {
            _player = Player.Instance;
        }

        if (_player &&_player.IsMyTurn && !_wasMyTurn)
        {
            Debug.Log("It's now the player's turn. Showing disease picker if necessary.");
            _wasMyTurn = true;
            RefreshDiseasePicker();
        }
        else if (_player && !_player.IsMyTurn && _wasMyTurn)
        {
            _wasMyTurn = false;
            HideDiseasePicker();
        }
    }

    public void Execute(PlayerAction action)
    {
        var currentCity = _player.CurrentCity;
        Debug.Log("ActionManager Execute called with action: " + action);
        switch (action)
        {

            case PlayerAction.TreatDisease:
                var cityVirusCubes = currentCity.GetVirusCubes();
                var diseaseTypes = GetDiseaseTypes(cityVirusCubes);
                int diseaseTypesCount = diseaseTypes.Count;
                if (diseaseTypesCount > 1)
                {
                    foreach(var cube in _selectedCubes)
                    {
                        // if cube _hoverActive is true, then send opcode to treat disease of that cube's infection type
                        var hoverManager = cube.GetComponent<HoverManager>();
                        if (hoverManager._hoverActive)
                        {
                            Debug.Log("Treating disease of type: " + hoverManager.InfectionType);
                            GameController.Instance.OpcodeManager.Send(new OutTreatDisease(hoverManager.InfectionType));
                            break; // Only treat one disease at a time
                        }

                    }

                }
                else if (diseaseTypesCount == 1)
                {
                    GameController.Instance.OpcodeManager.Send(new OutTreatDisease(cityVirusCubes[0].GetComponent<VirusCubeManager>().GetInfectionType()));
                }

                break;

            case PlayerAction.DiscoverCure:
                // PlayerActions.DiscoverCure();
                break;
        }
    }

    //temp function until server lets us know if the action was successful or not in the update loop

    public HashSet<InfectionType> GetDiseaseTypes(List<GameObject> cityVirusCubes)
    {
        HashSet<InfectionType> types = new HashSet<InfectionType>();

        foreach (GameObject virusCube in cityVirusCubes)
        {
            if (virusCube.TryGetComponent<VirusCubeManager>(out var manager))
            {
                types.Add(manager.GetInfectionType());
            }
        }

        return types;
    }

    public void RefreshDiseasePicker()
    {
        var cityVirusCubes = _player.CurrentCity.GetVirusCubes();
        var diseaseTypes = GetDiseaseTypes(cityVirusCubes);
        foreach (var cube in _selectedCubes)
        {
            var hoverManager = cube.GetComponent<HoverManager>();
            if (diseaseTypes.Contains(hoverManager.InfectionType))
            {
                cube.SetActive(true);
                cube.GetComponent<SpriteRenderer>().enabled = true;
                cube.GetComponent<BoxCollider>().enabled = true;
            }
            else
            {
                hoverManager._hoverActive = false;
                hoverManager.Hover.SetActive(false);
                cube.SetActive(false);
            }
        }
    }

    public void HideDiseasePicker()
    {
        foreach (var cube in _selectedCubes)
        {
            cube.SetActive(false);
        }
    }
}
