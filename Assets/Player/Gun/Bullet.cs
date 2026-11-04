using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public float lifeTime = 3f;

    private Rigidbody rb;

    [SerializeField] float damage = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = -transform.right * speed;
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player")) return;
        if (collider.CompareTag("Enemy"))
        {
            if (collider.TryGetComponent<BaseEnemy>(out BaseEnemy enemy))
            {
                enemy.ReceiveDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}
