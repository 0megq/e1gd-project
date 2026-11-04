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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.TryGetComponent<BaseEnemy>(out BaseEnemy enemy))
            {
                enemy.ReceiveDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}
