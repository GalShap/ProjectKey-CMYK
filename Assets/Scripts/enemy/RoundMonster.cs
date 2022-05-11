using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    float smooth = 5.0f;
    float tiltAngle = 60.0f; 
    Vector3 start;
    private float len = 0;

    void Awake()
    {
        start = gameObject.transform.position;
        movement = gameObject.transform.position;
        rb = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        if (!isAlive()) UponDead();
        // Store the input axes.
        // Smoothly tilts a transform towards a target rotation.
        len = clockWise? (len +1): (len -1);
        if (len == tiltAngle)
        {
            clockWise = false;
        }
        else if(len == -tiltAngle)
        {
            clockWise = true;
        }
        float tiltAroundZ =  len;
        // float tiltAroundZ = (clockWise? len: -len) * len;
        float tiltAroundX =  len;
        // float tiltAroundX = (clockWise? len: -len) * len;
        
        // Move the player around the scene.
        Move(tiltAroundZ);
    }

    void Move(float v)
    {
        // Rotate the cube by converting the angles into a quaternion.
        Quaternion target = Quaternion.Euler(0, 0, v);

        // Dampen towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, target,  Time.deltaTime * smooth);


    }

    void Shoot()
    {
        
    }

    protected override void UponDead()
    {
        throw new System.NotImplementedException();
    }
    
    
    

   
}