using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public static ActionManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Execute(PlayerAction action)
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var currentCity = player.GetComponent<Player>().CurrentCity;
        Debug.Log("ActionManager Execute called with action: " + action);
        switch (action)
        {

            case PlayerAction.TreatDisease:
                var cityVirusCubes = currentCity.GetVirusCubes();
                int diseaseTypesCount = CheckDiseaseTypes(cityVirusCubes);
                if (diseaseTypesCount > 1)
                {
                    Debug.Log("Multiple disease types present. Prompting player to choose which disease to treat.");
                    // Prompt player to choose which disease to treat
                    // This could be done via a UI dialog or similar mechanism
                    // For now, we'll just log and return
                    return;
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

    public int CheckDiseaseTypes(List<GameObject> cityVirusCubes)
    {
        HashSet<InfectionType> types = new HashSet<InfectionType>();

        if (cityVirusCubes.Count == 0)
        {
            return 0;
        }

        foreach (GameObject virusCube in cityVirusCubes)
        {
            if (virusCube.TryGetComponent<VirusCubeManager>(out var manager))
            {
                types.Add(manager.GetInfectionType());
            }
        }

        return types.Count;
    }

}
