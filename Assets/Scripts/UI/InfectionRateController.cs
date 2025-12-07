using UnityEngine;

public class InfectionRateController : MonoBehaviour
{
    private int[] _infectionRates = { 2, 2, 2, 3, 3, 4, 4 };
    private int _currentRateIndex = 0;

    public int GetCurrentInfectionRate()
    {
        return _infectionRates[_currentRateIndex];
    }

    public void IncreaseInfectionRate()
    {
        if (_currentRateIndex < _infectionRates.Length - 1)
        {
            _currentRateIndex++;
        }
    }

    public void MoveInfectionRateMarker()
    {
        transform.localPosition += new Vector3(60, 0, 0);
    }

    public void ResolveEpidemicInfectionRate()
    { 
        IncreaseInfectionRate();
        MoveInfectionRateMarker();
    }
}
