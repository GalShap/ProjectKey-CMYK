using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ChangingColorsPlatform : ColorObject
{
    [SerializeField] private List<LayerMask> colorLayers;

    [SerializeField] private int startIndex = 0;

    [SerializeField] private float changeSpeed = 3f;

    private float time = 0f;
    
    private int curIdx;

    private const int RESET_TIME = 0;

    protected override void Start()
    {
        
        layer.value = colorLayers[startIndex].value;
        base.Start();
    }
    
    private void Update()
    {
        time += Time.deltaTime;
        if (time >= changeSpeed)
        {
            curIdx++;
            if (curIdx == colorLayers.Count)
                curIdx = 0;

            layer.value = colorLayers[curIdx].value;
            SetColor();
            SetLayer();

            time = RESET_TIME;
        }
        


    }
}
