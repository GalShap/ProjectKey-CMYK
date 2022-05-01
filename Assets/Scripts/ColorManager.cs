using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    [Serializable]
    public struct ColorLayer
    {
        public LayerMask layer;
        public Color color;

        public int index =>
            Mathf.RoundToInt(Mathf.Log(layer.value,
                2)); //LayerMask.NameToLayer(LayerMask.LayerToName(layer.value));
    }
    
    #region Inspector
    
    [SerializeField] private GameObject Player;

    [SerializeField] private int CurWorldColor = -1;
    
    // [SerializeField] private int TotalColors = 3;

    [SerializeField] private SpriteRenderer Background;

    // [SerializeField] private List<Color> Colors = new List<Color>() {Color.red, Color.green, Color.blue};

    [SerializeField] private List<ColorLayer> layers;

    [SerializeField] private LayerMask Neutral;
    #endregion

    
    #region Constants
    // private const int FIRST_COLOR_LAYER = 7;

    #endregion

    #region Fields

    public static int CurrLayer => _shared.layers[_shared.CurWorldColor].index;

    private static ColorManager _shared;

    private int TotalColors => layers.Count;

    public static LayerMask GroundLayers
    {
        get
        {
            LayerMask m = _shared.Neutral;
            foreach (var layer in _shared.layers)
            {
                if(_shared.CurWorldColor != -1 && layer.index == CurrLayer)
                    continue;
                m |= layer.layer;
            }

            return m;
        }
    }

    #endregion

    protected enum CurColor
    {
        Default, Red, Green, Blue
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if (_shared == null)
        {
            _shared = this;
            if (CurWorldColor != -1)
                SetWorldColor(CurWorldColor);
        }
    }

    #region Private Methods
    
    // function disables the given layer and enables all other layers. 
    private void CancelCollisionLayer(int pos)
    {
        int layer = layers[pos].index;
        foreach (var l in layers)
        {
            Physics2D.IgnoreLayerCollision(l.index, Player.layer, l.index == layer);
        }
    }

    // color: the index of the color in the Colors list
    private void SetBackGroundColor(int color)
    {
        Background.color = layers[color].color;
    }

    public static void RotateColor()
    {
        _shared.CurWorldColor = (_shared.CurWorldColor + 1) % _shared.TotalColors;
        _shared.SetWorldColor(_shared.CurWorldColor);
    }
    
    #endregion
    
    #region Public Methods
    
    public void SetWorldColor(int color)
    {
        SetBackGroundColor(color);
        CancelCollisionLayer(color);
    }
    
    #endregion
    
}
