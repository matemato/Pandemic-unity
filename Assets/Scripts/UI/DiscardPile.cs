using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardPile : MonoBehaviour
{
    // Start is called before the first frame update
  
    public void DiscardCard(Sprite cardPic)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = cardPic;
    }
}
