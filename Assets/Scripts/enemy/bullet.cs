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
    private Animator _animator;
    private Vector2 vec2;
    private bool active = true;

    /// <summary>
    /// Message that is called when the script instance is being loaded
    /// </summary>
    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        vec2 = Vector2.Lerp(transform.position, transform.right, m_Speed);
        _animator = GetComponent<Animator>();
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

    public bool IsActive => active;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Projectile") || other.gameObject.CompareTag("Monster")) return;
        _animator.SetTrigger("Death");
        m_Rigidbody.velocity = Vector2.zero;
        active = false;
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}