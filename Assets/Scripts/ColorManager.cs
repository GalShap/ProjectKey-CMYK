using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    
    #region Inspector
    
    [SerializeField] private GameObject Player;

    [SerializeField] private int CurWorldColor = -1;
    
    [SerializeField] private int TotalColors = 3;

    [SerializeField] private SpriteRenderer Background;

    [SerializeField] private List<Color> Colors = new List<Color>() {Color.red, Color.green, Color.blue};
    #endregion

    
    #region Constants
    private const int PLAYER_LAYER = 6;
    
    private const int FIRST_COLOR_LAYER = 7;
    
    #endregion
    
    protected enum CurColor
    {
        Default, Red, Green, Blue
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if (CurWorldColor != -1)
            SetWorldColor(CurWorldColor);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Private Methods
    
    // function disables the given layer and enables all other layers. 
    private void CancelCollisionLayer(int layer)
    {
        int i = FIRST_COLOR_LAYER + 1;

        while (i <= FIRST_COLOR_LAYER + TotalColors)
        {
            if (i == layer) // cancel the given color collisions with player.
                Physics2D.IgnoreLayerCollision(layer, PLAYER_LAYER, true);
            
            else 
                Physics2D.IgnoreLayerCollision(i, PLAYER_LAYER, false);
        }
    }
    
    
    // color: the index of the color in the Colors list
    private void SetBackGroundColor(int color)
    {
        Background.color = Colors[color];
    }
    
    #endregion
    
    #region Public Methods
    
    public void SetWorldColor(int color)
    {
        SetBackGroundColor(color);
        CancelCollisionLayer(color + FIRST_COLOR_LAYER);
    }
    
    #endregion
    
}
