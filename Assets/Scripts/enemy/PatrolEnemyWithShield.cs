using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemyWithShield : PatrolEnemy
{
    [SerializeField] public GameObject shield;
    [SerializeField] private Rigidbody2D rig;
    
    [SerializeField] private Transform place1;
    [SerializeField] private Transform place2;
    private float x = 0f;

    private void FixedUpdate()
    {
        if (!isAlive()) UponDead();
        Move();
        if(rb.velocity.x != x)
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