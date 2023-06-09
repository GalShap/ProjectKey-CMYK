using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PatrolEnemyWithShield : PatrolEnemy
{
    [SerializeField] public GameObject shield;
    [SerializeField] private Rigidbody2D rig;
    [SerializeField] private EnemyHealth health;
    
    [SerializeField] private Transform place1;
    [SerializeField] private Transform place2;

    [FormerlySerializedAs("player")] [SerializeField] private Transform playerTrans;

    private bool facingRight;
    private bool FacingPlayer => facingRight && playerTrans.position.x > transform.position.x;
    private float x = 0f;

    private void Update()
    {
        if(!health.IsAlive)
            shield.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (!isAlive()) UponDead();
        Move();
        if(rb.velocity.x != x)
            MoveShield();

        health.damagable = (colored && shield.layer != ColorManager.NeutralIndex) || !FacingPlayer;
    }

    private void MoveShield()
    {
        if (x < 0 && rb.velocity.x > 0)
        {
            rb.transform.Rotate(0, -180, 0);
        }
        if (x > 0 && rb.velocity.x < 0)
        {
            rb.transform.Rotate(0, 180, 0);
        }
        x = rb.velocity.x;
    }

    public override void OnColorChange(ColorManager.ColorLayer layer)
    {
        colored = layer.index == shield.layer;
    }
}