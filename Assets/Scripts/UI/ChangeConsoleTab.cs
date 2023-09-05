using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class ChangeConsoleTab : MonoBehaviour
{
    public TMP_Dropdown Dropdown;
    public TMP_Text AllText;
    public TMP_Text ChatText;
    public TMP_Text ConsoleText;


    private TMP_Text _visibleText;

    //private Dropdown _dropdown;


    // Start is called before the first frame update
    void Start()
    {
        Dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(Dropdown);
        });
    }

    void DropdownValueChanged(TMP_Dropdown change)
    {

        AllText.gameObject.SetActive(false);
        ChatText.gameObject.SetActive(false);
        ConsoleText.gameObject.SetActive(false);

        switch (change.value)
        {
            case 0:
                _visibleText = AllText;
                break;

            case 1:
                _visibleText = ChatText;
                break;

            case 2:
                _visibleText = ConsoleText;
                break;

            default:
                _visibleText = AllText;
                break;
        }
        _visibleText.gameObject.SetActive(true);
        gameObject.GetComponent<ScrollRect>().content = _visibleText.gameObject.GetComponent<RectTransform>();
    }
}
