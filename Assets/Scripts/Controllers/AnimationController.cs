using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    // Map GameObject instance ID -> running Coroutine
    private readonly Dictionary<int, Coroutine> _activeCoroutines = new Dictionary<int, Coroutine>();

    public void MoveToTarget(GameObject gObject, Vector3? startPosition, Vector3 targetPosition, float duration)
    {
        if (gObject == null) return;

        int id = gObject.GetInstanceID();

        // If there's an existing coroutine for this GameObject, stop it
        if (_activeCoroutines.TryGetValue(id, out Coroutine existing) && existing != null)
        {
            StopCoroutine(existing);
            _activeCoroutines.Remove(id);
        }

        // Start a new coroutine and keep reference
        Coroutine c = StartCoroutine(LerpPosition(gObject, startPosition, targetPosition, duration));
        _activeCoroutines[id] = c;
    }

    private IEnumerator LerpPosition(GameObject gObject, Vector3? startPosition, Vector3 targetPosition, float duration)
    {
        if (gObject == null)
            yield break;

        int id = gObject.GetInstanceID();

        float time = 0f;
        Vector3 actualStartPosition = startPosition ?? gObject.transform.localPosition;

        // If duration is zero or negative, snap immediately
        if (duration <= 0f)
        {
            gObject.transform.localPosition = targetPosition;
            _activeCoroutines.Remove(id);
            yield break;
        }

        while (time < duration)
        {
            // Safeguard: stop if the object was destroyed mid-animation
            if (gObject == null)
                yield break;

            gObject.transform.localPosition = Vector3.Lerp(actualStartPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        // Final snap to target and cleanup dictionary entry
        if (gObject != null)
            gObject.transform.localPosition = targetPosition;

        // Remove dictionary entry if present (may have been replaced already)
        _activeCoroutines.Remove(id);
    }
}
