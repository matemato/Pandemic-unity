using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusCubeManager : MonoBehaviour
{
    public Tile tile;
    private float radius = 0.6f; // The radius of the circle
    private float angularSpeed = 1.5f; // The speed of rotation

    private float angle = 0f;

    // Update is called once per frame
    void Update()
    {
        RotateAroundTile();
        transform.Rotate(Vector3.up, 50f * Time.deltaTime);
        transform.Rotate(Vector3.left, 50f * Time.deltaTime);
    }

    public void RotateAroundTile() 
    {
        float x = tile.transform.localPosition.x + radius * Mathf.Cos(angle);
        float y = tile.transform.localPosition.y + radius * Mathf.Sin(angle);

        transform.localPosition = new Vector3(x, y, transform.localPosition.z);

        angle += angularSpeed * Time.deltaTime;

        angle = Mathf.Repeat(angle, 360f);
    }
}
