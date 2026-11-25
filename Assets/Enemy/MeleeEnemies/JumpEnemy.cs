using UnityEngine;

public class JumpEnemy : BaseEnemy
{
    private float jumpHeight = 5f;
    private float jumpCooldown = 0.3f;


    void Update()
    {
        TrackPlayerTarget();
    }
}
