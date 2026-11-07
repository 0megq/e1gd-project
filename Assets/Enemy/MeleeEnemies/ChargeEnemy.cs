using System.Collections;
using UnityEngine;

public class ChargeEnemy : MeleeEnemy
{
    [Header("Charge Settings")]
    [SerializeField] private float minChargeRate = 10f;
    [SerializeField] private float maxChargeRate = 30f;
    [SerializeField] private float chargeDuration = 3f;
    [SerializeField] private float chargeSpeed = 10f;

    private float chargeRate = 0f;
    private float nextChargeTime = 0f;
    private Coroutine chargeRoutine = null;
    private Rigidbody rb = null;

    protected override void Start()
    {
        base.Start();

        chargeRate = Random.Range(minChargeRate, maxChargeRate);
        nextChargeTime = Time.time + chargeRate + chargeDuration;

        TryGetComponent<Rigidbody>(out rb);
        if (rb != null) rb.isKinematic = true;
    }

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
                ChangeState(EnemyState.Chasing);
                break;

            case EnemyState.Chasing:
                agent.isStopped = false;
                agent.SetDestination(target.position);

                if (Time.time >= nextChargeTime)
                {
                    ChangeState(EnemyState.Charging);
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

                if (Time.time >= nextChargeTime)
                {
                    ChangeState(EnemyState.Charging);
                    break;
                }

                if (distanceToPlayer > attackRange || !HasLineOfSight())
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

            case EnemyState.Charging:
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

                case EnemyState.Charging:
                    enemyRenderer.material.color = Color.yellow;
                    if (chargeRoutine == null)
                    {
                        chargeRoutine = StartCoroutine(Charge());
                    }
                    break;
            }
        }
    }

    private IEnumerator Charge()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        float timer = 0f;

        while (timer < chargeDuration)
        {
            Vector3 lookPosition = target.position;
            lookPosition.y = transform.position.y;
            Vector3 directionToTarget = (lookPosition - transform.position).normalized;
            if (directionToTarget != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        agent.enabled = false;
        rb.isKinematic = false;

        rb.linearVelocity = transform.forward * chargeSpeed;

        yield return new WaitForSeconds(chargeDuration / 3);

        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;
        agent.enabled = true;
        agent.Warp(transform.position);

        chargeRate = Random.Range(minChargeRate, maxChargeRate);
        nextChargeTime = Time.time + chargeRate + chargeDuration;

        chargeRoutine = null;
        ChangeState(EnemyState.Chasing);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (chargeRoutine != null)
            {
                StopCoroutine(chargeRoutine);

                rb.linearVelocity = Vector3.zero;
                rb.isKinematic = true;
                agent.enabled = true;
                agent.Warp(transform.position);

                chargeRate = Random.Range(minChargeRate, maxChargeRate);
                nextChargeTime = Time.time + chargeRate + chargeDuration;

                chargeRoutine = null;

                ChangeState(EnemyState.Attacking);
            }

            Attack();
        }
    }
}