using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    
    [Tooltip("write all dialogues in here")]
    [SerializeField] private List<string> Dialogues;
    
    [Tooltip("for every line, enter the index of it's speaker, should be in correspondence to the sprite list")]
    [SerializeField] private List<int> Speakers;

    [SerializeField] private List<Sprite> SpeakersSprites; 
    
    [SerializeField] private Image speakerImage;
    
    [SerializeField] private TextMeshProUGUI TextBox;
    
    // the string is the dialogue the character says
    // the int is the index of that character sprite in the SpeakersSprites list; 
    private Queue<Tuple<string, int>> _dialogueLines;

    private float _timeToScaleBox = 0.5f;

    public bool hasDialogue = false;
    
    public static DialogueManager Manager;

    private const int NO_LINES = 0;

    private const int ZERO_SCALE = 0;

    private const int FULL_SCALE = 1;
    void Awake()
    {   
        // singleton stuff
        if (Manager == null) Manager = this;
        else if (Manager != this) Destroy(gameObject);
        
        // set up the dialogue Lines 
        _dialogueLines = new Queue<Tuple<string, int>>();
        for (int i = 0; i < Dialogues.Count; i++)
        {
            var curLine = new Tuple<string, int>(Dialogues[i], Speakers[i]);
            _dialogueLines.Enqueue(curLine);
        }

        if (_dialogueLines.Count > NO_LINES)
            hasDialogue = true;
        
        Debug.Log(_dialogueLines.Count);
    }

    private void OnEnable()
    {
        StartCoroutine(ResizeDialogueBox(ZERO_SCALE, FULL_SCALE));
    }

    public void DisableDialog()
    {
        StartCoroutine(ResizeDialogueBox(FULL_SCALE, ZERO_SCALE));
        gameObject.SetActive(true);
    }
    private void Start()
    {  
        if (hasDialogue)
            NextDialogue();
    }

    private void FixedUpdate()
    {
        if (Input.anyKeyDown && hasDialogue)
        {
            NextDialogue();
        }
    }

    private void NextDialogue()
    {
        Tuple<string, int> nextLine = _dialogueLines.Dequeue();

        StartCoroutine(DisplayNextLine(nextLine));
        
        if (_dialogueLines.Count == NO_LINES)
            hasDialogue = false;
    }

    IEnumerator DisplayNextLine(Tuple<string, int> next)
    {
        speakerImage.sprite = SpeakersSprites[next.Item2];
        TextBox.text = "";
        foreach (var letter in next.Item1)
        {
            TextBox.text += letter;
            yield return new WaitForSecondsRealtime(0.0125f);
        }
    }

    IEnumerator ResizeDialogueBox(float start, float end)
    {   
        
        Vector3 newScale = new Vector3(start, start, start);
        float elapsedTime = 0;
        while (elapsedTime < _timeToScaleBox)
        {
            float curVal = Mathf.Lerp(start, end, (elapsedTime / _timeToScaleBox));
            newScale = new Vector3(curVal, curVal, curVal);
            gameObject.GetComponent<RectTransform>().localScale = newScale;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    
}

