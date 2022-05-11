using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonScript : MonoBehaviour
{
    /// <summary>
    /// The class definition for a projectile's trigger
    /// </summary>
    /// <remarks>
    /// Attach this script as a component to any object capable of triggering projectiles
    /// The spawn transform should represent the position where the projectile is to appear, i.e. gun barrel end
    /// </remarks>
    /// <summary>
    /// Public fields
    /// </summary>
    public GameObject m_Projectile; // this is a reference to your projectile prefab

    public Transform m_SpawnTransform; // this is a reference to the transform where the prefab will spawn
    public GameObject player;
    [SerializeField] private float len = 4f;
    private int counter = 10;
    /// <summary>
    /// Message that is called once per frame
    /// </summary>
    private void FixedUpdate()
    {
        // Shoot();
        counter--;
        if (counter > 0) return;
        Shoot();
        counter = 10;
    }

    protected void Shoot()
    {
        Instantiate(m_Projectile, m_SpawnTransform.position, m_SpawnTransform.rotation);
    }
    
    private float PositionX()
    {
        return (transform.position.x - player.transform.position.x);
    }
}