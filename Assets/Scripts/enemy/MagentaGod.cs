using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MagentaGod : EnemyObject
{
    [SerializeField] private float timerCounter = 3;
    [SerializeField] public float speed = 3;
    [SerializeField] public float attackRange = 3;
    private float timer = 0;
    [SerializeField] private float lifeToGoDiffMode = 200;
    [SerializeField]private bool isLeft = false;
    private bool[] flag = {false, false, false, false};

    // [FormerlySerializedAs("m_Projectile")]
    [SerializeField] public GameObject b_Projectile; // this is a reference to your projectile prefab

    [SerializeField] public GameObject r_Projectile; // this is a reference to your projectile prefab

    [SerializeField]
    private Transform[] m_SpawnTransform; // this is a reference to the transform where the prefab will spawn

    [SerializeField]
    private Transform[] m_SpawnTransformBlue; // this is a reference to the transform where the prefab will spawn

    [SerializeField] private ColorOrb orb;

    public GameObject player;
    public Transform right;
    public Transform left;
    public GameObject platformLeft;
    public GameObject platformRight;
    private Vector3 initPos;

    private bool playing;

    // Start is called before the first frame update
    private void Awake()
    {
        initPos = transform.position;
        movement = gameObject.transform.position;
        rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        
        if (_animator == null)
            _animator = GetComponent<Animator>();
        collisionOffset = Vector2.right * (_renderer.sprite.rect.width / _renderer.sprite.pixelsPerUnit) / 2;
        platformLeft.SetActive(true);
        platformRight.SetActive(true);
    }

    public void Shoot()
    {
        int i = m_SpawnTransform.Length;
        int k = Random.Range(0, i);
        var dir = player.transform.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Instantiate(r_Projectile, m_SpawnTransform[k].position, Quaternion.AngleAxis(angle, Vector3.forward));
    }

    public void ShootBlue()
    {
        int i = m_SpawnTransformBlue.Length;
        for (int k = 0; k < m_SpawnTransformBlue.Length; k++)
        {
            Instantiate(b_Projectile, m_SpawnTransformBlue[k].position, m_SpawnTransformBlue[k].rotation);
        }
    }

    public bool Playing
    {
        get => playing;
        set
        {
            playing = value;
            _animator.SetBool("play",playing);
        }
    }

    private void FixedUpdate()
    {
        if (playing)
        {
            if (KickBackVector != null)
            {
                rb.velocity = KickBackVector.Value;
                KickBackVector = null;
            }
            else
            {
                rb.velocity = Vector2.zero;
            }   
        }
    }

    public void StartFight()
    {
        Playing = true;
    }

    public void Die()
    {
        // transform.position = initPos;
        rb.bodyType = RigidbodyType2D.Dynamic;
        Playing = false;
        doingDamage = false;
        DialogueManager.Manager.LoadDialogue(DialogueManager.Dialogues.PINK_DEAD, true,
            () =>
            {
                gameObject.SetActive(false);
                orb.transform.position = transform.position;
                orb.gameObject.SetActive(true);
            });
    }
    
    protected override void UponDead()
    {
        // DialogueManager.Manager.LoadDialogue(DialogueManager.Dialogues.PINK_DEAD,true,
        //     () =>
        //     {
        //         _animator.SetTrigger("dead");
        //         playing = false;
        //         DialogueManager.Manager.LoadDialogue(DialogueManager.Dialogues.PINK_DEAD,true,(
        //             () =>
        //             {
        //                 gameObject.SetActive(false);
        //             }));
        //     });
    }

    public bool isFlipped = false;

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > player.transform.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < player.transform.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    public void healtChange()
    {
        var animator = gameObject.GetComponent<Animator>();
        var hl = animator.GetComponent<EnemyHealth>();
        var hp = hl.GetHealth();
        if (hp <= lifeToGoDiffMode)
        {
            // rb.transform.position = new Vector3(rb.transform.position.x,platformLeft.transform.position.y +1, rb.transform.position.z) ;
        }
        // if (directionAcordingToHp(hp, 400, 0, "left", animator)) return;
        // if (directionAcordingToHp(hp, 300, 1, "right", animator)) return;
        // if (directionAcordingToHp(hp, 200, 2, "left", animator)) return;
        // else directionAcordingToHp(hp, 100, 3, "right", animator);
    }

    private bool directionAcordingToHp(float hp, float min, int index, string dir, Animator animator)
    {
        if (hp <= min && !flag[index])
        {
            // rb.AddForce(Vector2.up * 3);
            animator.SetTrigger(dir);
            flag[index] = true;
            return true;
        }

        return false;
    }

    public void resetBoss()
    {
        var animator = gameObject.GetComponent<Animator>();
        var hl = animator.GetComponent<EnemyHealth>();
        hl.SetHealth(hl.MAX_HEALTH);
        print($"health: {hl.GetHealth()}");
        animator.SetTrigger("coolDown");
        transform.position = initPos;
        Playing = false;
    }

    public GameObject getPlayer()
    {
        return player;
    }

    public void Move()
    {
        var animator = gameObject.GetComponent<Animator>();
        animator.SetTrigger(isLeft ? "left" : "right");
        isLeft = isLeft != true;
    }

}