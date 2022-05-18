using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeEnemy : PatrolEnemy
{
    
    [SerializeField] private GameObject monster;
    [SerializeField] private GameObject spikes;
    private Rigidbody2D reg;
    
    // Start is called before the first frame update
    void Start()
    {
     reg = spikes.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!isAlive()) UponDead();
        if (!colored)
        {
            // gameObject.SetActive(true);
            spikes.SetActive(false);
            Move();
        }
        else
        {
            // Instantiate(spikes);
            spikes.SetActive(true);
            reg.transform.position = rb.transform.position;
            // gameObject.SetActive(false);
        }
        
            
    }
    
    public override void OnColorChange(ColorManager.ColorLayer layer)
    {
        print("ok");
        base.OnColorChange(layer);
        colored = layer.index == gameObject.layer;
    }
}
