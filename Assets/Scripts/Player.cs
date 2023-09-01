using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private Vector2 position;
    // Start is called before the first frame update
    public Player(Vector2 initialPosition)
    {
        position = initialPosition;
    }

    // Property to get and set the position
    public Vector2 Position
    {
        get { return position; }
        set { position = value; }
    }
}
