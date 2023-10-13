using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCardScript : MonoBehaviour
{
    private PlayerCard _playerCard;
    private CityColor _cityColor;

    public Vector3 TargetPosition;
    public Vector3 BeginPosition;

    void Start()
    {
    }

    public void MoveToTarget()
    {
        Debug.Log(TargetPosition);
        StartCoroutine(LerpPosition(TargetPosition, 0.5f));
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.parent.localPosition;
        while (time < duration)
        {
            transform.parent.localPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.parent.localPosition = targetPosition;
    }

    void Update()
    {
       
    }

    public void SetCityColor(CityColor cityColor)
    {
        _cityColor = cityColor;
    }

    public CityColor GetCityColor() 
    { 
        return _cityColor; 
    }
    public void SetPlayerCard(PlayerCard playerCard)
    { 
        _playerCard = playerCard;
    }

    public PlayerCard GetPlayerCard() 
    { 
        return _playerCard;
    }
}
