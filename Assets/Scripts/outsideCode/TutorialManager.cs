using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public enum TutorialState
    {
        MOVE, JUMP, ATTACK, COLOR, OFF,
        END
    }
    [SerializeField] private Image tutorial;

    [SerializeField] private List<UnityEvent> timelineEvents;
    
    private int index = 0;

    public static TutorialManager Manager;
    private TutorialState _state;

    private void Awake()
    {
        if (Manager == null)
        {
            Manager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowTutorial()
    {
        // tutorial.gameObject.SetActive(true);
    }
    
    public void HideTutorial()
    {
        tutorial.gameObject.SetActive(false);
    }

    public void SetState(TutorialState s)
    {
        _state = s;
    }

    public void NextEvent()
    {
        timelineEvents[index].Invoke();
        index++;
    }

    public TutorialState State => (tutorial.gameObject.activeSelf) ? _state : TutorialState.OFF;

    public void RaiseImage()
    {
        var pos = tutorial.rectTransform.anchoredPosition;
        pos.y += 100;
        tutorial.rectTransform.anchoredPosition = pos;
    }
}
