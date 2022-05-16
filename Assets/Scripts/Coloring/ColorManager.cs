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
        public ColorName name;

        public int index =>
            Mathf.RoundToInt(Mathf.Log(layer.value,
                2));
    }
    
    #region Inspector
    
    [SerializeField] private GameObject Player;

    [SerializeField] private int CurWorldColor = -1;

    [SerializeField] private SpriteRenderer Background;

    [SerializeField] private List<ColorLayer> AllLayers;

    [SerializeField] private LayerMask Neutral;

    [SerializeField] private PlayerHUD PlayerHUD;

    [SerializeField] private List<LayerMask> startWith;
    
    [SerializeField] private bool startWithAll;
    #endregion

    #region Fields

    public static int CurrLayer => _shared.AllLayers[_shared.CurWorldColor].index;

    private static ColorManager _shared;

    private int TotalColors => AllLayers.Count;

    private List<ColorLayer> availableLayers;

    private HashSet<ColorChangeListener> colorListeners = new HashSet<ColorChangeListener>();

    public static LayerMask GroundLayers
    {
        get
        {
            LayerMask m = _shared.Neutral;
            foreach (var layer in _shared.AllLayers)
            {
                if(_shared.CurWorldColor != -1 && layer.index == CurrLayer)
                    continue;
                m |= layer.layer;
            }

            return m;
        }
    }

    public static LayerMask NeutralLayer => _shared.Neutral;

    #endregion

    public enum ColorName
    {
        Neutral, Cyan, Magenta, Yellow
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        if (_shared == null)
        {
            _shared = this;
            InitAvailable();
            Background.color = Color.white;
            if (CurWorldColor != -1)
                SetWorldColor(CurWorldColor);
        }
    }

    private void InitAvailable()
    {
        if (startWithAll)
        {
            availableLayers = AllLayers;
        }
        else
        {
            availableLayers = new List<ColorLayer>();
            foreach (var l in startWith)
            {
                ColorLayer? cl = GetColorLayer(l.value);
                if(cl == null)
                    return;
                availableLayers.Add((ColorLayer) cl);
            }
        }
    }

    #region Private Methods
    
    // function disables the given layer and enables all other layers. 
    private void CancelCollisionLayer(ColorLayer layer)
    {
        foreach (var l in AllLayers)
        {
            bool ignore = layer.layer != Neutral.value && l.index == layer.index;
            Physics2D.IgnoreLayerCollision(l.index, Player.layer, ignore);
            foreach (var l2 in AllLayers)
            {
                if (l2.index != layer.index)
                {
                    Physics2D.IgnoreLayerCollision(l.index, l2.index, ignore);
                }
            }
        }
    }

    // color: the index of the color in the Colors list
    private void SetBackGroundColor(Color color)
    {
        Background.color = color;
    }

    public static void RotateColor()
    {
        _shared.CurWorldColor = (_shared.CurWorldColor + 1) % _shared.availableLayers.Count;
        _shared.SetWorldColor(_shared.CurWorldColor);
    }

    public static void AddColor(LayerMask l)
    {
        ColorLayer? cl = GetColorLayer(l.value);
        if(cl == null || _shared.availableLayers.Contains((ColorLayer) cl))
            return;
        
        _shared.availableLayers.Add((ColorLayer) cl);
//        _shared.PlayerHUD.AddColor((ColorLayer) cl);
    }

    public static ColorLayer? GetColorLayer(int layerValue)
    {
        foreach (var layer in _shared.AllLayers)
        {
            if (layer.layer.value == layerValue)
                return layer;
        }

        return null;
    }

    public static void RegisterColorListener(ColorChangeListener l)
    {
        if(!_shared.colorListeners.Contains(l))
            _shared.colorListeners.Add(l);
    }
    
    public static void UnregisterColorListener(ColorChangeListener l)
    {
        if(_shared.colorListeners.Contains(l))
            _shared.colorListeners.Remove(l);
    }
    
    public static ColorLayer? GetColorLayer(Color color)
    {
        foreach (var layer in _shared.AllLayers)
        {
            if (layer.color == color)
                return layer;
        }

        return null;
    }
    
    public static int GetLayer(Color c)
    {
        foreach (var layer in _shared.AllLayers)
        {
            if (layer.color == c)
                return layer.index;
        }

        return -1;
    }

    public static Color? GetColor(LayerMask mask)
    {
        int layerValue = mask.value;
        if (layerValue == _shared.Neutral.value)
            return Color.white;
        
        foreach (var layer in _shared.AllLayers)
        {
            if (layer.layer.value == layerValue)
                return layer.color;
        }

        return null;
    }
    
    #endregion
    
    #region Public Methods
    
    public void SetWorldColor(int color)
    {
        ColorLayer cl = availableLayers[color];
        SetBackGroundColor(cl.color);
        CancelCollisionLayer(cl);
        foreach (var l in colorListeners)
        {
            l.OnColorChange(cl);
        }
        // PlayerHUD.SetCurColorUI(color);
    }

    public static void SetColor(ColorName c)
    {
        int index = _shared.AllLayers.FindIndex(cl => cl.name == c);
        if(index == -1)
            return;
        _shared.SetBackGroundColor(_shared.AllLayers[index].color);
        _shared.CancelCollisionLayer(_shared.AllLayers[index]);
    }
    
    #endregion
    
}
