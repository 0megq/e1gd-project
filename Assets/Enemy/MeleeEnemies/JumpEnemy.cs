using NUnit.Framework.Interfaces;
using UnityEngine;

public class JumpEnemy : BaseEnemy
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float jumpForwardSpeed = 6f;
    [SerializeField] private float jumpCooldown = 0.3f;
    [SerializeField] private float landingDuration = 0.15f;
    [SerializeField] private float turnSpeed = 8f;

    [Header("Ground Check")]
    [SerializeField] private float groundCheckDistance = 0.6f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody rb;
    private Animator animator;
    private float stateTimer = 0f;
    private bool isGrounded = false;
    private Color baseColor;

    private EnemyState currentState = EnemyState.Landing;

    protected override void Start()
    {
        base.Start();

        TryGetComponent<Rigidbody>(out rb);
        TryGetComponent<Animator>(out animator);

        if (agent != null) agent.enabled = false;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.freezeRotation = true;
        }

        ChangeState(EnemyState.Idle);

        if (enemyRenderer != null)
        {
            baseColor = enemyRenderer.material.color;

            Debug.Log("Hi!");
        }
    }

    void Update()
    {
        TrackPlayerTarget();

        if (target == null || rb == null) return;

        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        UpdateState();
    }

    private void UpdateState()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                FacePlayer();

                stateTimer += Time.deltaTime;
                if (stateTimer >= jumpCooldown && isGrounded)
                {
                    ChangeState(EnemyState.Jumping);
                }
                break;

            case EnemyState.Jumping:
                stateTimer += Time.deltaTime;

                if (stateTimer > 0.1f && isGrounded && rb.linearVelocity.y <= 0f)
                {
                    ChangeState(EnemyState.Landing);
                }
                break;

            case EnemyState.Landing:
                stateTimer += Time.deltaTime;
                if (stateTimer >= landingDuration)
                {
                    ChangeState(EnemyState.Idle);
                }
                break;
        }
    }

    private void ChangeState(EnemyState newState)
    {
        if (currentState == newState) return;

        switch (currentState)
        {
            case EnemyState.Jumping:
                rb.linearVelocity = Vector3.zero;
                break;
        }

        currentState = newState;
        stateTimer = 0f;

        switch (currentState)
        {
            case EnemyState.Idle:
                if (enemyRenderer != null) enemyRenderer.material.color = baseColor;
                // if (animator != null) animator.SetTrigger("idle");
                break;

            case EnemyState.Jumping:
                JumpTowardsPlayer();
                if (enemyRenderer != null) enemyRenderer.material.color = Color.yellow;
                // if (animator != null) animator.SetTrigger("jump");
                break;

            case EnemyState.Landing:
                if (enemyRenderer != null) enemyRenderer.material.color = Color.gray;
                // if (animator != null) animator.SetTrigger("land");
                break;
        }
    }

    private void FacePlayer()
    {
        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0f;

        if (lookPos != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * turnSpeed);
        }
    }

    private void JumpTowardsPlayer()
    {
        float verticalVelocity = Mathf.Sqrt(2f * Mathf.Abs(Physics.gravity.y) * jumpHeight);

        Vector3 forwardDirection = target.position - transform.position;
        forwardDirection.y = 0f;
        forwardDirection.Normalize();

        rb.linearVelocity = (forwardDirection * jumpForwardSpeed) + (Vector3.up * verticalVelocity);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}