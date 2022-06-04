using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class PlayerHUD : MonoBehaviour
{    
    
    [Serializable]
    private class ColorPallete
    {
        [SerializeField] private GameObject pallete;
        [SerializeField] private List<Image> colorsInPallete;

        private void highlightColors(int active)
        {
            for (int i = 0; i < colorsInPallete.Count; i++)
            {
                var color = colorsInPallete[i].color;

                if (i == active)
                {
                    color.a = 1;
                }
                else
                {
                    color.a = 0.3f;
                }

                colorsInPallete[i].color = color;

            }
        }
         public List<Image> get()
         {
             return colorsInPallete;
         }

         public void toggleColors(bool toggle, int index)
         {
             pallete.SetActive(toggle);
             if (toggle) highlightColors(index);
         }

         public void fadeColor(int index)
         {
             var color = colorsInPallete[index].color;
             color.a = 0.6f;

             colorsInPallete[index].color = color;
         }
         
         
     }

    [SerializeField] private Slider lifeFill;

    [SerializeField] private List<ColorPallete> levelColors = new List<ColorPallete>();
   
    private float _timeToScaleLife = 0.3f;

    private float _timeToScaleColor = 0.2f;
    
    private int _currLife = MaxLife;

    private bool switching = false;
    
    private static int _currColorPallete = 0;

    private int _currColor;
    
    private Dictionary<int, Tuple<int, int>> _lifeFillVal = new Dictionary<int, Tuple<int, int>>()
    {
        {1, new Tuple<int, int>(0, 25)},
        {2, new Tuple<int, int>(25, 42)},
        {3, new Tuple<int, int>(42, 60)},
        {4, new Tuple<int, int>(60, 80)},
        {5, new Tuple<int, int>(80, 98)},
        {6, new Tuple<int, int>(98, 120)}
    };

    private Dictionary<int, int> _layerToColor = new Dictionary<int, int>()
    {
        {10, 0}, {9, 1}, {8,2}, {7, 3}
    };

    private enum Colors
    {
        Neutral = 10, Cyan = 9, Magenta = 8, Yellow = 7
    }

    private enum Level
    {
        One = 1, Two = 2, Three = 3
    }

    public static PlayerHUD sharedHud;
    
    #region Constants

    public const int MaxLife = 6;

    private const int MaxLifeValue = 120;

    private const int MaxAlpha = 255;

    private const int MinAlpha = 100;
    
    #endregion

    void Awake()
    {
        lifeFill.value = MaxLifeValue;
        if (sharedHud == null)
        {
             sharedHud = this;
        }
    }

    private void Start()
    {
        sharedHud._currColor = ColorManager.CurrLayer;
        sharedHud.levelColors = levelColors;
        sharedHud.lifeFill = lifeFill;
    }

    /// <summary>
    /// method is called when player has acquired a new color. gets the number of level(which is basically colors
    /// available) and sets it's wheel color active. it then gets the current color and plays a coroutine that highlights
    /// this color. 
    /// </summary>
    /// <param name="level">
    /// the new level
    /// </param>
    public void SetColorPallete(int level)
    {   
        
        for (int i = 0; i < levelColors.Count; i++)
        {
            if (i == level) levelColors[i].toggleColors(true, _currColor);
            else levelColors[i].toggleColors(false, _currColor);
        }

        _currColorPallete = level;
    }

    
    /// <summary>
    ///  method gets the current color and highlights it on the hud. 
    /// </summary>
    public void HighlightColor()
    {   
       
        int newColor = ColorManager.CurrLayer;

        int indexToHighlight = sharedHud._layerToColor[newColor];
        
        Debug.Log(indexToHighlight);
      
        StartCoroutine(Highlight(indexToHighlight, _currColorPallete));

      
        
    }

    

    /// <summary>
    ///  coroutine gets a level and current color and highlights the color while reducing the alpha and scale for all
    ///  other colors.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Highlight(int newColor, int level)
    {
        List<Image> colors = levelColors[level].get();
            float time = 0;
            int oldColor = -1;

            var oldOne = new Color();
            var newOne = new Color();
            for (int i = 0; i < colors.Count; i++)
            {
                if (i == newColor) newOne = colors[i].color;

                else if ((math.abs(colors[i].color.a - 1) < (1e-9)))
                {
                    oldColor = i;
                    oldOne = colors[i].color;
                }

            }

            while (time < _timeToScaleColor)
            {

                oldOne.a = Mathf.Lerp(1, 0.6f, time / _timeToScaleColor);
                newOne.a = Mathf.Lerp(0.6f, 1, time / _timeToScaleColor);
                colors[newColor].color = newOne;
                if (oldColor >= 0)
                {
                    colors[oldColor].color = oldOne;
                }

                time += Time.deltaTime;
                yield return null;
            }

            oldOne.a = 0.6f;
            newOne.a = 1;
            colors[newColor].color = newOne;
            if (oldColor >= 0) colors[oldColor].color = oldOne;

            for (int i = 0; i < colors.Count; i++)
            {
                if (i != newColor && i != oldColor)
                {   
                    
                    var color = colors[i].color;
                    color.a = 0.6f;
                    colors[i].color = color;
                }
            }

    }

    public void removeLifeOnUI(int livesToRemove)
    {   
     
        int lives = livesToRemove;
        if (lives > _currLife)
            lives = _currLife;
        
       
        StartCoroutine(ReduceLifeSlider(_currLife, lives));
        _currLife -= lives;
    }

    public void addLifeOnUI(int livesToAdd)
    {   
        
        int lives = livesToAdd;
        if (_currLife == MaxLife)
            return;

        if (lives + _currLife > MaxLife)
            lives = MaxLife - _currLife;
        
      
        StartCoroutine(FillLifeSlider(_currLife + 1 , lives));
        _currLife += lives;
    }

   
    private IEnumerator FillLifeSlider(int barKey, int livesToAdd)
    {
        int key = barKey;
        for (int i = 0; i < livesToAdd; ++i)
        {
            int startVal = _lifeFillVal[key].Item1;
            int endVal =  _lifeFillVal[key].Item2;
            float elapsedTime = 0;
            while (elapsedTime < _timeToScaleLife)
            {
                lifeFill.value = Mathf.Lerp(startVal, endVal, (elapsedTime / _timeToScaleLife));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            lifeFill.value = endVal;
            key += 1;
        }
        
    }

    private IEnumerator ReduceLifeSlider(int bar, int livesToRemove)
    {
        if (livesToRemove >= _currLife)
        {
            lifeFill.value = 0;
            
        }
        else
        {

            int key = bar;
            for (int i = 0; i < livesToRemove; ++i)
            {
                int startVal = _lifeFillVal[key].Item2;
                int endVal = _lifeFillVal[key].Item1;
                float elapsedTime = 0;
                lifeFill.value = startVal;
                while (elapsedTime < _timeToScaleLife)
                {
                    lifeFill.value = Mathf.Lerp(startVal, endVal, (elapsedTime / _timeToScaleLife));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                lifeFill.value = endVal;
                key -= 1;
            }
        }

    }

    public void FullHealth()
    {
        lifeFill.value = MaxLifeValue;
        _currLife = MaxLife;
    }

    public void AddColor(ColorManager.ColorLayer cl)
    {
        return;
    }
}
