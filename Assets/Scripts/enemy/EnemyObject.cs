using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyObject : MonoBehaviour
{
    protected int lifeCount = 3;
    protected abstract void UponDead();
    protected Rigidbody2D rb;
    protected Vector3 movement;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public bool isAlive()
    {
        if (lifeCount > 0)
        {
            return true;
        }
        return false;
    }

    


}
