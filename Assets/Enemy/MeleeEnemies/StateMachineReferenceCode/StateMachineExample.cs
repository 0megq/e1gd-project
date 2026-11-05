using UnityEngine;

public class StateMachineExample : BaseEnemy
{
    [SerializeField] protected float attackRange = 2f;

    [SerializeField] private LayerMask lineOfSightLayers;

    [SerializeField] protected float baseAttackRate = 2f;
    protected float attackRate = 0f;
    protected float nextAttackTime = 0f;

    protected EnemyState currentState = EnemyState.Chasing;

    protected Color baseColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        baseColor = enemyRenderer.material.color;

        attackRate = baseAttackRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null || agent == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        if (distanceToPlayer < attackRange && HasLineOfSight())
        {
            ChangeState(EnemyState.Attacking);

        }
        else
        {
            ChangeState(EnemyState.Chasing);
            agent.isStopped = false;
            agent.SetDestination(target.position);
        }
    }

    // THIS IS THE MAIN EXAMPLE!! Code here is not tested. It's here to show the idea of the state machine
    void UpdateState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);
        switch(currentState)
        {
            case EnemyState.Idle:
                if (target != null && agent != null) ChangeState(EnemyState.Chasing);
                break;
            case EnemyState.Chasing:    
                agent.isStopped = false;
                agent.SetDestination(target.position);
                if (distanceToPlayer < attackRange && HasLineOfSight()) ChangeState(EnemyState.Attacking);
                break;
            case EnemyState.Attacking:
                agent.isStopped = true;
                agent.velocity = Vector3.zero;

                Vector3 lookPosition = target.position;

                lookPosition.y = transform.position.y;

                transform.LookAt(lookPosition);

                if (Time.time >= nextAttackTime)
                {
                    Attack();

                    nextAttackTime = Time.time + attackRate;
                }
                break;
            case EnemyState.Fleeing:
                //instead of setting attackDamage you can also setup Attack function to take
                // a parameter like this Attack(baseAttackRate * 2);
                Attack();
                break;
        }
    }

    private void ChangeState(EnemyState state)
    {
        // 
        if (currentState == state)
        {
            return;
        }

        // do a switch case statement to handle exit code
        // switch(currentState)

        // this handles enter code, but should use switch case instead
        if (state == EnemyState.Attacking)
        {
            enemyRenderer.material.color = Color.red;

            attackRate = baseAttackRate;
        }

        if (state == EnemyState.Chasing)
        {
            enemyRenderer.material.color = baseColor;
        }

        if (state == EnemyState.Fleeing)
        {
            enemyRenderer.material.color = Color.green;

            attackRate = baseAttackRate * 2;
        }

        currentState = state;
    }

    protected bool HasLineOfSight()
    {
        Vector3 startPosition = transform.position;
        Vector3 directionToPlayer = (target.position - transform.position).normalized;

        if (Physics.Raycast(startPosition, directionToPlayer, out RaycastHit hit, attackRange, lineOfSightLayers))
        {
            if (hit.transform == target)
            {
                return true;
            }
        }

        return false;
    }

    protected override void Attack()
    {
        base.Attack();
    }
}
