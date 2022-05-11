using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemyWhithShield : PatrolEnemy
{
    [SerializeField] public GameObject shield;
    [SerializeField] private Rigidbody2D rig;

    private void FixedUpdate()
    {
        if (!isAlive()) UponDead();
        Move();
    }

    private void Move()
    {
        var x = transform.position.x - places[counter].position.x;
        rb.velocity = new Vector2((x <= 0 ? speed : -speed),
            rb.velocity.y);
        if (Math.Abs(x) > 0.1f) return;
        counter = (counter >= places.Length - 1) ? 0 : counter + 1;
        rig.transform.position = new Vector2(((x < 0) ? rig.transform.position.x  : rig.transform.position.x *-1),
            rig.transform.position.y);
        // shield.GetComponent<Rigidbody2D>().transform.position = new Vector3(
            // ((x >= 0) ? shield.transform.position.x : (-1 * shield.transform.position.x)), shield.transform.position.y);

    }
}