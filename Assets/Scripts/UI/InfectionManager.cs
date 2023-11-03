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
    private Sprite[] _cardPics;
    [SerializeField]
    private GameObject _animationControllerObject;


    private AnimationController _animationController;
    private int discardCardOnTop = 0;
    // Start is called before the first frame update
    void Start()
    {
        _animationController = _animationControllerObject.GetComponent<AnimationController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("yoyoyo");
            //drawCard(InfectionCard.ICARD_KOLKATA, "Kolkata");
            Infect(InfectionCard.ICARD_KOLKATA, InfectionType.VIRUS_BLACK, PlayerCard.CCARD_KOLKATA, 3);
        }
    }

    public void Infect(InfectionCard infectionCard, InfectionType infectionType, PlayerCard playerCard, int infectCount)
    {
        string infectionCardName = EnumToString(infectionCard);
        drawCard(infectionCard, infectionType, infectionCardName);

        var tile = GameObject.Find(infectionCardName);
        Tile tileScript = tile.GetComponent<Tile>();

        tileScript.SetInfectionCount(infectionType, infectCount);

        Debug.Log(tileScript.GetInfectionCount(infectionType));
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
