using UnityEngine;


public class ActionButton : MonoBehaviour
{
    public PlayerAction action;

    public void OnClick()
    {
        Debug.Log("ActionButton OnClick called for action: " + action);
        ActionManager.Instance.Execute(action);
    }
}
