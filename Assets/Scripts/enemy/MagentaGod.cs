using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MagentaGod : EnemyObject
{
    [SerializeField] private float timerCounter = 3;
    [SerializeField] public float speed = 3;
    [SerializeField] public float attackRange = 3;
    private float timer = 0;
    private bool[] flag = {false, false, false, false};

    [FormerlySerializedAs("m_Projectile")]
    public GameObject b_Projectile; // this is a reference to your projectile prefab

    public GameObject r_Projectile; // this is a reference to your projectile prefab

    [SerializeField]
    private Transform[] m_SpawnTransform; // this is a reference to the transform where the prefab will spawn

    [SerializeField]
    private Transform[] m_SpawnTransformBlue; // this is a reference to the transform where the prefab will spawn

    public GameObject player;
    public Transform right;
    public Transform left;
    public GameObject platformLeft;
    public GameObject platformRight;

    // Start is called before the first frame update
    private void Awake()
    {
        movement = gameObject.transform.position;
        rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        collisionOffset = Vector2.right * (_renderer.sprite.rect.width / _renderer.sprite.pixelsPerUnit) / 2;
        platformLeft.SetActive(false);
        // platformRight.SetActive(false);
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

    protected override void UponDead()
    {
        gameObject.SetActive(false);
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
        if (directionAcordingToHp(hp, 400, 0, "right", animator)) return;
        if (directionAcordingToHp(hp, 300, 1, "left", animator)) return;
        if (directionAcordingToHp(hp, 200, 2, "right", animator)) return;
        else directionAcordingToHp(hp, 100, 3, "left", animator);
    }

    private bool directionAcordingToHp(float hp, float min, int index, string dir, Animator animator)
    {
        if (hp <= min && !flag[index])
        {
            rb.AddForce(Vector2.up * 3);
            animator.SetTrigger(dir);
            flag[index] = true;
            return true;
        }

        return false;
    }
}