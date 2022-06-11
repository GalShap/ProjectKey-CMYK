using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerDialogues : MonoBehaviour
{
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private Collider2D blueCollider;
    [SerializeField] private MagentaGod pinky;
    [SerializeField] private UnityEvent yellowBoss;

    private void Start()
    {
        if (blueCollider != null) 
            Physics2D.IgnoreCollision(playerCollider, blueCollider, true);
    }

    public void Chase1()
    {
        Physics2D.IgnoreCollision(playerCollider, blueCollider, false);
        TimelineManager.Manager.Pause();
        DialogueManager.Manager.LoadDialogue(DialogueManager.Dialogues.CHASE_KEY, true, () => {TutorialManager.Manager.ShowTutorial();});
    }

    public void Chase2()
    {
        TimelineManager.Manager.Pause();
        DialogueManager.Manager.LoadDialogue(DialogueManager.Dialogues.DAMN_IT, true);
    }

    public void TakeOrb()
    {
        DialogueManager.Manager.LoadDialogue(DialogueManager.Dialogues.TAKE_ORB, true, () => {TutorialManager.Manager.ShowTutorial();});
    }

    public void EndTutorial()
    {
        DialogueManager.Manager.LoadDialogue(DialogueManager.Dialogues.ONE_DOWN, true,
            () => { SceneManager.LoadScene("Game");});
    }

    public void BeforePink()
    {
        DialogueManager.Manager.LoadDialogue(DialogueManager.Dialogues.BEFORE_PINK);
    }

    public void PinkFirst()
    {
        DialogueManager.Manager.LoadDialogue(DialogueManager.Dialogues.PINK, true, (() => { pinky.Playing = true;}));
    }
    
    public void PinkAgain()
    {
        DialogueManager.Manager.LoadDialogue(DialogueManager.Dialogues.PINK_AGAIN, true, (() => { pinky.Playing = true;}));
    }

    public void YellowFirst()
    {
        DialogueManager.Manager.LoadDialogue(DialogueManager.Dialogues.YELLOW_FIGHT_1, true, 
            (() =>
            {
                yellowBoss.Invoke();
            }));
    }
    
    public void YellowFight1()
    {
        TimelineManager.Manager.Pause();
        DialogueManager.Manager.LoadDialogue(DialogueManager.Dialogues.YELLOW_FIGHT_1, true, 
            (() =>
            {
                TimelineManager.Manager.Play();
            }));
    }
    
    public void YellowFight2()
    {
        TimelineManager.Manager.Pause();
        DialogueManager.Manager.LoadDialogue(DialogueManager.Dialogues.YELLOW_FIGHT_2, true, 
            (() =>
            {
                TimelineManager.Manager.Play();
            }));
    }
    
    public void YellowFight3()
    {
        TimelineManager.Manager.Pause();
        DialogueManager.Manager.LoadDialogue(DialogueManager.Dialogues.YELLOW_FIGHT_3, true, 
            (() =>
            {
                TimelineManager.Manager.Play();
            }));
    }

    public void YellowGive1()
    {
        TimelineManager.Manager.Pause();
        DialogueManager.Manager.LoadDialogue(DialogueManager.Dialogues.YELLOW_GIVE_1, true,
            (() => {
            {
                TimelineManager.Manager.Play();
            }}));
    }
    
    public void YellowGive2()
    {
        TimelineManager.Manager.Pause();
        DialogueManager.Manager.LoadDialogue(DialogueManager.Dialogues.YELLOW_GIVE_2, true,
            (() => {
            {
                TimelineManager.Manager.Play();
            }}));
    }

    public void YellowDead1()
    {
        DialogueManager.Manager.LoadDialogue(DialogueManager.Dialogues.YELLOW_DEAD_1);
    }
    
    public void YellowDead2()
    {
        DialogueManager.Manager.LoadDialogue(DialogueManager.Dialogues.YELLOW_DEAD_2);
    }
}
