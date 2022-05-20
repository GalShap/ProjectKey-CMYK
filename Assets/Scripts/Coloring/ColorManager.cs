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

    [SerializeField] private GameObject BackgroundMachine;
    
    [SerializeField] private SpriteRenderer Background;

    [SerializeField] private List<ColorLayer> AllLayers;

    [SerializeField] private LayerMask Neutral;

    [SerializeField] private List<LayerMask> startWith;

    [SerializeField] private List<GameObject> Backgrounds;
    
    [SerializeField] private bool startWithAll;
    #endregion

    #region Fields

    public static int CurrLayer => _shared.AllLayers[_shared.CurWorldColor].index;

    public static ColorManager _shared;
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
                if(_shared.CurWorldColor != -1 && layer.index == CurrLayer)
                    continue;
                m |= layer.layer;
            }

            return m;
        }
    }

    public static LayerMask NeutralLayer => _shared.Neutral;

    
    public enum ColorName
    {
        Neutral, Cyan, Magenta, Yellow        
    }
    
    #endregion
    
    #region Constants

    private const int NoColors = 1;
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
            if (CurWorldColor != -1)
                SetWorldColor(CurWorldColor);
        }
        
        DeactivateBackgrounds();
        _afterAwake = true;

       
        
    }

    private void Start()
    {
        int numOfColors = availableLayers.Count;
        
       
        if (numOfColors != NoColors)
        {   
            
            PlayerHUD.sharedHud.SetColorPallete(numOfColors - 2);
        }
        
        PlayerHUD.sharedHud.HighlightColor();
                
       
    }


    #region Private Methods
    
    /// <summary>
    ///  called on awake, deactivates background that represents unavailable color.
    /// </summary>
    private void DeactivateBackgrounds()
    {
        int count = availableLayers.Count;
        for (int i = count; i < Backgrounds.Count; i++)
        {
            Backgrounds[i].SetActive(false);
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

    private (float, float, float) CalcRotations(int count)
    {
         int world = CurWorldColor - 1;
        Debug.Log("world color: " + world);
       
        switch (count)
        {
        
            // neutral + cyan
            case 2:
                
                //  neutral -> cyan
                if (world == (int) ColorName.Neutral)
                    return (NeutralRot, CyanRot, FullTime);
                
                // cyan -> neutral
                else if (world == (int) ColorName.Cyan || world == -1) 
                    return (CyanRot, NeutralRot , HalfTime);
                
                break;
            
            // neutral + cyan + magenta
            case 3:
                //  neutral -> cyan
                if (world == (int) ColorName.Neutral)
                    return (NeutralRot, CyanRot, FullTime);
                
                // cyan -> yellow
                else if (world == (int) ColorName.Cyan)
                    return (CyanRot, MagentaRot, FullTime);
                
                // magenta -> neutral
                else if (world == (int) ColorName.Magenta || world == -1)
                    return (MagentaRot, NeutralRot, HalfTime);

                break;

            // all colors
            case 4:

                //  neutral -> cyan
                if (world == (int) ColorName.Neutral)
                    return (NeutralRot, CyanRot, FullTime);
                
                // cyan -> magenta
                else if (world == (int) ColorName.Cyan)
                    return (CyanRot, MagentaRot, FullTime);
                
                //  magenta -> yellow
                else if (world == (int) ColorName.Magenta)
                    return (MagentaRot, YellowRot ,FullTime);
                
                // magenta -> neutral
                else if (world == (int) ColorName.Yellow || world == -1)
                    return (YellowRot, NeutralRot, HalfTime);
                
                break;
        }

        return (NeutralRot, NeutralRot, FullTime);
    }
    
    private void ChangeBackGround()
    {   
        if (availableLayers.Count == NoColors) return;
        
        (float, float, float) rotations = CalcRotations(availableLayers.Count);
        float start = rotations.Item1;
        float end = rotations.Item2;
        float time = rotations.Item3;
        StartCoroutine(RotateBackground(start, end, time));
        PlayerHUD.sharedHud.HighlightColor();
        // todo add audio here!!!

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
        _shared.Backgrounds[_shared.availableLayers.Count - 1].SetActive(true);
        PlayerHUD.sharedHud.SetColorPallete(_shared.availableLayers.Count);

        
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
        
 
        CancelCollisionLayer(cl);
        foreach (var l in colorListeners)
        {
            l.OnColorChange(cl);
        }
        
        if (_afterAwake) ChangeBackGround();
        
      
        //PlayerHUD.sharedHud.HighlightColor();
        //SetBackGroundColor(cl.color);
        // PlayerHUD.SetCurColorUI(color);
     
        
    }

    public static void SetColor(ColorName c)
    {
        int index = _shared.AllLayers.FindIndex(cl => cl.name == c);
        if(index == -1)
            return;
        //_shared.SetBackGroundColor(_shared.AllLayers[index].color);
        _shared.ChangeBackGround();
        _shared.CancelCollisionLayer(_shared.AllLayers[index]);
    }
    
    #endregion
    
    #region Corutines

    IEnumerator RotateBackground(float start, float end, float time)
    {
        Debug.Log("start: " + start + " end: " + end);
        float cyclicEnd = (end == 0) ? 360 : end;
        float elapsedTime = 0;
        float duration = time;
        float x = BackgroundMachine.transform.rotation.eulerAngles.x;
        float y = BackgroundMachine.transform.rotation.eulerAngles.y;
        float z;
        Vector3 rotation = new Vector3(0,0,0);
        
        //Debug.Log("start: " + start + " end: " + end);

        while (elapsedTime < duration)
        {
            z = Mathf.Lerp(start, cyclicEnd, elapsedTime / duration);
            rotation = new Vector3(x, y, z);
            BackgroundMachine.transform.rotation = Quaternion.Euler(rotation);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rotation.z = cyclicEnd;
        BackgroundMachine.transform.rotation = Quaternion.Euler(rotation);
        
        
    }

    IEnumerator LateAwake()
    {
        yield return new WaitForSeconds(1f);
    }
    
    #endregion
    
}
