using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverManager : MonoBehaviour
{
    public GameObject hover;
    void Start()
    {
        hover.SetActive(false);
    }

    private void OnMouseEnter()
    {
        hover.SetActive(true);
    }
    private void OnMouseExit()
    {
        hover.SetActive(false);
    }
}
