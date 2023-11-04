using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusCubeManager : MonoBehaviour
{
    private Tile _tile = null;
    private float _radius = 0.6f; // The radius of the circle
    private float _angularSpeed = 1.5f; // The speed of rotation

    private float _angle = 0f;

    // Update is called once per frame
    void Update()
    {
        if (_tile != null)
        {
            RotateAroundTile();
            transform.Rotate(Vector3.up, 50f * Time.deltaTime);
            transform.Rotate(Vector3.left, 50f * Time.deltaTime);
        }
    }

    public void RotateAroundTile() 
    {
        float x = _tile.transform.localPosition.x + _radius * Mathf.Cos(_angle);
        float y = _tile.transform.localPosition.y + _radius * Mathf.Sin(_angle);

        transform.localPosition = new Vector3(x, y, -1);

        _angle += _angularSpeed * Time.deltaTime;

        _angle = Mathf.Repeat(_angle, 2*Mathf.PI);
    }

    public void SetStartingAngle(float startAngle)
    {
        _angle = startAngle;
    }

    public void SetTile(Tile tile)
    {
        _tile = tile;
    }

    public Tile GetTile() { return _tile; }

}
