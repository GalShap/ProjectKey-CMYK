using System;
using UnityEngine;

public class ColorObject : MonoBehaviour
{
    [Header("Color Object")]
    [SerializeField] private LayerMask layer;
    [SerializeField] private Sprite neutralSprite;
    [SerializeField] private Sprite whiteSprite;

    private SpriteRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        if (layer.value == 0)
            layer = ColorManager.NeutralLayer;

        SetColor();
        SetLayer();
    }

    private void SetLayer()
    {
        gameObject.layer = (int) Mathf.Log(layer.value, 2);
    }

    private void SetColor()
    {
        Color? c = ColorManager.GetColor(layer.value);
        if (c == null)
            throw new Exception("ColorObject " + name + " must have a Color layer");

        if (c != Color.white)
        {
            _renderer.sprite = whiteSprite;
            _renderer.color = (Color) c;
        }
        else
        {
            _renderer.sprite = neutralSprite;
            _renderer.color = Color.white;
        }
    }
}