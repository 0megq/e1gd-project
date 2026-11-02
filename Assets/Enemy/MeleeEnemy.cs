using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    [SerializeField] private float attackRange = 2f;

    [SerializeField] private LayerMask lineOfSightLayers;

    [SerializeField] private float attackRate = 2f;
    private float nextAttackTime = 0f;

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

        if (distanceToPlayer < attackRange && HasLineOfSight())
        {
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

    protected override void Attack()
    {
        base.Attack();
    }
}
