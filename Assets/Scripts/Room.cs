using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class Room : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera camera;
    [SerializeField] private Door[] doors;
    [SerializeField] private GameObject contents;

    private SpriteRenderer _renderer;
    private Color _color;

    private void OnEnable()
    {
        // DisableContents();    
    }

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public CinemachineVirtualCamera Camera => camera;

    public void DisableContents()
    {   
        // if (contents != null)
        //     contents.SetActive(false);
    }
    
    public void EnableContents()
    {   
        // if (contents != null)
        //     contents.SetActive(true);
    }

    public Color Color
    {
        get => _color;
        set
        {
            _color = value;
            _renderer.color = value;
        }
    }
    
    

  
}
