using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueGod : MonoBehaviour
{
    [SerializeField] private Transform playerAnim;
    [SerializeField] private float toggleSpeed = 0.8f;

    private float counter;
    private bool blue;
    private bool toggle;

    private void Update()
    {
        if (!toggle)
            return;
        counter += Time.deltaTime;
        if (counter >= toggleSpeed)
        {
            counter = 0;
            if (blue)
            {
                blue = false;
                ColorManager.SetColor(ColorManager.ColorName.Neutral);
            }
            else
            {
                blue = true;
                ColorManager.SetColor(ColorManager.ColorName.Cyan);
            }
        }
    }

    public void StartChase()
    {
        TimelineManager.Manager.Pause();
        DialogueManager.Manager.LoadDialogue(DialogueManager.Dialogues.CHASE_BLUE, true, () =>
        {
            TimelineManager.Manager.Play();
        });
    }

    public void Bridge()
    {
        TimelineManager.Manager.Pause();
        DialogueManager.Manager.LoadDialogue(DialogueManager.Dialogues.BRIDGE, true, () =>
        {
            TimelineManager.Manager.Play();
        });
    }

    public void HideBridge()
    {
        blue = true;
        ColorManager.SetColor(ColorManager.ColorName.Cyan);
    }

    public void ToggleColors()
    {
        counter = toggleSpeed;
        toggle = true;
    }
    
    public void EndDialogue()
    {
        toggle = false;
        ColorManager.SetColor(ColorManager.ColorName.Neutral);
        TimelineManager.Manager.Pause();
        DialogueManager.Manager.LoadDialogue(DialogueManager.Dialogues.BLUE_END, true, () => { TutorialManager.Manager.ShowTutorial(); });
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("AttackCollider"))
        {
            DialogueManager.Manager.LoadDialogue(DialogueManager.Dialogues.BLUE_DEAD, true, () =>
            {
                TimelineManager.Manager.Play();
            });
        }
    }
}
