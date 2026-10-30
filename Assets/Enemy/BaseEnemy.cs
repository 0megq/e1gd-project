using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    protected float health;
    protected float maxHealth;
    protected float speed;
    protected float damageAmount;

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
