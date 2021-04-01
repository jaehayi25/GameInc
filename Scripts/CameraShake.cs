using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator DelayShake(float delay, float duration, float magnitude)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(Shake(duration, magnitude));
    }

    public IEnumerator Shake (float duration, float magnitude)
    {
        Vector3 orginialPos = transform.localPosition;

        float elapsed = 0f; 

        while (elapsed < duration)
        {
            float dx = Random.Range(-1f, 1f) * magnitude;
            float dy = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(orginialPos.x + dx, orginialPos.y + dy, orginialPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = orginialPos;
    }
}
