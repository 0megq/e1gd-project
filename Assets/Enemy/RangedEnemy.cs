using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.LowLevel;

public class RangedEnemy : BaseEnemy
{
    [SerializeField] private float attackRange = 10f;

    [SerializeField] private float runDistance = 5f;

    [SerializeField] private LayerMask lineOfSightLayers;

    [SerializeField] private GameObject bullet;

    [SerializeField] private float fireRate = 2f;
    private float nextFireTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null || agent == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        if (distanceToPlayer < runDistance)
        {
            Run();
        }
        else if (distanceToPlayer < attackRange && HasLineOfSight())
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;

            Vector3 lookPosition = target.position;

            lookPosition.y = transform.position.y;

            transform.LookAt(lookPosition);

            if (Time.time >= nextFireTime)
            {
                Attack();

                nextFireTime = Time.time + fireRate;
            }
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
        }
    }

    private bool HasLineOfSight()
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

    private void Run()
    {
        Vector3 directionAway = (transform.position - target.position).normalized;

        Vector3 fleePosition = transform.position + directionAway * runDistance;

        if (NavMesh.SamplePosition(fleePosition, out NavMeshHit hit, runDistance, NavMesh.AllAreas))
        {
            agent.isStopped = false;
            agent.SetDestination(hit.position);
        }
        else
        {
            agent.isStopped = true;

            Vector3 lookPosition = target.position;

            lookPosition.y = transform.position.y;

            transform.LookAt(lookPosition);

            if (Time.time >= nextFireTime)
            {
                Attack();

                nextFireTime = Time.time + fireRate;
            }
        }
    }

    protected override void Attack()
    {
        base.Attack();

        Instantiate(bullet, transform.position + (transform.forward * 1.5f), transform.rotation);
    }
}
