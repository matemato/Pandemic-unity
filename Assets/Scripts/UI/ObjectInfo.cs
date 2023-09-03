using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInfo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Vector3 itemSize = spriteRenderer.bounds.size;

        float pixelsPerUnit = spriteRenderer.sprite.pixelsPerUnit;


        itemSize.y *= pixelsPerUnit;
        itemSize.x *= pixelsPerUnit;

        Debug.Log("y:, " + itemSize.y);
        Debug.Log("x: " + itemSize.x);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
