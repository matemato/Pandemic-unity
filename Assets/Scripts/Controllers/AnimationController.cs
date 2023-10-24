using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public void MoveToTarget(GameObject gObject, Vector3? startPosition, Vector3 targetPosition, float duration)
    {
        StartCoroutine(LerpPosition(gObject, startPosition, targetPosition, duration));
    }

    IEnumerator LerpPosition(GameObject gObject, Vector3? startPosition, Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 actualStartPosition = startPosition ?? gObject.transform.localPosition; // checks if startPosition is null, if it is then default value is localPosition
        while (time < duration)
        {
            gObject.transform.localPosition = Vector3.Lerp(actualStartPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        gObject.transform.localPosition = targetPosition;
    }
}
