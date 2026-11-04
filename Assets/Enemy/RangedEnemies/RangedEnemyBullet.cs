using UnityEngine;

public class RangedEnemyBullet : MonoBehaviour
{
    public float speed = 15f;
    public float lifeTime = 3f;

    private Rigidbody rb = null;

    private float damage = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TryGetComponent<Rigidbody>(out rb);

        if (rb != null)
        {
            rb.linearVelocity = transform.forward * speed;
        }

        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerController>(out PlayerController player))
            {
                player.ReceiveDamage(damage);
            }
        }
    }
}
