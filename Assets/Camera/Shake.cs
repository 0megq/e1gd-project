using System.Collections;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public static bool shake = false;

    void Update() {
        if (shake)
        {
            shake = false;
            StartShake();
        }
    }

    void StartShake(float duration = 0.1f, float strength = 0.1f) {
        StartCoroutine(Shaking(duration, strength));
    }

    IEnumerator Shaking(float duration, float strength) {
        Vector3 startPosition = transform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            transform.localPosition = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }

        transform.localPosition = startPosition;
    }
}
