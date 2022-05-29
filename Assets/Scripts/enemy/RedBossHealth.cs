using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RedBossHealth : MonoBehaviour, IDamageable
{
    #region Inspector

    [SerializeField] private int health = 100;
    
    [SerializeField] private SpriteRenderer bossSpriteRenderer;
    private Animator BossAnimator;

    [Tooltip("How much time the animation takes for every blip?")] 
    [SerializeField] private float timeToAnimate = 0.05f;

    [SerializeField] private float bounce = 50f;

    [SerializeField] private float _timeToBounce = 0.2f;
    
    #endregion
    
    
    private Rigidbody2D _bossRigidBody;

    private float _time = 0f;

    private bool _isBouncing = false;

    #region Constants
    
    private const int MAX_HEALTH = 100;

    private const int MIN_HEALTH = 0;

    private const int MAX_ALPHA = 1;

    private const int MIN_ALPHA = 0;
    
    #endregion

    private void Start()
    {
        _bossRigidBody = gameObject.GetComponent<Rigidbody2D>();
        BossAnimator = gameObject.GetComponent<Animator>();
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

    private IEnumerator DamageFlashAnimation(int count)
    {   
        
        float elapsedTimeMax = 0;
        float elapsedTimeMin = 0;
        Color alpha = bossSpriteRenderer.color;
        for (int i = 0; i < count; i++)
        {
            elapsedTimeMax = 0;
            elapsedTimeMin = 0;
            
            while (elapsedTimeMin < timeToAnimate)
            {   
      
                alpha.a =  Mathf.Lerp(MAX_ALPHA, MIN_ALPHA, (elapsedTimeMin / timeToAnimate));
                bossSpriteRenderer.color = alpha;
                elapsedTimeMin += Time.deltaTime;
                yield return null;
            }

            alpha.a = MIN_ALPHA;
            
            while (elapsedTimeMax < timeToAnimate)
            {   
         
                alpha.a =  Mathf.Lerp(MIN_ALPHA, MAX_ALPHA, (elapsedTimeMax / timeToAnimate));
                bossSpriteRenderer.color = alpha;
                elapsedTimeMax += Time.deltaTime;
                yield return null;
            }

            alpha.a = MAX_ALPHA;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {   
        Debug.Log("triggering!");
        
        if (other.gameObject.CompareTag("Player") && !PlayerController.onGround && PlayerController.attacking) {   
         
            Hit(other.gameObject);
            other.gameObject.GetComponent<PlayerHealth>().PlayerKickBack(this.gameObject);
        }
    }

    public void Hit(GameObject hitter)
    {
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

        var newKick = (_bossRigidBody.position - playerRigidBody.position).normalized * bounce;
        EnemyObject enemy = GetComponent<EnemyObject>();
        enemy.SetKickBack(newKick);
        
        
        _isBouncing = false;
    }


    public void Damage(int amount)
    {
        StartCoroutine(DamageFlashAnimation(1));
        health -= amount;
        if (health < MIN_HEALTH)
        {
            health = MIN_HEALTH;
            Dead();
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

    public void Dead()
    {
        BossAnimator.SetTrigger("dead");
        // gameObject.SetActive(false);
    }
}
