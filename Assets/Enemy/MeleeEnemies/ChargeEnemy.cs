using UnityEngine;

public class ChargeEnemy : MeleeEnemy
{
    [Header("Charge Settings")]
    [SerializeField] private float minChargeRate = 3f;
    [SerializeField] private float maxChargeRate = 5f;
    [SerializeField] private float chargeDuration = 3f;
    [SerializeField] private float lungeDuration = 1f;
    [SerializeField] private float attackOutDuration = 0.75f;
    [SerializeField] private float chargeSpeed = 15f;

    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float chargeRange = 10f;

    private float chargeRate = 0f;
    private float nextChargeTime = 0f;
    private float stateTimer = 0f;
    private Rigidbody rb = null;
    private Animator animator = null;

    protected override void Start()
    {
        base.Start();

        chargeRate = Random.Range(minChargeRate, maxChargeRate);
        nextChargeTime = Time.time + chargeRate;

        TryGetComponent<Animator>(out animator);

        TryGetComponent<Rigidbody>(out rb);
        if (rb != null) rb.isKinematic = true;

        currentState = EnemyState.Idle;
    }

    protected override void Update()
    {
        TrackPlayerTarget();

        if (target == null || agent == null) return;

        UpdateState();
    }

    private void UpdateState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        Vector3 lookPosition = target.position;
        lookPosition.y = transform.position.y;

        switch (currentState)
        {
            case EnemyState.Idle:
                agent.isStopped = true;
                transform.LookAt(lookPosition);

                ChangeState(EnemyState.Chasing);

                break;

            case EnemyState.Chasing:
                agent.isStopped = false;
                agent.SetDestination(target.position);
                if (Time.time >= nextChargeTime && distanceToPlayer < chargeRange && HasLineOfSight())
                {
                    ChangeState(EnemyState.Charging);
                    break;
                }
                break;

            case EnemyState.Attacking:
                agent.isStopped = true;
                agent.velocity = Vector3.zero;

                transform.LookAt(lookPosition);

                if (Time.time >= nextChargeTime)
                {
                    ChangeState(EnemyState.Charging);
                    break;
                }

                if (distanceToPlayer > attackRange || !HasLineOfSight())
                {
                    ChangeState(EnemyState.Idle);
                    break;
                }

                if (Time.time >= nextAttackTime)
                {
                    Attack();
                    nextAttackTime = Time.time + attackRate;
                }
                break;

            case EnemyState.Charging:
                Vector3 directionToTarget = (lookPosition - transform.position).normalized;
                if (directionToTarget != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
                }

                stateTimer += Time.deltaTime;
                if (stateTimer >= chargeDuration)
                {
                    ChangeState(EnemyState.Lunging);
                }
                break;

            case EnemyState.Lunging:
                stateTimer += Time.deltaTime;
                // If enemy slows down too much (by e.g. hitting a wall) immediately leave the state
                // Also leave if this state reaches its max duration
                if (stateTimer >= lungeDuration || rb.linearVelocity.sqrMagnitude < 20.0f)
                {
                    ChangeState(EnemyState.AttackOut);
                }
                break;

            case EnemyState.AttackOut:
                stateTimer += Time.deltaTime;
                if (stateTimer >= attackOutDuration)
                {
                    ChangeState(EnemyState.Chasing);
                }
                break;
        }
    }

    private void ChangeState(EnemyState newState)
    {
        if (currentState == newState) return;

        switch (currentState)
        {
            case EnemyState.Lunging:
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.isKinematic = true;
                }

                agent.enabled = true;
                agent.Warp(transform.position);

                chargeRate = Random.Range(minChargeRate, maxChargeRate);
                nextChargeTime = Time.time + chargeRate;
                break;
        }

        currentState = newState;
        stateTimer = 0f;

        switch (currentState)
        {
            case EnemyState.Idle:
                if (enemyRenderer != null) enemyRenderer.material.color = baseColor;
                if (animator != null) animator.Play("idle");
                break;

            case EnemyState.Attacking:
                if (enemyRenderer != null) enemyRenderer.material.color = Color.red;
                attackRate = baseAttackRate;
                break;

            case EnemyState.Chasing:
                if (enemyRenderer != null) enemyRenderer.material.color = baseColor;
                if (animator != null) animator.SetTrigger("walk");
                break;

            case EnemyState.Fleeing:
                if (enemyRenderer != null) enemyRenderer.material.color = Color.green;
                attackRate = baseAttackRate * 2;
                break;

            case EnemyState.Charging:
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
                if (enemyRenderer != null) enemyRenderer.material.color = Color.yellow;
                if (animator != null) animator.SetTrigger("charge");
                break;

            case EnemyState.Lunging:
                agent.enabled = false;
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.linearVelocity = transform.forward * chargeSpeed;
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                }
                if (enemyRenderer != null) enemyRenderer.material.color = new Color(1f, 0.5f, 0f);
                if (animator != null) animator.SetTrigger("lunge");
                break;

            case EnemyState.AttackOut:
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
                if (enemyRenderer != null) enemyRenderer.material.color = Color.gray;
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ChangeState(EnemyState.AttackOut);

            Attack();
        }
    }
}