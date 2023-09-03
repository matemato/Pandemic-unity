using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class TileClickHandler : MonoBehaviour
{
    private ClickManager _clickManager;

    void Start()
    {
        _clickManager = GameObject.Find("ClickController").GetComponent<ClickManager>();
        if (_clickManager == null)
        {
            Debug.LogError("ClickManager is null");
        }
    }

    private void OnMouseDown()
    {

        bool neighbouringCity = gameObject.GetComponent<Tile>().Highlight;

        if (neighbouringCity) 
        {
            var tileId = gameObject.GetComponent<Tile>().GetId();
            _clickManager.Create(new ClickMove(tileId));
        }

        
    }
}
