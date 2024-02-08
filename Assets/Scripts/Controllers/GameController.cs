using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    public GameObject PlayerPrefab;
    [SerializeField]
    public GameObject OtherPlayerPrefab;

    public ServerInput ServerInput;
    public OpcodeManager OpcodeManager;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
