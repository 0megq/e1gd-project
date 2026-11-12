using UnityEngine;
using UnityEngine.AI;

public class FlyingRangedEnemy : BaseEnemy
{
    [SerializeField] private float flightHeight = 10f;
    [SerializeField] private float bobbingSpeed = 2f;
    [SerializeField] private float bobbingAmount = 1f;

    [SerializeField] private Transform body;

    [SerializeField] private float radius = 2f;

    private Color baseColor;

    private float flightTimer = 0f;

    private EnemyState currentState = EnemyState.Chasing;

    [SerializeField] private float attackRange = 20f;
    [SerializeField] private float runDistance = 7.5f;
    [SerializeField] private float fleeDistance = 5f;
    [SerializeField] private LayerMask lineOfSightLayers;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float baseFireRate = 2f;

    private float fireRate = 0f;
    private float nextFireTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        if (body.TryGetComponent<Renderer>(out Renderer enemyRenderer))
        {
            baseColor = enemyRenderer.material.color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null || agent == null) return;

        UpdateState();
        UpdateFlight();
    }

    private void UpdateFlight()
    {
        float desiredY = flightHeight + (Mathf.Sin(flightTimer += Time.deltaTime * bobbingSpeed) * bobbingAmount);

        Vector3 startPos = transform.position + new Vector3(0, 0.1f, 0);

        if (Physics.SphereCast(startPos, radius, Vector3.up, out RaycastHit hit, desiredY + radius, lineOfSightLayers))
        {
            float maxSafeHeight = hit.distance - (radius * 0.5f);

            desiredY = Mathf.Min(desiredY, maxSafeHeight);
        }

        Vector3 targetHeight = new Vector3(0, desiredY, 0);
        body.localPosition = Vector3.Lerp(body.localPosition, targetHeight, Time.deltaTime * 3f);

        if (target != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target.position - body.position);
            body.rotation = Quaternion.Slerp(body.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    private void UpdateState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        switch (currentState)
        {
            case EnemyState.Chasing:
                agent.isStopped = false;
                agent.SetDestination(target.position);

                if (distanceToPlayer < fleeDistance)
                {
                    ChangeState(EnemyState.Fleeing);
                    break;
                }

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

                if (distanceToPlayer < fleeDistance)
                {
                    ChangeState(EnemyState.Fleeing);
                    break;
                }

                if (distanceToPlayer > attackRange || !HasLineOfSight())
                {
                    ChangeState(EnemyState.Chasing);
                    break;
                }

                if (Time.time >= nextFireTime)
                {
                    Attack();
                    nextFireTime = Time.time + fireRate;
                }
                break;

            case EnemyState.Fleeing:
                if (Time.time >= nextFireTime)
                {
                    Attack();
                    nextFireTime = Time.time + fireRate;
                }

                if (HasReachedDestination())
                {
                    if (distanceToPlayer < attackRange && HasLineOfSight())
                    {
                        ChangeState(EnemyState.Attacking);
                    }
                    else
                    {
                        ChangeState(EnemyState.Chasing);
                    }
                }
                break;
        }
    }

    private void ChangeState(EnemyState newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        if (body.TryGetComponent<Renderer>(out Renderer enemyRenderer))
        {
            switch (currentState)
            {
                case EnemyState.Attacking:
                    enemyRenderer.material.color = Color.red;
                    fireRate = baseFireRate;
                    break;

                case EnemyState.Chasing:
                    enemyRenderer.material.color = baseColor;
                    break;

                case EnemyState.Fleeing:
                    enemyRenderer.material.color = Color.green;
                    fireRate = baseFireRate * 2;

                    CalculateFleePath();
                    break;
            }
        }
    }

    private bool HasReachedDestination()
    {
        if (agent.pathPending) return false;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                return true;
            }
        }
        return false;
    }

    private void CalculateFleePath()
    {
        Vector3 directionAway = (transform.position - target.position).normalized;
        Vector3 fleePosition = transform.position + (directionAway * runDistance);

        if (NavMesh.SamplePosition(fleePosition, out NavMeshHit hit, runDistance, NavMesh.AllAreas))
        {
            agent.isStopped = false;
            agent.SetDestination(hit.position);
        }
        else
        {
            ChangeState(EnemyState.Attacking);
        }
    }

    private bool HasLineOfSight()
    {
        Vector3 startPosition = body.position;
        Vector3 directionToPlayer = (target.position - body.position).normalized;

        if (Physics.Raycast(startPosition, directionToPlayer, out RaycastHit hit, attackRange, lineOfSightLayers))
        {
            if (hit.transform == target) return true;
        }
        return false;
    }

    protected override void Attack()
    {
        base.Attack();

        Vector3 directionToPlayer = (target.position - body.position).normalized;
        Vector3 spawnPos = body.position + (directionToPlayer * 1.5f);

        Instantiate(bullet, spawnPos, Quaternion.LookRotation(directionToPlayer));
    }
}
