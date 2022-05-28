using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    #region Inspector

    [SerializeField] private int lives = 6;
    
    [SerializeField] private float bounce = 6f;
    
    [SerializeField] private float timeToBounce = 0.2f;
    
    [SerializeField] private float timeToHit = 0.8f;
    #endregion
    
    #region Private Field
    
    private float _time = 0;

    private Animator playerAnimator;
    
    private Rigidbody2D _playerRigidBody;

    private BoxCollider2D _playerCollider; 
    
    private bool _isBouncing = false;

    private bool _hit = false;

    private float _timeToNextHit = 0;

    private int _lastCollision = -1;
    
    private static readonly int SpikesDeath = Animator.StringToHash("DeathBySpikes");
    private static readonly int MonsterDeath = Animator.StringToHash("DeathByMonster");

    private enum CollisionWith
    {
        Monster , Spikes
    }
    
    #endregion
    
    #region Constants
  
    private const int MAX_LIVES = 6;

    private const int MIN_LIVES = 0;
   
    #endregion
    
    #region Mono Behaviour Funcs
    void Awake()
    {
        _playerRigidBody = GetComponent<Rigidbody2D>();
        _playerCollider = GetComponent<BoxCollider2D>();
        playerAnimator = GetComponentInChildren<Animator>();
        lives = PlayerHUD.MaxLife;
    }

    // Update is called once per frame
    void Update()
    {
        if (_hit)
        {
            _timeToNextHit += Time.deltaTime;

            if (_timeToNextHit >= timeToHit)
            {
                _hit = false;
                _timeToNextHit = 0;
            }
        }
        
        if (_isBouncing)
        {
            _time += Time.deltaTime;
            if (_time >= timeToBounce)
            {
                _isBouncing = false;
                _time = 0;
            }
            
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {   
     
        if (EnemyCollision(other.gameObject) && !_hit)
        {
            _hit = true;
            
            EnemyObject enemy = other.gameObject.GetComponent<EnemyObject>();
            if (enemy == null)
            {
                enemy = other.gameObject.GetComponentInChildren<EnemyObject>();
            }
            
            if(enemy == null)
                return;

            int damage = 1;
            if (enemy.IsOneHit)
            {
                damage = lives;
            }
            Damage(damage);
            
            if (!_isBouncing)
            {
                _isBouncing = true;
                PlayerKickBack(other.gameObject);
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (EnemyCollision(other.gameObject))
        {
            EnemyObject enemy = other.gameObject.GetComponent<EnemyObject>();
            if (enemy == null)
            {
                enemy = other.gameObject.GetComponentInChildren<EnemyObject>();
            }
            
            if(enemy == null)
                return;

            int damage = 1;
            if (enemy.IsOneHit)
            {
                damage = lives;
            }
            Damage(damage);
            if (!_isBouncing)
            {
                _isBouncing = true;
                PlayerKickBack(other.gameObject);
            }
            
        }
    }

    #endregion

    #region Private Methods    
    private bool EnemyCollision(GameObject other)
    {
        if (other.CompareTag("Spikes"))
        {
            _lastCollision = (int) CollisionWith.Spikes;
            return true;
        }
        
        if (other.CompareTag("Monster") || other.gameObject.CompareTag("Projectile"))
        {
            _lastCollision = (int) CollisionWith.Monster;
            return true;
        }

        return false;
    }
    
    /// <summary>
    ///  called when play is colliding with an enemy. kicks the player back by the "_bounce" parameter
    /// </summary>
    /// <param name="other">
    /// the collision with an enemy. 
    /// </param>
    private void PlayerKickBack(GameObject other)
    {   
        
        Rigidbody2D enemyRigidBody = other.GetComponent<Rigidbody2D>();
        
        if (enemyRigidBody == null)
            enemyRigidBody = other.GetComponentInParent<Rigidbody2D>();
        PlayerController.SetKickBack((_playerRigidBody.position - enemyRigidBody.position).normalized * bounce);
       
        _isBouncing = false;
    }
    
    #endregion
    
    #region Public Methods

    /// <summary>
    ///  inflicts damage to player;
    /// </summary>
    /// <param name="amount">
    /// how many lives to remove! not how much health.
    /// Alon: "Ma HaHevdel...?"
    /// Gal: that max life is 6 and max health on slider is 120, duh...
    /// </param>
    public void Damage(int amount)
    {
        CameraManager.Manager.ShakeCamera();
        lives -= amount;
        if (lives <= MIN_LIVES)
        {
            lives = MIN_LIVES;
            StartCoroutine(DeathSequence(3.2f));
        }

        PlayerHUD.sharedHud.removeLifeOnUI(amount);

    }
    
    /// <summary>
    ///  heals player
    /// </summary>
    /// <param name="amount">
    /// how many lives to add! not how much health.
    /// </param>
    public void Heal(int amount)
    {
        lives += amount;
        if (lives >= MAX_LIVES)
            lives = MAX_LIVES;
        
        PlayerHUD.sharedHud.addLifeOnUI(amount);
    }

    public void SetHealth(int amount)
    {   
        if (amount == lives)
            return;

        if (amount < lives)
        {
            PlayerHUD.sharedHud.removeLifeOnUI(lives - amount);
        }

        else if (amount > lives)
        {
            PlayerHUD.sharedHud.addLifeOnUI(amount - lives);
        } 
        
        lives = amount;
        if (lives > MAX_LIVES)
            lives = MAX_LIVES;
        else if (lives < MIN_LIVES)
            lives = MIN_LIVES;

        
    }

    public int GetHealth()
    {
        return lives;
    } 
    
    public void Dead()
    {
        SetHealth(MAX_LIVES);
        GameManager.Manager.Respawn();
        PlayerHUD.sharedHud.FullHealth();
    }
    
    #endregion
    
    
    private IEnumerator DeathSequence(float time)
    {   
        InputManager.Manager.DisableAll();
        _playerRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        _playerCollider.enabled = false;
        switch (_lastCollision)
        {
            // play spikes death animation
            case (int) CollisionWith.Spikes:
                playerAnimator.SetTrigger(SpikesDeath);
                break;
            
            
            // play monster death animation
            case (int) CollisionWith.Monster:
                playerAnimator.SetTrigger(MonsterDeath);
                break;
            
        }

        yield return new WaitForSeconds(time);
        
        Dead();
        _playerCollider.enabled = true;
        var b = RigidbodyConstraints2D.FreezeRotation;
        _playerRigidBody.constraints = b;
        InputManager.Manager.EnableAll();
    }   

    
    
}
