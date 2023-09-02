using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    private GameObject _player; // Reference to the object you want to change.

    void Start()
    {
        _player = GameObject.Find("Player");
    }

    private void OnMouseDown()
    {
        bool neighbouringCity = gameObject.GetComponent<Tile>().Highlight;

        if (neighbouringCity) {
            _player.GetComponent<Player>().Click = new Click(gameObject.GetComponent<Tile>().GetId());
        }

        
    }
}
