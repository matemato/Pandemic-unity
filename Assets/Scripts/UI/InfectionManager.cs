using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
    private GameObject _consoleObject;
    [SerializeField]
    private GameObject _animationControllerObject;
    [SerializeField]
    private GameObject _gameControllerObject;
    [SerializeField]
    private Texture[] _virusTextures; // black, blue, yellow, red

    private Console _console;
    private AnimationController _animationController;
    private GameController _gameController;
    private int discardCardOnTop = 0;
    private float nextInfection = 0f;
    private float infectionTime = 1f;

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
        _console = _consoleObject.GetComponent<Console>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameController.ServerInput != null)
        {
            if (Time.time > nextInfection)
            { 
                var request = _gameController.ServerInput.InfectionHolder.GetNext();

                if (request != null)
                {
                    var infectionCard = request.Item1;
                    var infectionInfo = request.Item2;
                    drawCard(infectionCard);

                    while (infectionInfo.Count > 0)
                    {
                        var infection = infectionInfo.Dequeue();
                        if (infection.Item1 == InfectionType.EXPLOSION)
                        {
                            // play animation
                            _console.AddText(ServerMessageType.SMESSAGE_INFO, "Explosion");
                        }
                        //< color = green > green </ color >
                        else
                        {
                            var virusColor = VirusTypeToColor(infection.Item1);
                            Infect(infection.Item2, infection.Item1, 1);
                            _console.AddText(ServerMessageType.SMESSAGE_INFO, "Infected <color=" + virusColor + ">" + EnumToString(infectionCard) + "</color> for city: " + infection.Item2);
                        }
                    }

					if (_gameController.ServerInput.InfectionHolder.IsQueueEmpty())
					{
						//we have processed all of the stuff, send ready packet to server
						_gameController.OpcodeManager.Send(new OutReady());
					}

					nextInfection = Time.time + infectionTime;
				}
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            //Debug.Log("yoyoyo");
            //drawCard(InfectionCard.ICARD_KOLKATA, "Kolkata");
            Infect(0, InfectionType.VIRUS_BLACK, 1);
        }
        /*else if (Input.GetKeyDown(KeyCode.L))
        {
            Infect(InfectionCard.ICARD_LAGOS, InfectionType.VIRUS_YELLOW, 2);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            Infect(InfectionCard.ICARD_JAKARTA, InfectionType.VIRUS_RED, 1);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            Infect(InfectionCard.ICARD_PARIS, InfectionType.VIRUS_BLUE, 1);
        }*/
    }

    public void Infect(int cityId, InfectionType infectionType, int infectCount)
    {
        
        //drawCard(infectionCard, infectionType);

        var tiles = GameObject.FindGameObjectsWithTag("Tile");
        GameObject tile = null;
        Tile tileScript = null;

        foreach(GameObject t in tiles)
        {
            if (t.GetComponent<Tile>().GetId() == cityId)
            {
                tile = t;
                tileScript = tile.GetComponent<Tile>();
                break;
            }
        }

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
    }

    public void drawCard(InfectionCard infectionCard)
    {
        string infectionCardName = EnumToString(infectionCard);

        var newInfectionCard = Instantiate(_infectionCardPrefab, new Vector3(-100, 20, discardCardOnTop), Quaternion.identity);
        newInfectionCard.transform.SetParent(gameObject.transform, false);
        var newCard = newInfectionCard.transform.GetChild(0).gameObject;
        newCard.GetComponent<InfectionCardScript>().SetInfectionCard(infectionCard);
        //newCard.GetComponent<InfectionCardScript>().SetInfectionType(infectionType);

        foreach (Sprite cardPic in _cardPics)
        {
            if (cardPic.name == infectionCardName)
            {
                newCard.GetComponent<SpriteRenderer>().sprite = cardPic;
                break;
            }
        }
        var targetPosition = new Vector3(165, 20, discardCardOnTop);
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

    public string VirusTypeToColor(InfectionType infectionType)
    {
        switch (infectionType)
        {
            case InfectionType.VIRUS_BLUE:
                return "blue";
            case InfectionType.VIRUS_RED:
                return "red";
            case InfectionType.VIRUS_BLACK:
                return "#7F7F7F";
            case InfectionType.VIRUS_YELLOW:
                return "yellow";
            default:
                return "";
        }
    }
}
