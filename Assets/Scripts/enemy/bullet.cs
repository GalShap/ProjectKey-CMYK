using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class bullet : MonoBehaviour
{
    /// <summary>
    /// The class definition for a projectile
    /// </summary>
    /// <summary>
    /// Public fields
    /// </summary>
    public float m_Speed = 10f; // this is the projectile's speed

    public float m_Lifespan = 3f; // this is the projectile's lifespan (in seconds)

    /// <summary>
    /// Private fields
    /// </summary>
    private Rigidbody2D m_Rigidbody;

    // [SerializeField]private Vector2 vec2;
    private Vector2 vec2;

    /// <summary>
    /// Message that is called when the script instance is being loaded
    /// </summary>
    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        vec2 = Vector2.Lerp(transform.position, transform.right, m_Speed);
        
    }

    /// <summary>
    /// Message that is called before the first frame update
    /// </summary>
    void Start()
    {
        vec2 = Vector2.Lerp(transform.position, transform.right, m_Speed);
        m_Rigidbody.AddForce(vec2 * m_Speed, ForceMode2D.Impulse);
        Destroy(gameObject, m_Lifespan);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Projectile") || other.gameObject.CompareTag("Monster")) return;
        Destroy(gameObject);
    }
}