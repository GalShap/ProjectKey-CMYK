using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeEnemy : PatrolEnemy
{
    
    [SerializeField] private GameObject monster;
    [SerializeField] private GameObject spikes;
    // private Rigidbody2D reg;
    private Animator _animator;
    private LayerMask originalLayer;
    
    private static readonly int Walking = Animator.StringToHash("Walking");
    private static readonly int Disappear = Animator.StringToHash("Disappear");
    private static readonly int Reappear = Animator.StringToHash("Reappear");

    // Start is called before the first frame update
    public override void Start()
    { 
        base.Start();
        // reg = spikes.GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        originalLayer = gameObject.layer;
        // if (_animator == null)
        //     _animator = GetComponentInChildren<Animator>();
    }
    
    private void FixedUpdate()
    {
        if (!isAlive()) UponDead();
        if (!colored)
        {
            Move();
        }
    }


    public override void OnColorChange(ColorManager.ColorLayer layer)
    {
        base.OnColorChange(layer);
        if (colored)
        {
            if (IsOneHit)
            {
                DisableSpikes();
                _animator.SetTrigger(Reappear);
                _animator.SetBool(Walking, true);
                colored = false;
            }
            else
            {
                _animator.SetBool(Walking, false);
                _animator.SetTrigger(Disappear);   
            }
            // reg.transform.position = rb.transform.position;
        }
        else
        {
            DisableSpikes();
            _animator.SetTrigger(Reappear);
            _animator.SetBool(Walking, true);
        }
    }

    public void EnableSpikes()
    {
        gameObject.layer = (int) Mathf.Log(ColorManager.NeutralLayer,2);
        // tag = "Spikes";
        IsOneHit = true;
    }
    
    public void DisableSpikes()
    {
        gameObject.layer = originalLayer;
        // tag = "Monster";
        IsOneHit = false;
    }

    public void OnDeathEnd()
    {
        gameObject.SetActive(false);
    }
}
