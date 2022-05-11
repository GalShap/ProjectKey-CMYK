using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorOrb : MonoBehaviour
{
    [SerializeField] private LayerMask layer;
    
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ColorManager.AddColor(_renderer.color, layer);
        Destroy(gameObject);
    }
}
