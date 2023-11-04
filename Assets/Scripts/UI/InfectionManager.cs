using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class InfectionManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _infectionCardPrefab;
    [SerializeField]
    private GameObject _virusCubePrefab;
    [SerializeField]
    private Sprite[] _cardPics;
    [SerializeField]
    private GameObject _animationControllerObject;
    [SerializeField]
    private GameObject _gameControllerObject;
    [SerializeField]
    private Texture[] _virusTextures; // black, blue, yellow, red

    private AnimationController _animationController;
    private GameController _gameController;
    private int discardCardOnTop = 0;

    private Dictionary<InfectionType, Texture> _virusTexturesDict = new Dictionary<InfectionType, Texture>();

    // Start is called before the first frame update
    void Start()
    {
        _virusTexturesDict[InfectionType.VIRUS_BLACK] = _virusTextures[0];
        _virusTexturesDict[InfectionType.VIRUS_BLUE] = _virusTextures[1];
        _virusTexturesDict[InfectionType.VIRUS_YELLOW] = _virusTextures[2];
        _virusTexturesDict[InfectionType.VIRUS_RED] = _virusTextures[3];

        _animationController = _animationControllerObject.GetComponent<AnimationController>();
        _gameController = _gameControllerObject.GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (_gameController.ServerInput != null)
        {
            var request = _gameController.ServerInput.

            if (request != null)
            {

            }
        }*/

        if (Input.GetKeyDown(KeyCode.K))
        {
            //Debug.Log("yoyoyo");
            //drawCard(InfectionCard.ICARD_KOLKATA, "Kolkata");
            Infect(InfectionCard.ICARD_KOLKATA, InfectionType.VIRUS_BLACK, PlayerCard.CCARD_KOLKATA, 3);
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            Infect(InfectionCard.ICARD_LAGOS, InfectionType.VIRUS_YELLOW, PlayerCard.CCARD_LAGOS, 2);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            Infect(InfectionCard.ICARD_JAKARTA, InfectionType.VIRUS_RED, PlayerCard.CCARD_JAKARTA, 1);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            Infect(InfectionCard.ICARD_PARIS, InfectionType.VIRUS_BLUE, PlayerCard.CCARD_PARIS, 1);
        }
    }

    public void Infect(InfectionCard infectionCard, InfectionType infectionType, PlayerCard playerCard, int infectCount)
    {
        string infectionCardName = EnumToString(infectionCard);
        drawCard(infectionCard, infectionType, infectionCardName);

        var tile = GameObject.Find(infectionCardName);
        Tile tileScript = tile.GetComponent<Tile>();

        tileScript.SetInfectionCount(infectionType, infectCount);

        for (int i = 0; i < infectCount; i++)
        {
            var virusCube = Instantiate(_virusCubePrefab, gameObject.transform.position, Quaternion.identity);
            virusCube.transform.SetParent(tile.transform.parent, false);

            virusCube.GetComponentInChildren<MeshRenderer>().material.mainTexture = _virusTexturesDict[infectionType];
            tileScript.AddVirusCube(virusCube);
        }

        var virusCubes = tileScript.GetVirusCubes();

        for (int i = 0; i < virusCubes.Count; i++) 
        {
            virusCubes[i].GetComponent<VirusCubeManager>().SetStartingAngle(i * 2 * Mathf.PI / virusCubes.Count);
            virusCubes[i].GetComponent<VirusCubeManager>().SetTile(tileScript);
        }


        //Debug.Log(tileScript.GetInfectionCount(infectionType));
    }

    public void drawCard(InfectionCard infectionCard, InfectionType infectionType, string infectionCardName)
    {
        var newInfectionCard = Instantiate(_infectionCardPrefab, new Vector3(-100, 15, discardCardOnTop), Quaternion.identity);
        newInfectionCard.transform.SetParent(gameObject.transform, false);
        var newCard = newInfectionCard.transform.GetChild(0).gameObject;
        newCard.GetComponent<InfectionCardScript>().SetInfectionCard(infectionCard);
        newCard.GetComponent<InfectionCardScript>().SetInfectionType(infectionType);

        foreach (Sprite cardPic in _cardPics)
        {
            if (cardPic.name == infectionCardName)
            {
                newCard.GetComponent<SpriteRenderer>().sprite = cardPic;
                break;
            }
        }
        var targetPosition = new Vector3(165, 15, discardCardOnTop);
        _animationController.MoveToTarget(newInfectionCard, null, targetPosition, 0.5f);
        discardCardOnTop--;
    }

    public string EnumToString(InfectionCard infectionCard)
    {
        if (infectionCard == InfectionCard.ICARD_ST_PETERSBURG) return "St. Petersburg";

        string cardName = infectionCard.ToString().Substring(6);
        cardName = cardName.Replace("_", " ").ToLower();

        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(cardName.ToLower());
    }
}
