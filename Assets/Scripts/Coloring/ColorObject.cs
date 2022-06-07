using System;
using UnityEngine;

public class ColorObject : MonoBehaviour
{
    [Header("Color Object")]
    [SerializeField] protected LayerMask layer;
    [SerializeField] protected Sprite neutralSprite;
    [SerializeField] protected Sprite whiteSprite;
    [SerializeField] protected Color neutralColor = Color.white;

    protected SpriteRenderer _renderer;

    protected virtual void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        if (_renderer == null)
        {
            _renderer = GetComponentInChildren<SpriteRenderer>();
        }
        if (layer.value == 0)
            layer = ColorManager.NeutralLayer;

        SetColor();
        SetLayer();
    }

    public int LayerIndex => (int) Mathf.Log(layer, 2);

    public void ChangeColor(ColorManager.ColorName cname)
    {
        ColorManager.ColorLayer clayer = ColorManager.GetColorLayer(cname);
        layer = clayer.layer;
        SetColor();
        SetLayer();
    }

    protected void SetLayer()
    {
        gameObject.layer = (int) Mathf.Log(layer.value, 2);
    }

    protected void SetColor()
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
            _renderer.color = neutralColor;
        }
    }
}