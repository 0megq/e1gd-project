using UnityEngine;

public class JumpEnemy : BaseEnemy
{
    private float jumpHeight = 5f;
    private float jumpCooldown = 0.3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
