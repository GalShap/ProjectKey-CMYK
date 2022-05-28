using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    #region Inspector

    [SerializeField] private int health = 100;
    
    [SerializeField] private SpriteRenderer enemySpriteRenderer;

    [SerializeField] [CanBeNull] private GameObject lifeBar;

    [Tooltip("How much time the animation takes for every blip?")] 
    [SerializeField] private float timeToAnimate = 0.1f;

    [SerializeField] private float bounce = 100f;

    [SerializeField] private float _timeToBounce = 0.2f;

    [SerializeField] private float _timeToFillLife = 0.2f;
    
    #endregion

    private Rigidbody2D _enemyRigidBody;

    private float _time = 0f;

    private bool _isBouncing = false;

    [CanBeNull] private Slider _lifeFill;

    #region Constants
    
    private const int MaxHealth = 100;

    private const int MinHealth = 0;

    private const int MaxAlpha = 1;

    private const int MinAlpha = 0;
    
    #endregion
    
    
    private void Start()
    {
        _enemyRigidBody = gameObject.GetComponent<Rigidbody2D>();
        if (lifeBar != null)
        {
            _lifeFill = lifeBar.GetComponent<Slider>();
            if (_lifeFill != null) _lifeFill.maxValue = health;
           
        }
    }

    private void Update()
    {
        if (_isBouncing)
        {
            _time += Time.deltaTime;
            if (_time >= _timeToBounce)
            {
                _isBouncing = false;
                _time = 0;
            }
            
        }
    }

    private IEnumerator EnemyLife(int start, int end)
    {
        float time = 0f;

        while (time <= _timeToFillLife)
        {
             _lifeFill.value = Mathf.Lerp(start, end, time / _timeToFillLife);
            yield return null;
        }

        _lifeFill.value = end;
    }

    private IEnumerator DamageFlashAnimation(int count)
    {   
        
        float elapsedTimeMax = 0;
        float elapsedTimeMin = 0;
        Color alpha = enemySpriteRenderer.color;
        for (int i = 0; i < count; i++)
        {
            elapsedTimeMax = 0;
            elapsedTimeMin = 0;
            
            while (elapsedTimeMin < timeToAnimate)
            {
                alpha.a =  Mathf.Lerp(MaxAlpha, MinAlpha, (elapsedTimeMin / timeToAnimate));
                enemySpriteRenderer.color = alpha;
                elapsedTimeMin += Time.deltaTime;
                yield return null;
            }

            alpha.a = MinAlpha;
            
            while (elapsedTimeMax < timeToAnimate)
            {   
         
                alpha.a =  Mathf.Lerp(MinAlpha, MaxAlpha, (elapsedTimeMax / timeToAnimate));
                enemySpriteRenderer.color = alpha;
                elapsedTimeMax += Time.deltaTime;
                yield return null;
            }

            alpha.a = MaxAlpha;
        }
        
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("AttackCollider"))
        {
            Damage(50);
           
            
            if (!_isBouncing)
            {
                _isBouncing = true;
                EnemyKickBack(other);
            }
        }
    }

    private void EnemyKickBack(Collider2D other)
    {
        Rigidbody2D _playerRigidBody = other.gameObject.GetComponent<Rigidbody2D>();
        if (_playerRigidBody == null)
            _playerRigidBody = other.gameObject.GetComponentInParent<Rigidbody2D>();
        
       
        _playerRigidBody.AddForce((_enemyRigidBody.position - _playerRigidBody.position).normalized * bounce,
            ForceMode2D.Impulse);
        _isBouncing = false;
    }


    public void Damage(int amount)
    {
        StartCoroutine(DamageFlashAnimation(1));
        int curHealth = health;
        int newHealth = health - amount;
        
        if (lifeBar != null) StartCoroutine(EnemyLife(curHealth, newHealth));
        health = newHealth;
        if (health < MinHealth)
        {
            health = MinHealth;
            Dead();
        }
        
        
    }

    public void Heal(int amount)
    {
        health += amount;
        if (health > MaxHealth)
            health = MaxHealth;
    }

    public void SetHealth(int amount)
    {
        health = amount;
        if (health > MaxHealth)
            health = MaxHealth;
        
        else if (health < MinHealth)
            health = MinHealth;
    }

    public int GetHealth()
    {
        return health;
    }

    public void Dead()
    {
        gameObject.SetActive(false);
    }

    public void InitLifeBar()
    {
        lifeBar.SetActive(true);
        StartCoroutine(EnemyLife(0, health));
    }
}
