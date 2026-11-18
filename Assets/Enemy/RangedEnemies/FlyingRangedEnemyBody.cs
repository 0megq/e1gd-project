using UnityEngine;

public class FlyingRangedEnemyBody : BaseEnemy
{
    [SerializeField] private FlyingRangedEnemy enemy;

    public override void ReceiveDamage(float damage)
    {
        enemy.ReceiveDamage(damage);

        Debug.Log(health);
    }
}
