using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDialogues : MonoBehaviour
{
    public void Chase1()
    {
        transform.parent.position = transform.position;
        transform.localPosition = Vector3.zero;
        TimelineManager.Manager.Pause();
        DialogueManager.Manager.LoadDialogue(DialogueManager.Dialogues.CHASE_KEY, true, () => {TutorialManager.Manager.ShowTutorial();});
    }
}
