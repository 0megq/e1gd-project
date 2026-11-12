using System.Collections;
using UnityEngine;

public class BeamBulletController : MonoBehaviour
{
    [Header("Beam Settings")]
    [SerializeField] private float maxBeamLength = 12f;
    [SerializeField] private float beamExpandSpeed = 20f;
    [SerializeField] private float beamHoldDuration = 1f;

    [SerializeField] private LayerMask hitLayers;

    private float damage = 10f;

    private bool tookDamage = false;

    void Start()
    {
        gameObject.SetActive(false);
        gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0);
    }

    public IEnumerator FireSequence()
    {
        gameObject.SetActive(true);
        float currentLength = 0f;
        bool hasHitObstacle = false;

        while (currentLength < maxBeamLength && !hasHitObstacle)
        {
            float step = beamExpandSpeed * Time.deltaTime;

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, currentLength + step, hitLayers))
            {
                hasHitObstacle = true;

                currentLength = hit.distance;

                if (hit.collider.CompareTag("Player"))
                {
                    if (hit.collider.TryGetComponent<PlayerController>(out PlayerController player))
                    {
                        if (tookDamage == false)
                        {
                            player.ReceiveDamage(damage);
                            tookDamage = true;
                        }

                        Debug.Log("OUCH!!!!");
                    }
                }
            }
            else
            {
                currentLength += step;
                if (currentLength > maxBeamLength) currentLength = maxBeamLength;
            }

            gameObject.transform.localScale = new Vector3(0.3f, 0.3f, currentLength);

            yield return null;
        }

        yield return new WaitForSeconds(beamHoldDuration);

        gameObject.SetActive(false);
        gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0);

        tookDamage = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (tookDamage == false)
            {
                if (other.TryGetComponent<PlayerController>(out PlayerController player))
                {
                    player.ReceiveDamage(damage);
                    tookDamage = true;
                }

                Debug.Log("OUCH!!!!");
            }
        }
    }
}