using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeEnemy : PatrolEnemy
{
    
    [SerializeField] private GameObject monster;
    [SerializeField] private GameObject spikes;
    private Rigidbody2D reg;
    
    // Start is called before the first frame update
    public override void Start()
    { 
        base.Start();
        reg = spikes.GetComponent<Rigidbody2D>();
    }
    
    private void FixedUpdate()
    {
        if (!isAlive()) UponDead();
        if (!colored)
        {
            spikes.SetActive(false);
            Move();
        }
        else
        {
            spikes.SetActive(true);
            reg.transform.position = rb.transform.position;
        }
        
    }
    
}
