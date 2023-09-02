using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    private GameObject _player = null;

    void Start()
    {
    }

    private void OnMouseDown()
    {

        bool neighbouringCity = gameObject.GetComponent<Tile>().Highlight;

        if (neighbouringCity) 
        {
            if(_player == null)
            {
                _player = GameObject.FindGameObjectWithTag("Player");
                if(_player == null)
                {
                    Debug.LogError("Player not instanced yet");
                    return;
                }
            }
            _player.GetComponent<Player>().Click = new Click(gameObject.GetComponent<Tile>().GetId());
        }

        
    }
}
