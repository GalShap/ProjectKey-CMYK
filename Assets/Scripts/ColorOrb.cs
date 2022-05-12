using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorOrb : MonoBehaviour
{
    [SerializeField] private LayerMask layer;
    
    private SpriteRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        Init();
    }

    private void Init()
    {
        ColorManager.ColorLayer? cl = ColorManager.GetColorLayer(layer.value);
        if (cl == null)
            return;
        _renderer.color = (Color) cl?.color;
        gameObject.layer = (int) cl?.index;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ColorManager.AddColor(layer);
        Destroy(gameObject);
    }
}
