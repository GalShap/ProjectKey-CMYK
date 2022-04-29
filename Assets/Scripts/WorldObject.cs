using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour
{   
    
    [SerializeField] [Range(0,2)] protected int curColor;

    protected enum Color
    {
        Red,Blue,Green
    }
    
    
        // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
