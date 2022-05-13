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
        x = rb.velocity.x;
        rig.transform.position = x < 0 ? place1.position : place2.position;
        shield.GetComponent<SpriteRenderer>().flipX = x < 0 ? true: false;
    }
}