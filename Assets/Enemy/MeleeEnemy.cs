using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    private void Awake()
    {
        maxHealth = 100f;
        speed = 5f;
        damageAmount = 10f;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Attack()
    {
        base.Attack();
    }
}
