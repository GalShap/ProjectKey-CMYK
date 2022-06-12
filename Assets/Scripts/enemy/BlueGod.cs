using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueGod : MonoBehaviour
{
    [SerializeField] private Transform godAnim;
    [SerializeField] private float toggleSpeed = 0.8f;

    private float counter;
    private bool blue;
    private bool toggle;
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private CapsuleCollider2D _collider;
    private static readonly int Almost = Animator.StringToHash("Almost");
    private static readonly int Die = Animator.StringToHash("Die");

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<CapsuleCollider2D>();
        _rigidbody = GetComponentInChildren<Rigidbody2D>();
        
    }

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
                AudioManager.SharedAudioManager.PlayBossSounds((int) AudioManager.BossSounds.CyanColorChange);
                ColorManager.SetColor(ColorManager.ColorName.Neutral);
            }
            else
            {
                blue = true;
                AudioManager.SharedAudioManager.PlayBossSounds((int) AudioManager.BossSounds.CyanColorChange);
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
        AudioManager.SharedAudioManager.PlayBossSounds((int) AudioManager.BossSounds.CyanColorChange);
    }

    public void ToggleColors()
    {
        counter = toggleSpeed;
        toggle = true;
    }

    public void StopToggle()
    {
        toggle = false;
        ColorManager.SetColor(ColorManager.ColorName.Neutral);
    }

    public void StartToggle()
    {
        counter = toggleSpeed;
        toggle = true;
    }
    
    public void EndDialogue()
    {
        toggle = false;
        ColorManager.SetColor(ColorManager.ColorName.Neutral);
        TimelineManager.Manager.Pause();
        DialogueManager.Manager.LoadDialogue(DialogueManager.Dialogues.BLUE_END, true, () =>
        {
            TutorialManager.Manager.ShowTutorial();
            _collider.enabled = true;
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        });
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("AttackCollider"))
        {
            godAnim.localPosition = Vector3.zero;
            _animator.SetTrigger(Almost);
            DialogueManager.Manager.LoadDialogue(DialogueManager.Dialogues.BLUE_DEAD, true, () =>
            {
                _animator.SetTrigger(Die);
                TimelineManager.Manager.Play();
            });
        }
    }

    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         godAnim.localPosition = Vector3.zero;
    //         _animator.SetTrigger(Almost);
    //         DialogueManager.Manager.LoadDialogue(DialogueManager.Dialogues.BLUE_DEAD, true, () =>
    //         {
    //             _animator.SetTrigger(Die);
    //             TimelineManager.Manager.Play();
    //         });   
    //     }
    // }

    public void Hit()
    {
        godAnim.localPosition = Vector3.zero;
        _animator.SetTrigger(Almost);
        DialogueManager.Manager.LoadDialogue(DialogueManager.Dialogues.BLUE_DEAD, true, () =>
        {
            _animator.SetTrigger(Die);
            TimelineManager.Manager.Play();
        });
    }
}
