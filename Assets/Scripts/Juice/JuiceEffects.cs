using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;

public class JuiceEffects : MonoBehaviour
{

    [SerializeField] private bool isRotate = false;

    [SerializeField] private bool isMoving = false;

    [SerializeField] private float rotateSpeed = 1;

    [SerializeField] private float moveSpeed = 1;

    [SerializeField] private float moveRadius = 1;
    
    [Tooltip("0 is move up and down, 1 is move left and right")]
    [SerializeField] private int moveDir;
    
    [Tooltip("0 is rotate right, 1 is rotate left!")]
    [SerializeField] private int rotateDir;

    private float initY;

    private float initX;
    
    private float y;

    private float x;

    private enum directionRotation
    {
        Left, Right
    }
    
    private enum directionMovement
    {
        Up, Down, Left ,Right
    }

    private enum moveType
    {
        UpDown, LeftRight
    }

    private Vector3 objectTransform;

    // Start is called before the first frame update
    void Start()
    {
        objectTransform = gameObject.GetComponent<Transform>().position;
        initY = objectTransform.y;
        initX = objectTransform.x;
    }


    private void FixedUpdate()
    {
        if (isMoving)
            Move();
        
        if (isRotate)
            Spin();
    }

    private void Spin()
    {
        var spinVec = rotateDir == (int) directionRotation.Right ? new Vector3(0, 0, 1) : 
            new Vector3(0, 0, -1);
        spinVec *= rotateSpeed;
        transform.Rotate(spinVec);
    }



    private void Move()
    {
        switch (moveDir)
        {
            case (int) moveType.UpDown:
                y = Mathf.PingPong( Time.time * moveSpeed, moveRadius);
                objectTransform = new Vector3(transform.localPosition.x,initY + y, transform.localPosition.z);
                transform.localPosition = objectTransform;
                break;
                    
                    
            case (int) moveType.LeftRight: 
                x = Mathf.PingPong(Time.time * moveSpeed, moveRadius);
                objectTransform = new Vector3(initX + x, transform.localPosition.y, transform.localPosition.z);
                transform.localPosition = objectTransform;
                break;
        }
    }
    
    
        
    
}