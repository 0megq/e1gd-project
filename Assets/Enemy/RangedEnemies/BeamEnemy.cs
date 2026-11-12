using System.Collections;
using UnityEngine;

public class BeamEnemy : BaseEnemy
{
    [Header("Settings")]
    [SerializeField] private float attackRange = 12f;
    [SerializeField] private LayerMask lineOfSightLayers;
    [SerializeField] private float attackCooldown = 3f;
    [SerializeField] private float windUpTime = 1.5f;

    [Header("Weapon Reference")]
    [SerializeField] private BeamBulletController beam;

    private float nextAttackTime = 0f;
    private EnemyState currentState = EnemyState.Chasing;
    private Color baseColor;

    protected override void Start()
    {
        base.Start();
        
        baseColor = enemyRenderer.material.color;
    }

    void Update()
    {
        if (target == null || agent == null) return;
        UpdateState();
    }

    private void UpdateState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        switch (currentState)
        {
            case EnemyState.Chasing:
                agent.isStopped = false;
                agent.SetDestination(target.position);

                if (distanceToPlayer < attackRange && HasLineOfSight() && Time.time >= nextAttackTime)
                {
                    ChangeState(EnemyState.Attacking);
                }
                break;

            case EnemyState.Attacking:
                break;
        }
    }

    private void ChangeState(EnemyState newState)
    {
        if (currentState == newState) return;
        currentState = newState;

        switch (currentState)
        {
            case EnemyState.Attacking:
                StartCoroutine(AttackSequence());
                break;

            case EnemyState.Chasing:
                enemyRenderer.material.color = baseColor;
                break;
        }
    }

    private IEnumerator AttackSequence()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        enemyRenderer.material.color = Color.yellow;

        float timer = 0f;
        while (timer < windUpTime)
        {
            Vector3 lookPosition = target.position;
            lookPosition.y = transform.position.y;
            Vector3 direction = (lookPosition - transform.position).normalized;

            if (direction != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 5f);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        enemyRenderer.material.color = Color.red;
        base.Attack();

        yield return StartCoroutine(beam.FireSequence());

        nextAttackTime = Time.time + attackCooldown;
        ChangeState(EnemyState.Chasing);
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
}