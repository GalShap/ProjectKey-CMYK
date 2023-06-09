using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    #region Inspector

    [SerializeField] private bool godMode = false;

    [SerializeField] private int lives = 6;

    [SerializeField] private float bounce = 6f;

    [SerializeField] private float timeToBounce = 0.2f;

    [SerializeField] private float timeToHit = 0.8f;

    [SerializeField] private float timeToDie = 1.2f;

    [SerializeField] private UnityEvent OnPinkDie;
    
    [SerializeField] private UnityEvent OnYellowDie;

    [SerializeField] private float bounceMult = 1;

    #endregion

    #region Private Field

    private float _time = 0;

    private Animator playerAnimator;

    private Rigidbody2D _playerRigidBody;

    private BoxCollider2D _playerCollider;

    private PlayerController _player;

    private HashSet<MovingPlatform> _platforms = new HashSet<MovingPlatform>();

    private bool _isBouncing = false;

    private bool _hit = false;

    private float _timeToNextHit = 0;

    private int _lastCollision = -1;

    private bool _fightingPink;
    private bool _fightingYellow;

    private static readonly int SpikesDeath = Animator.StringToHash("DeathBySpikes");
    private static readonly int MonsterDeath = Animator.StringToHash("DeathByMonster");
    private static readonly int Hit = Animator.StringToHash("Hit");

    private enum CollisionWith
    {
        Monster,
        Spikes
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
        _player = GetComponent<PlayerController>();
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

    public void StartFightPink()
    {
        _fightingPink = true;
    }
    
    
    public void StopFightPink()
    {
        _fightingPink = false;
    }
    
    public void StartFightYellow()
    {
        _fightingYellow = true;
    }
    
    
    public void StopFightYellow()
    {
        _fightingYellow = false;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        MovingPlatform platform = other.gameObject.GetComponent<MovingPlatform>();
        if (platform != null)
        {
            if (!_platforms.Contains(platform))
            {
                _platforms.Add(platform);
            }
        }
        
        if (_player.jumpAttacking && EnemyCollision(other.gameObject))
        {
            BlueGod god = other.collider.GetComponent<BlueGod>();
            if (god != null)
            {
                PlayerKickBack(Vector2.right);
                god.Hit();
                return;
            }

            Vector2 kickback = GetKickback(other).normalized * bounceMult;
            PlayerKickBack(kickback);
            _player.JumpHit();
            if (!other.gameObject.CompareTag("Spikes"))
            {
                EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
                enemyHealth.Hit(gameObject);
            }

            return;
        }

        if (EnemyCollision(other.gameObject) && !_hit)
        {
            _hit = true;

            EnemyObject enemy = other.gameObject.GetComponent<EnemyObject>();
            if (enemy == null)
            {
                enemy = other.gameObject.GetComponentInChildren<EnemyObject>();
            }

            int damage = 1;
            if (enemy != null)
            {
                if (!enemy.doingDamage)
                {
                    return;
                }
                if (enemy.IsOneHit)
                {
                    damage = lives;
                }
                
            }

            Damage(damage);

            if (!_isBouncing)
            {
                _isBouncing = true;
                PlayerKickBack(other.gameObject);
            }

        }
    }

    private Vector2 GetKickback(Collision2D otherContact)
    {
        var playerPos = transform.position;
        var enemyPos = otherContact.collider.gameObject.transform.position;
        var normal = otherContact.contacts[0].normal;

        var pushBack = Vector2.right * Mathf.Sign(playerPos.x-enemyPos.x);
        return (normal.y >= 0) ? normal + pushBack : pushBack;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        // it was a jump attack succka, no life lost! 
        if (_player.jumpAttacking && !_player.onGround)
        {
            return;
        }

        if (EnemyCollision(other.gameObject))
        {
            int damage = 1;
            if (other.gameObject.CompareTag("Monster"))
            {
                EnemyObject enemy = other.gameObject.GetComponent<EnemyObject>();
                if (enemy == null)
                {
                    enemy = other.gameObject.GetComponentInChildren<EnemyObject>();
                }

                if (enemy == null)
                    return;

                if (enemy.IsOneHit)
                {
                    damage = lives;
                }
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

        if (other.CompareTag("Monster"))
        {
            _lastCollision = (int) CollisionWith.Monster;
            return true;
        }

        if (other.gameObject.CompareTag("Projectile"))
        {
            bullet b = other.GetComponent<bullet>();
            if (b != null && b.IsActive)
            {
                _lastCollision = (int) CollisionWith.Monster;
                b.Unactive();
                return true;
            }
        }

        return false;
    }

    /// <summary>
    ///  called when play is colliding with an enemy. kicks the player back by the "_bounce" parameter
    /// </summary>
    /// <param name="other">
    /// the collision with an enemy. 
    /// </param>
    public void PlayerKickBack(GameObject other)
    {

        Rigidbody2D enemyRigidBody = other.GetComponent<Rigidbody2D>();

        if (enemyRigidBody == null)
            enemyRigidBody = other.GetComponentInParent<Rigidbody2D>();

        var kick = (_playerRigidBody.position - enemyRigidBody.position).normalized * bounce;
        bool isBellow = other.transform.position.y < (_player.transform.position.y - _player.height.y / 2);
        if (!_player.onGround || other.transform)
            kick *= bounceMult*bounceMult;
        PlayerController.SetKickBack(kick);

        _isBouncing = false;
    }

    public void PlayerKickBack(Vector2 dir)
    {
        PlayerController.SetKickBack((dir * bounce));

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
        if(godMode) return;
        
        PlayerHUD.SharedHud.removeLifeOnUI(amount);
        CameraManager.Manager.HitShake();
        lives -= amount;
        if (lives <= MIN_LIVES)
        {
            lives = MIN_LIVES;
            AudioManager.SharedAudioManager.PlayKeyActionSound((int) AudioManager.KeySounds.Death);
            StartCoroutine(DeathSequence());
        }
        else
        {
            AudioManager.SharedAudioManager.PlayKeyActionSound((int) AudioManager.KeySounds.Hit);
            playerAnimator.SetTrigger(Hit);
        }



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

        PlayerHUD.SharedHud.addLifeOnUI(amount);
    }

    public void SetHealth(int amount)
    {
        if (amount == lives)
            return;

        if (amount < lives)
        {
            PlayerHUD.SharedHud.removeLifeOnUI(lives - amount);
        }

        else if (amount > lives)
        {
            PlayerHUD.SharedHud.addLifeOnUI(amount - lives);
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
        foreach (var platform in _platforms)
        {
            platform.Detach(_playerRigidBody);
        }
        
        playerAnimator.SetBool("jumping",false);
        SetHealth(MAX_LIVES);
        if (_fightingPink)
            OnPinkDie.Invoke();
        if(_fightingYellow)
            OnYellowDie.Invoke();
        GameManager.Manager.Respawn();
        PlayerHUD.SharedHud.FullHealth();
        
    }

    #endregion


    private IEnumerator DeathSequence()
    {
        
        
        InputManager.Manager.DisableAll();
        _playerRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        _playerCollider.enabled = false;
        //switch (_lastCollision)
        //{
        // play spikes death animation
        // case (int) CollisionWith.Spikes:
        //  playerAnimator.SetTrigger(SpikesDeath);
        //break;


        // play monster death animation
        //case (int) CollisionWith.Monster:
        // playerAnimator.SetTrigger(MonsterDeath);
        //break;

        //}

        playerAnimator.SetTrigger(MonsterDeath);
        AudioManager.SharedAudioManager.PlayKeyActionSound((int) AudioManager.KeySounds.Death);

        yield return new WaitForSeconds(timeToDie);
        Dead();

        _playerCollider.enabled = true;
        var b = RigidbodyConstraints2D.FreezeRotation;
        _playerRigidBody.constraints = b;
        InputManager.Manager.EnableAll();
    }
}
