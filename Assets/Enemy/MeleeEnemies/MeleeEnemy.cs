using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : BaseEnemy
{
    [SerializeField] protected float attackRange = 2f;
    [SerializeField] private LayerMask lineOfSightLayers;
    [SerializeField] protected float baseAttackRate = 2f;

    protected float attackRate = 0f;
    protected float nextAttackTime = 0f;

    protected EnemyState currentState = EnemyState.Chasing;
    protected Color baseColor;

    protected override void Start()
    {
        base.Start();
        baseColor = enemyRenderer.material.color;
        attackRate = baseAttackRate;
    }

    protected virtual void Update()
    {
        if (target == null || agent == null)
        {
            return;
        }

        UpdateState();
    }

    private void UpdateState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        switch (currentState)
        {
            case EnemyState.Idle:
                ChangeState(EnemyState.Chasing);
                break;

            case EnemyState.Chasing:
                agent.isStopped = false;
                agent.SetDestination(target.position);

                if (distanceToPlayer < attackRange && HasLineOfSight())
                {
                    ChangeState(EnemyState.Attacking);
                }
                break;

            case EnemyState.Attacking:
                agent.isStopped = true;
                agent.velocity = Vector3.zero;

                Vector3 lookPosition = target.position;
                lookPosition.y = transform.position.y;
                transform.LookAt(lookPosition);

                if (distanceToPlayer >= attackRange || !HasLineOfSight())
                {
                    ChangeState(EnemyState.Chasing);
                    break;
                }

                if (Time.time >= nextAttackTime)
                {
                    Attack();
                    nextAttackTime = Time.time + attackRate;
                }
                break;
        }
    }

    private void ChangeState(EnemyState newState)
    {
        if (currentState == newState)
        {
            return;
        }

        currentState = newState;

        switch (currentState)
        {
            case EnemyState.Attacking:
                enemyRenderer.material.color = Color.red;
                attackRate = baseAttackRate;
                break;

            case EnemyState.Chasing:
                enemyRenderer.material.color = baseColor;
                break;

            case EnemyState.Fleeing:
                enemyRenderer.material.color = Color.green;
                attackRate = baseAttackRate * 2;
                break;
        }
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