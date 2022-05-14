using System.Collections;
using System.Collections.Generic;
using Avrahamy.Math;
using UnityEngine;
using System;

public class RoundMonster : EnemyObject
{
    // public float speed = 6f; // The speed that the player will move at.
    private bool clockWise;
    [SerializeField] private float maxLeft;
    [SerializeField] private float maxRight;
    [SerializeField] private float currentDiag;
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject cannon;
    [SerializeField] public Transform shootingSpot;
    [SerializeField] float smooth = 1.0f;
    [SerializeField] float tiltAngle = 60.0f;
    private float tiltAngle_ = 60.0f;
    [SerializeField] float attackAngleFromPlayer = 3.0f;
    Quaternion start;
    Quaternion target;
    private float len = 0;

    void Awake()
    {
        start = gameObject.transform.rotation;
        movement = gameObject.transform.position;
        rb = GetComponent<Rigidbody2D>();
        tiltAngle_ = tiltAngle;
        target = Quaternion.Euler(0, 0, tiltAngle_) * start;
    }


    void Update()
    {
        if (!isAlive()) UponDead();
        // Move the player around the scene.
        // if (IsInDegreeRange(attackAngleFromPlayer))
            // return;
        Move();
    }

    void Move()
    {
        // Rotate the cube by converting the angles into a quaternion.

        if ((target == transform.rotation))
        {
            tiltAngle_ *= -1;
            target = Quaternion.Euler(0, 0, tiltAngle_) * start;
        }
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target, Time.deltaTime * smooth);
        
    }


    public bool IsInDegreeRange(float degreeRange)
    {
        Vector3 playerPosition = player.transform.position;
        Vector3 dir = playerPosition - transform.position;
        float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        return transform.rotation == Quaternion.AngleAxis(angle, Vector3.forward);
        // float deg = transform.position.x - transform.position.y
    }

    protected override void UponDead()
    {
        throw new System.NotImplementedException();
    }
}