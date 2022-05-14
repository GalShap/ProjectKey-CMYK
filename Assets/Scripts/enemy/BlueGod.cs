using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueGod : MonoBehaviour
{
    public void StartChase()
    {
        TimelineManager.Manager.Pause();
        DialogueManager.Manager.LoadDialogue(DialogueManager.Dialogues.CHASE_BLUE, true, () => {TimelineManager.Manager.Play();});
    }
}
