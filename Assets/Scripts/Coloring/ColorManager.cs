using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

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

    [SerializeField] private List<LayerMask> startWith;

    // [SerializeField] private BackgroundMachine bgMachine;
    
    [SerializeField] private bool startWithAll;
    #endregion

    #region Fields

    public static int CurrLayer => _shared.AllLayers[_shared.CurWorldColor].index;

    private static ColorManager _shared;
    private int TotalColors => AllLayers.Count;

    private List<ColorLayer> availableLayers;

    private HashSet<ColorChangeListener> colorListeners = new HashSet<ColorChangeListener>();

    private bool _afterAwake = false;
    
    public static LayerMask GroundLayers
    {
        get
        {
            LayerMask m = _shared.Neutral;
            foreach (var layer in _shared.AllLayers)
            {
                if (_shared.CurWorldColor != -1 && layer.index == CurrLayer)
                {
                    continue;
                }
                m |= layer.layer;
            }

            return m | LayerMask.GetMask("Default");
        }
    }

    public static LayerMask NeutralLayer => _shared.Neutral;

    
    public enum ColorName
    {
        Neutral, Cyan, Magenta, Yellow        
    }
    
    #endregion
    
    #region Constants

    private const int NoColors = 0;
    private const float NeutralRot = 0f;
    private const float CyanRot = 90f;
    private const float MagentaRot = 180f;
    private const float YellowRot = 270f;
    private const float FullTime = 1f;
    private const float HalfTime = 0.5f;

    #endregion
    
    void Awake()
    {  
        if (_shared == null)
        {
            _shared = this;
            InitAvailable();
            Background.color = Color.white;
        }
        
        // DeactivateBackgrounds();
        // _afterAwake = true;
    }

    private void Start()
    {
        if (CurWorldColor != -1)
            SetWorldColor(CurWorldColor);
        
        int numOfColors = availableLayers.Count;
        
       
        if (numOfColors != NoColors)
        {
            PlayerHUD.SharedHud.SetColorPallete(numOfColors - 1);
        }
        
        PlayerHUD.SharedHud.HighlightColor();
    }


    #region Private Methods

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

    public static void RotateColor(int dir)
    {
        if(Math.Abs(dir) != 1) return;

        if (dir == -1 && _shared.CurWorldColor == 0)
        {
            _shared.CurWorldColor = _shared.availableLayers.Count - 1;
        }
        else
        {
            _shared.CurWorldColor = (_shared.CurWorldColor + dir) % _shared.availableLayers.Count;   
        }
        _shared.SetWorldColor(_shared.CurWorldColor);
    }

    public static void AddColor(LayerMask l)
    {
        ColorLayer? cl = GetColorLayer(l.value);
        if(cl == null || _shared.availableLayers.Contains((ColorLayer) cl))
            return;

        _shared.availableLayers.Add((ColorLayer) cl);
        PlayerHUD.SharedHud.SetColorPallete(_shared.availableLayers.Count-1);

        
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
    
    public static ColorLayer GetColorLayer(ColorName name)
    {
        foreach (var layer in _shared.AllLayers)
        {
            if (layer.name == name)
                return layer;
        }

        return _shared.availableLayers[0];
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
        PlayerHUD.SharedHud.HighlightColor();
        SetColor(cl.name);
    }

    public static void SetColor(ColorName c)
    {
        int index = _shared.AllLayers.FindIndex(cl => cl.name == c);
        if(index == -1)
            return;
        _shared.CurWorldColor = index;
        ColorLayer cl = _shared.AllLayers[index];
        _shared.CancelCollisionLayer(_shared.AllLayers[index]);
        foreach (var l in _shared.colorListeners)
        {
            l.OnColorChange(cl);
        }

        _shared.Background.color = cl.color;
        // _shared.bgMachine.Rotate(cl.color, () =>
        // {
        //     _shared.CancelCollisionLayer(_shared.AllLayers[index]);
        //     foreach (var l in _shared.colorListeners)
        //     {
        //         l.OnColorChange(cl);
        //     }
        // });
    }

    #endregion
    
    #region Corutines

    IEnumerator LateAwake()
    {
        yield return new WaitForSeconds(1f);
    }
    
    #endregion

    public static void RotateColorRight()
    {
        RotateColor(-1);
    }
    
    public static void RotateColorLeft()
    {
        RotateColor(1);
    }
}
