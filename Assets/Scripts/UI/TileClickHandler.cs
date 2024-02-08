using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class TileClickHandler : MonoBehaviour
{
    public GameController GameController;

    private void OnMouseDown()
    {

        bool neighbouringCity = gameObject.GetComponent<Tile>().Highlight;

        if (neighbouringCity) 
        {
            var tileId = gameObject.GetComponent<Tile>().GetId();
            GameController.OpcodeManager.Send(new OutMove((byte)tileId));
        }

        
    }
}
