using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Whoosh: MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float lifeTime = 0.5f;

    private SpriteRenderer _renderer;
    private float mult = 5;
    private float counter;
    

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        transform.eulerAngles = new Vector3(0, 0, Random.Range(-180f, 180f));
        
        if(sprites.Length <= 0) return;
        int i = Random.Range(0, sprites.Length);
        _renderer.sprite = sprites[i];
    }

    private void Update()
    {
        transform.localScale = new Vector3(1 + counter*mult, 1 + counter*mult, 0);
        counter += Time.deltaTime;
        if (counter >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
