using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemyShield : FollowPlayerEnemy
{
    [SerializeField] public GameObject shield;
    [SerializeField] private Rigidbody2D rig;
    
    [SerializeField] private Transform place1;
    [SerializeField] private Transform place2;
    private float x = 0f;
    protected void FixedUpdate()
    {
        if (!isAlive()) UponDead();
        Move();
        MoveShield();
    }

    private void MoveShield()
    {
        if (x < 0 && rb.velocity.x > 0)
        {
            rb.transform.Rotate(0, 180, 0);
        }
        if (x > 0 && rb.velocity.x < 0)
        {
            rb.transform.Rotate(0, -180, 0);
        }
        x = rb.velocity.x;
    }
}