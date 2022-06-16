using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeEnemy : PatrolEnemy
{
    
    [SerializeField] private GameObject monster;
    [SerializeField] private GameObject spikes;
    // private Rigidbody2D reg;
    private Animator _animatorr;
    private int originalLayer;
    private ColorObject _colorObject;
    private EnemyHealth _health;
    
    private static readonly int Walking = Animator.StringToHash("Walking");
    private static readonly int Disappear = Animator.StringToHash("Disappear");
    private static readonly int Reappear = Animator.StringToHash("Reappear");

    // Start is called before the first frame update
    public override void Start()
    { 
        base.Start();
        // reg = spikes.GetComponent<Rigidbody2D>();
        _animatorr = GetComponent<Animator>();
        _colorObject = GetComponent<ColorObject>();
        _health = GetComponent<EnemyHealth>();
        originalLayer = _colorObject.LayerIndex;
        
        _animatorr.SetBool(Walking, true);
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
        colored = layer.index == originalLayer; 
        if(!isAlive()) return;
        if (colored)
        {
            EnableSpikes();
            _animatorr.SetBool(Walking, false);
            _animatorr.SetTrigger(Disappear);
        }
        else
        {
            DisableSpikes();
            _animatorr.SetTrigger(Reappear);
            _animatorr.SetBool(Walking, true);
        }
    }

    public void EnableSpikes()
    {
        gameObject.layer = ColorManager.NeutralIndex;
        IsOneHit = true;
        _health.damagable = false;
    }
    
    public void DisableSpikes()
    {
        gameObject.layer = originalLayer;
        IsOneHit = false;
        _health.damagable = true;
    }

    public void OnDeathEnd()
    {
        gameObject.SetActive(false);
    }
}
