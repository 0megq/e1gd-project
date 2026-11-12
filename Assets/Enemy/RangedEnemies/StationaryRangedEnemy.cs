using UnityEngine;

public class StationaryRangedEnemy : RangedEnemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        currentState = EnemyState.Idle;

        agent.Warp(transform.position);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (target == null || agent == null) return;

        UpdateState();
    }

    private void UpdateState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        switch (currentState)
        {
            case EnemyState.Idle:
                if(distanceToPlayer <= attackRange && HasLineOfSight())
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

                if (distanceToPlayer > attackRange || !HasLineOfSight())
                {
                    ChangeState(EnemyState.Idle);
                    break;
                }

                if (Time.time >= nextFireTime)
                {
                    Attack();
                    nextFireTime = Time.time + fireRate;
                }
                break;
        }
    }

    private void ChangeState(EnemyState newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        if (TryGetComponent<Renderer>(out Renderer enemyRenderer))
        {
            switch (currentState)
            {
                case EnemyState.Idle:
                    enemyRenderer.material.color = baseColor;
                    break;

                case EnemyState.Attacking:
                    enemyRenderer.material.color = Color.red;
                    fireRate = baseFireRate;
                    break;
            }
        }
    }

    private bool HasLineOfSight()
    {
        Vector3 startPosition = transform.position;
        Vector3 directionToPlayer = (target.position - transform.position).normalized;

        if (Physics.Raycast(startPosition, directionToPlayer, out RaycastHit hit, attackRange, lineOfSightLayers))
        {
            if (hit.transform == target) return true;
        }
        return false;
    }

    protected override void Attack()
    {
        base.Attack();
    }
}
