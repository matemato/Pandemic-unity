using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    private GameObject Player; // Reference to the object you want to change.

    void Start()
    {
        Player = GameObject.Find("Player");
    }

    private void OnMouseDown()
    {
        bool neighbouringCity = gameObject.GetComponent<Tile>().Highlight;

        if (neighbouringCity) {
            Player.GetComponent<Player>().City = gameObject.GetComponent<Tile>();
        }

        
    }
}
