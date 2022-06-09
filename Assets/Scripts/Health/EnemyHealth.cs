using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    #region Inspector

    [SerializeField] private int health = 100;
    
    [SerializeField] private SpriteRenderer enemySpriteRenderer;

    [Tooltip("How much time the animation takes for every blip?")] 
    [SerializeField] private float timeToAnimate = 0.05f;

    [SerializeField] private float bounce = 50f;

    [SerializeField] private float _timeToBounce = 0.2f;
    
    [SerializeField] public int MAX_HEALTH = 100;

    [SerializeField] private Rigidbody2D _rigidbody2D;

    [SerializeField] private UnityEvent onDeath;
    #endregion

    public bool damagable = true;
    
    private Rigidbody2D _enemyRigidBody;

    private Animator _animator;

    private float _time = 0f;

    private bool _isBouncing = false;

    private EnemyObject _enemyObject;
    
    private static readonly int Death = Animator.StringToHash("Death");

    #region Constants
    
    // private const int MAX_HEALTH = 100;

    private const int MIN_HEALTH = 0;

    private const int MAX_ALPHA = 1;

    private const int MIN_ALPHA = 0;
    
    #endregion

    protected virtual void Start()
    {
        _enemyRigidBody = gameObject.GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        if (_animator == null)
            _animator = GetComponent<Animator>();
        
        _enemyObject = GetComponentInChildren<EnemyObject>();
        if (_enemyObject == null)
            _enemyObject = GetComponent<EnemyObject>();
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

    public bool IsAlive => health > MIN_HEALTH;

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
      
                alpha.a =  Mathf.Lerp(MAX_ALPHA, MIN_ALPHA, (elapsedTimeMin / timeToAnimate));
                enemySpriteRenderer.color = alpha;
                elapsedTimeMin += Time.deltaTime;
                yield return null;
            }

            alpha.a = MIN_ALPHA;
            
            while (elapsedTimeMax < timeToAnimate)
            {   
         
                alpha.a =  Mathf.Lerp(MIN_ALPHA, MAX_ALPHA, (elapsedTimeMax / timeToAnimate));
                enemySpriteRenderer.color = alpha;
                elapsedTimeMax += Time.deltaTime;
                yield return null;
            }

            alpha.a = MAX_ALPHA;
        }
    }
    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //
    //     if (other.gameObject.CompareTag("Player")) {
    //         PlayerController p = other.gameObject.GetComponent<PlayerController>();
    //         if (p == null) return;
    //         if (!p.onGround && p.jumpAttacking)
    //         {
    //             Hit(other.gameObject);
    //             other.gameObject.GetComponent<PlayerHealth>().PlayerKickBack(this.gameObject);   
    //         }
    //     }
    // }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Spikes"))
        {
            Damage(MAX_HEALTH);
        }
    }

    public virtual void Hit(GameObject hitter)
    {
        print("hit");
        Damage(50);
            
        if (!_isBouncing)
        {   
            _isBouncing = true;
            EnemyKickBack(hitter);
        }
    }

    private void EnemyKickBack(GameObject other)
    {
        Rigidbody2D playerRigidBody = other.GetComponent<Rigidbody2D>();
        if (playerRigidBody == null)
            playerRigidBody = other.GetComponentInParent<Rigidbody2D>();

        var newKick = Vector2.right;
        if (playerRigidBody.position.x < _enemyRigidBody.position.x)
        {
            newKick *= bounce;
        }
        else
        {
            newKick *= -bounce;
        }
        // var newKick = (_enemyRigidBody.position - playerRigidBody.position).normalized * bounce;
        EnemyObject enemy = GetComponent<EnemyObject>();
        enemy.SetKickBack(newKick);
        
        
        _isBouncing = false;
    }


    public void Damage(int amount)
    {
        if(!damagable) return;
        health -= amount;
        if (health <= MIN_HEALTH)
        {
            health = MIN_HEALTH;
            Dead();
        }

        else
        {
            StartCoroutine(DamageFlashAnimation(1));
            AudioManager.SharedAudioManager.PlayEnemySounds((int) AudioManager.EnemySounds.Hit);
        }
    }

    public void Heal(int amount)
    {
        health += amount;
        if (health > MAX_HEALTH)
            health = MAX_HEALTH;
    }

    public void SetHealth(int amount)
    {
        health = amount;
        if (health > MAX_HEALTH)
            health = MAX_HEALTH;
        
        else if (health < MIN_HEALTH)
            health = MIN_HEALTH;
    }

    public int GetHealth()
    {
        return health;
    }

    public virtual void Dead()
    {
        if (_enemyObject != null)
        {
            _enemyObject.doingDamage = false;
        }
        damagable = false;
        if (_rigidbody2D != null)
        {
            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;   
        }
        if (_animator == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            _animator.SetTrigger(Death);
        }
        

        onDeath.Invoke();
        AudioManager.SharedAudioManager.PlayEnemySounds((int) AudioManager.EnemySounds.Death);
    }
}
