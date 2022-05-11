using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private Slider lifeFill;

    [SerializeField] private List<Image> colorRenderers;

    private float _timeToScaleLife = 0.3f;

    private float _timeToScaleColor = 0.5f;
    
    private int _currLife = MAX_LIFE;

    private int _curActiveColor = (int) Colors.Neutral;

    private Dictionary<int, Tuple<int, int>> _lifeFillVal = new Dictionary<int, Tuple<int, int>>()
    {
        {1, new Tuple<int, int>(0, 21)},
        {2, new Tuple<int, int>(21, 40)},
        {3, new Tuple<int, int>(40, 59)},
        {4, new Tuple<int, int>(59, 77)},
        {5, new Tuple<int, int>(77, 95)},
        {6, new Tuple<int, int>(95, 120)}
    };

    private enum Colors
    {
        Magneta, Yellow, Cyan, Neutral
    }

    #region Constants

    public const int MAX_LIFE = 6;

    private const int MAX_LIFE_VALUE = 120;

    private const int MAX_ALPHA = 255;

    private const int MIN_ALPHA = 100;
    
    #endregion

    void Start()
    {
        lifeFill.value = MAX_LIFE_VALUE;
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
        if (_currLife == MAX_LIFE)
            return;

        if (lives + _currLife > MAX_LIFE)
            lives = MAX_LIFE - _currLife;
        
      
        StartCoroutine(FillLifeSlider(_currLife + 1 , lives));
        _currLife += lives;
    }

    public void SetCurColorUI(int newColor)
    {
        StartCoroutine(ChangeColor(_curActiveColor, newColor));
        _curActiveColor = newColor;
    }

    private IEnumerator ChangeColor(int curColor, int newColor)
    { 
        var curColorVec = colorRenderers[newColor].color;
        var newColorVec = colorRenderers[curColor].color;
        float elapsedTime = 0;
        while (elapsedTime < _timeToScaleLife)
        {
            curColorVec.a = Mathf.Lerp(MAX_ALPHA, MIN_ALPHA, (elapsedTime / _timeToScaleColor));
            newColorVec.a = Mathf.Lerp(MIN_ALPHA, MAX_ALPHA, (elapsedTime / _timeToScaleColor));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
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
        int key = bar;
        for (int i = 0; i < livesToRemove; ++i)
        {
            int startVal = _lifeFillVal[key].Item2;
            int endVal =  _lifeFillVal[key].Item1;
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

    public void AddColor(ColorManager.ColorLayer cl)
    {
        return;
    }
}
