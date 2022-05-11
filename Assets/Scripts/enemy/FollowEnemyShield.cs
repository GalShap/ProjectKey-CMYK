using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemyShield : FollowPlayerEnemy
{
    [SerializeField] public GameObject shield;
    [SerializeField] private Rigidbody2D rig;
    
    [SerializeField] private Transform place1;
    [SerializeField] private Transform place2;

    protected void FixedUpdate()
    {
        if (!isAlive()) UponDead();
        Move();
        MoveShield();
    }

    private void MoveShield()
    {
        rig.transform.position = rb.velocity.x < 0 ? place1.position : place2.position;
    }
}