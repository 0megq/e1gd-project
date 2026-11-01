using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    protected float health = 0f;
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected float damageAmount = 10f;

    [SerializeField] protected Transform target;

    protected NavMeshAgent agent = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        health = maxHealth;

        Debug.Log(health);

        TryGetComponent<NavMeshAgent>(out agent);

        if (agent != null)
        {
            agent.speed = speed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ReceiveDamage(float damage)
    {
        if (health - damage <= 0)
        {
            Debug.Log(health);

            Destroy(gameObject);
        }

        health -= damage;
    }

    protected virtual void Attack()
    {
        Debug.Log("Attack!");
    }
}
