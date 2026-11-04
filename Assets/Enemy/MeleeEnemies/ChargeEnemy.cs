using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class ChargeEnemy : MeleeEnemy
{
    [SerializeField] private float minChargeRate = 10f;
    [SerializeField] private float maxChargeRate = 30f;

    private float chargeRate = 0f;
    private float nextChargeTime = 0f;
    private float chargeDuration = 3f;

    private Coroutine chargeRoutine = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        chargeRate = Random.Range(minChargeRate, maxChargeRate);

        nextChargeTime += Random.Range(minChargeRate, maxChargeRate) + chargeDuration;
    }

    private void ChangeState(EnemyState state)
    {
        if (currentState == state)
        {
            return;
        }

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

        if (state == EnemyState.Charging)
        {
            enemyRenderer.material.color = Color.yellow;
        }

        currentState = state;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null || agent == null)
        {
            return;
        }

        if (chargeRoutine != null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        if (distanceToPlayer < attackRange && HasLineOfSight())
        {
            ChangeState(EnemyState.Attacking);

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
        }
        else
        {
            ChangeState(EnemyState.Chasing);
            agent.isStopped = false;
            agent.SetDestination(target.position);
        }

        if (Time.time > nextChargeTime)
        {
            nextChargeTime += chargeRate + chargeDuration;

            chargeRate = Random.Range(minChargeRate, maxChargeRate);

            chargeRoutine = StartCoroutine(Charge());
        }
    }

    protected override void Attack()
    {
        base.Attack();
    }

    private IEnumerator Charge()
    {
        ChangeState(EnemyState.Charging);

        yield return new WaitForSeconds(chargeDuration);

        chargeRoutine = null;
    }
}
