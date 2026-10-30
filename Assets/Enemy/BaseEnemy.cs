using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] protected float health = 0f;
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected float damageAmount = 10f;

    [SerializeField] protected Transform target;

    protected NavMeshAgent agent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        health = maxHealth;

        TryGetComponent<NavMeshAgent>(out agent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ReceiveDamage(float damage)
    {

    }

    protected virtual void Attack()
    {

    }
}
