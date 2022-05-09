using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera camera;
    [SerializeField] private Door[] doors;

    private SpriteRenderer _renderer;
    private Color _color;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public CinemachineVirtualCamera Camera => camera;

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