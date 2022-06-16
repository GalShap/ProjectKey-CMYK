using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Serializable]
    struct TutorialButton
    {
        public GameObject keyboardImage;
        public GameObject gamepadImage;
    }
    public enum TutorialState
    {
        MOVE, JUMP, ATTACK, COLOR, OFF,
        END
    }

    [SerializeField] private List<TutorialButton> buttons;
    
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

    private void Update()
    {
        var names = Input.GetJoystickNames();
        ToggleInput(names.Length > 0 && !names[0].Equals(""));
    }
    
    public void ToggleInput(bool gamepad)
    {
        foreach (var button in buttons)
        {
            button.gamepadImage.SetActive(gamepad);
            button.keyboardImage.SetActive(!gamepad);
        }
    }

    public void ShowTutorial()
    {
        // tutorial.gameObject.SetActive(true);
    }
    
    public void HideTutorial()
    {
        // tutorial.gameObject.SetActive(false);
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

    // public TutorialState State => (tutorial.gameObject.activeSelf) ? _state : TutorialState.OFF;
    public TutorialState State => _state;
    public void RaiseImage()
    {
        // var pos = tutorial.rectTransform.anchoredPosition;
        // pos.y += 100;
        // tutorial.rectTransform.anchoredPosition = pos;
    }
}
