using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The class definition for a projectile's trigger
/// </summary>
/// <remarks>
public class CannonScript : EnemyObject
{
    /// Attach this script as a component to any object capable of triggering projectiles
    /// 
    public GameObject m_Projectile; // this is a reference to your projectile prefab

    public Transform m_SpawnTransform; // this is a reference to the transform where the prefab will spawn
    public GameObject player;
    [SerializeField] private float len = 4f;
    [SerializeField] private float timerCounter = 3;
    private float timer = 0;


    /// <summary>
    /// Message that is called once per frame
    /// </summary>
    private void Update()
    {
        timer -= Time.deltaTime;
        if (!(timer <= 0)) return;
        Shoot();
        timer = timerCounter;
    }
    
    protected void Shoot()
    {
        if (!colored)
        {
            Instantiate(m_Projectile, m_SpawnTransform.position, m_SpawnTransform.rotation);
        }
        
    }

    private float PositionX()
    {
        return (transform.position.x - player.transform.position.x);
    }

    public override void OnColorChange(ColorManager.ColorLayer layer)
    {
        base.OnColorChange(layer);
        colored = layer.index == gameObject.layer;
    }
    protected override void UponDead()
    {
        gameObject.SetActive(false);
    }
}