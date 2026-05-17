using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverManager : MonoBehaviour
{
    public GameObject Hover;
    public bool _hoverActive = false;
    private GameObject[] _selectedCubes;
    public InfectionType InfectionType;
    void Start()
    {
        _selectedCubes = GameObject.FindGameObjectsWithTag("HoverCube");
        Hover.SetActive(false);
    }

    private void OnMouseEnter()
    {
        Hover.SetActive(true);
    }
    private void OnMouseExit()
    {
        if (!_hoverActive) Hover.SetActive(false);
    }
    private void OnMouseDown()
    {
        if (this.CompareTag("HoverCube")) {
            _hoverActive = !_hoverActive;
            Hover.SetActive(_hoverActive);

            // deselect previous selected cube
            foreach (var cube in _selectedCubes)
            {
                if (cube != gameObject)
                {
                    cube.GetComponent<HoverManager>()._hoverActive = false;
                    cube.GetComponent<HoverManager>().Hover.SetActive(false);
                }
            }
        }



    }
}
