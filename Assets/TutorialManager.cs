using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public enum TutorialState
    {
        MOVE, JUMP, ATTACK, COLOR
    }
    [SerializeField] private Image tutorial;

    public static TutorialManager Manager;
    private TutorialState _state;

    private void Awake()
    {
        if (Manager == null)
            Manager = this;
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowTutorial()
    {
        tutorial.gameObject.SetActive(true);
    }
    
    public void HideTutorial()
    {
        tutorial.gameObject.SetActive(false);
    }

    public void SetState(TutorialState s)
    {
        _state = s;
    }

    public TutorialState State => _state;
}
