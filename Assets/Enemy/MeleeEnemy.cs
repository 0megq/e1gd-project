using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : BaseEnemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        Attack();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Attack()
    {
        base.Attack();

        if (agent != null && target != null)
        {
            agent.SetDestination(target.position);
        }
    }
}
