using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public enum Dialogues
    {
        CHASE_KEY,
        CHASE_BLUE,
        BLOCK,
        BRIDGE,
        DAMN_IT,
        BLUE_END,
        BLUE_DEAD,
        TAKE_ORB,
        ONE_DOWN,
        BEFORE_PINK,
        PINK,
        PINK_AGAIN,
        PINK_DEAD,
        YELLOW_GIVE_1,
        YELLOW_GIVE_2,
        SECRET_ROOM_CYAN,
        SECRET_ROOM_MAGENTA,
        SECRET_ROOM_YELLOW,
        YELLOW_FIGHT_1,
        YELLOW_FIGHT_2,
        YELLOW_FIGHT_3,
        YELLOW_DEAD_1,
        YELLOW_DEAD_2,
        
    }

    [Serializable]
    public struct Sentence
    {
        public Sentence(string sentence, int id)
        {
            this.sentence = sentence;
            this.id = id;
        }

        private String sentence;
        private int id;

        public string Sentence1 => sentence;

        public int ID => id;
    }

    // [Tooltip("write all dialogues in here")]
    // [SerializeField] private List<string> Dialogues;
    //
    // [Tooltip("for every line, enter the index of it's speaker, should be in correspondence to the sprite list")]
    // [SerializeField] private List<int> Speakers;

    [SerializeField] private List<Sprite> SpeakersSprites;

    [SerializeField] private List<string> TalkerNames;

    [SerializeField] private Image speakerImage;

    [SerializeField] private GameObject Arrow;

    [SerializeField] private TextMeshProUGUI TextBox;

    [SerializeField] private TextMeshProUGUI TalkerNameTextBox;

    // the string is the dialogue the character says
    // the int is the index of that character sprite in the SpeakersSprites list; 
    // private Queue<Tuple<string, int>> _dialogueLines;

    private Queue<Sentence> _dialogueLines = new Queue<Sentence>();

    private Coroutine currentLineCoroutine;

    private Hashtable dialogues = new Hashtable()
    {
        {
            Dialogues.CHASE_BLUE, 
            new List<Sentence>()
            {
                new Sentence("*Huff*...\n*Huff*...",1),
                new Sentence("... Damn...",1)
            }
        },
        {
            Dialogues.CHASE_KEY, 
            new List<Sentence>()
            {
                new Sentence("*Huff*...\n*Huff*...",0),
                new Sentence("Get back here!",0)
            }
        },
        {
            Dialogues.BRIDGE,
            new List<Sentence>()
            {
                new Sentence("Gotcha!",0),
                new Sentence("You didn't think its gonna be that easy wont you?",1)
            }
        },
        {
            Dialogues.DAMN_IT,
            new List<Sentence>()
            {
                new Sentence("DAMNIT!",0),
                new Sentence("After all, we do run this place you know...", 1),
                new Sentence("We'll see who runs things around here very soon cyan!", 0)
            }
        },
        {
            Dialogues.BLOCK,
            new List<Sentence>()
            {
                new Sentence("Gotta move on",0)
            }
        },
        {
            Dialogues.BLUE_END,
            new List<Sentence>()
            {
                new Sentence("*Huff*...\n*Huff*...",1),
                new Sentence("*Huff*...\n*Huff*...",1),
                new Sentence("Rats. Seems like you got me...",1),
                new Sentence("You know I have to do this...",0),
                new Sentence("I've never imagined this day will come",1),
                new Sentence("Come on then! Do your worst!",1)
            }
        },
        {
            Dialogues.BLUE_DEAD,
            new List<Sentence>()
            {
                new Sentence("You... You have.. no idea...",1),
                new Sentence("Wh-what... You're getting yourself... into...",1),
                new Sentence("That may be, But it's too late to go back now.",0),
                new Sentence("All of this, it's only because we saw... your potential...",1),
            }
        },
        {
            Dialogues.TAKE_ORB,
            new List<Sentence>()
            {
                new Sentence("So much power...",0),
                new Sentence("Hard to believe such a wimp could control it...",0)
            }
        },
        {
            Dialogues.ONE_DOWN,
            new List<Sentence>()
            {
                new Sentence("One step closer to get out of this hell hole.",0),
                new Sentence("those so called \"higher beings\" are gonna regret soon...", 0)
            }
        },
        {
            Dialogues.BEFORE_PINK,
            new List<Sentence>()
            {
                new Sentence("What the hell is this music?",0)
            }
        },
        {
            Dialogues.PINK,
            new List<Sentence>()
            {
                new Sentence("What the hell?",0),
                new Sentence("Oh ho ho... Look who we have here! Are you lost my dear?",2),
                new Sentence("Magenta! I should have known when i heard the guitars!",0),
                new Sentence("Nice to see you again too, I heard cyan took a little beating by you...",2),
                new Sentence("...Then again, the old ink spot never knew how to deal with his gift", 2),
                new Sentence("and you're gonna suffer a similar fate if you don't let me through",0),
                new Sentence("Careful now hon', you might find yourself all pierced up if you keep this talk.",2),
                new Sentence("Do I? I think you and your brother are gonna unite real soon",0),
                new Sentence("That's it! Im gonna teach you some manners you ungrateful shit ",2)
            }
        },
        {
            Dialogues.PINK_AGAIN,
            new List<Sentence>()
            {
                new Sentence("Back again? Sucks for you. Get ready for round two!",2),
                new Sentence("Lets just get this over with...",0)
            }
        },
        {
            Dialogues.PINK_DEAD,
            new List<Sentence>()
            {
                new Sentence("Key...Listen...\"groans\" spare me, please!" ,2),
                new Sentence("you've been treating me like nothing for to long magenta, you got whats coming to you, " +
                             "that's all",0),
                new Sentence("Key...I beg you... ", 2),
                new Sentence("All my though love was only to make you the strongest!",2),
                new Sentence("So why you're complaining?, you clearly succeeded", 0),
                }
        },
        {
            Dialogues.YELLOW_GIVE_1,
            new List<Sentence>()
            {   
                new Sentence("Hello Daughter i've been waiting for you.",3),
                new Sentence("Daughter? You're just as sick as everyone else",0),
                new Sentence("Oh you don't mean that, don't be foolish", 3),
                new Sentence("Anyway, i want you to have this key...", 3),
                new Sentence("Why will you do this? what is this sick game", 0),
                new Sentence("I just want you to get a taste of what you can do if you stop this madness!", 3),
                new Sentence("If you truly knew me like you think you do, then you'd know i won't go back!", 0)
                
              
            }
        },
        {
            Dialogues.YELLOW_GIVE_2,
            new List<Sentence>()
            {
                new Sentence("you made a great journey, but it will have to come to an end im afraid", 3),
                new Sentence("funny, your bothers said the same...", 0),
                new Sentence("Magenta and Cyan will be missed, but its a small price to pay...", 3),
                new Sentence("I hope you make it alive my child, you truly are remarkable", 3)
            }
        },
        {
            Dialogues.SECRET_ROOM_CYAN,
            new List<Sentence>()
            {
                new Sentence("dammit Cyan, you really left me no choice you know...", 0),
               
            }
        },
        {
            Dialogues.SECRET_ROOM_MAGENTA,
            new List<Sentence>()
            {
                new Sentence("you were always a sick bastard Magenta, one can only wonder " +
                             "how you abused all those other test subjects", 0)
            }
        },
        {
            Dialogues.SECRET_ROOM_YELLOW,
            new List<Sentence>()
            {
                new Sentence("Still can't believe she sees herself as my mother, how twisted...", 0)
            }
        },
        {   
            
            Dialogues.YELLOW_FIGHT_1,
            new List<Sentence>()
            {
                new Sentence("YOU!",0),
                new Sentence("How was your little detour, my child?",3),
                new Sentence("How do you think?! What the hell is going on here?",0),
                new Sentence("Confused? I bet you are. But know this is all for the greater good.",3),
                new Sentence("Now, child... How about we take a little walk and discuss what you've just seen.",3),
                new Sentence("I'm not going anywhere with you. I'm putting an end to all of this!",0),
                new Sentence("What a shame. And here I thought we managed to create a more civil prototype.",3),
                new Sentence("If you wish to fight, I suppose I shall take this back.",3),
            }
        },
        {
            Dialogues.YELLOW_FIGHT_2,
            new List<Sentence>()
            {
                new Sentence("AW C'MON!",0),
                new Sentence("What do you say, child? Maybe we settle this in a more calm manner?",3),
                new Sentence("Take you stupid power, I'm gonna kick your ass with or without it!",0),
                new Sentence("I see... Then you leave me no choice.",3),
                new Sentence("How disappointing. I really believed you were going to be different than all of the others.",3)
            }
        },
        {
            Dialogues.YELLOW_FIGHT_3,
            new List<Sentence>()
            {
                new Sentence("Come child! Show me the monster I've created!",3)
            }
        },
        {
            Dialogues.YELLOW_DEAD_1,
            new List<Sentence>()
            {
                new Sentence("... ...",3),
                new Sentence("... ...",3),
                new Sentence("*Huff* *Huff*",0),
                new Sentence("You've impressed me, my child.",3),
                new Sentence("Take my power, fulfill your destiny as a god.",3),
                new Sentence("You can keep you title, I'm finally free of this madness.",0)
            }
        }
    };

    private float _timeToScaleBox = 0.5f;

    private float timer = 0.5f;

    private float time = 0;

    public bool hasDialogue = false;

    private bool dialogueEnd = false;
    
    public static DialogueManager Manager;
    private Action _onEnd;

    #region Constants
    
    private const int NO_LINES = 0;

    private const int ZERO_SCALE = 0;

    private const int FULL_SCALE = 1;
    
    #endregion
    
    #region MonoBehaviour
    void Awake()
    {   
        // singleton stuff
        if (Manager == null)
        {
            Manager = this;
            time = timer;
        }
        else if (Manager != this)
        {
            Destroy(gameObject);
        }
        
        // set up the dialogue Lines 
        // _dialogueLines = new Queue<Tuple<string, int>>();
        // for (int i = 0; i < Dialogues.Count; i++)
        // {
        //     var curLine = new Tuple<string, int>(Dialogues[i], Speakers[i]);
        //     _dialogueLines.Enqueue(curLine);
        // }
        //
        // if (_dialogueLines.Count > NO_LINES)
        //     hasDialogue = true;
    }

    private void Start()
    {
        var newScale = new Vector3(0, 0, 0);
        gameObject.GetComponent<RectTransform>().localScale = newScale;
    }

    void Update()
    {
        if(time > 0)
            time -= Time.deltaTime;
        
    }
    #endregion
    
    #region Private Methods
    
    private void NextDialogue()
    {   
        AudioManager.SharedAudioManager.bossAudioSource.Stop();
        if(currentLineCoroutine != null)
            StopCoroutine(currentLineCoroutine);
        Sentence nextLine = _dialogueLines.Dequeue();

        AudioManager.SharedAudioManager.PlayBossSounds((int)AudioManager.BossSounds.CyanTalk);

       currentLineCoroutine = StartCoroutine(DisplayNextLine(nextLine));
        
        if (_dialogueLines.Count == NO_LINES)
            hasDialogue = false;
    }
    private IEnumerator DisplayNextLine(Sentence next)
    {
        
        Arrow.SetActive(false);
        speakerImage.sprite = SpeakersSprites[next.ID];
        TalkerNameTextBox.text = TalkerNames[next.ID];
        TextBox.text = "";
        int index = 0;
        foreach (var letter in next.Sentence1)
        {
            TextBox.text += letter;
            index++;
            //if (index % 2 == 0)
               // 
            
            yield return new WaitForSecondsRealtime(0.025f);
        }
        Arrow.SetActive(true);


        AudioManager.SharedAudioManager.bossAudioSource.Stop();
    }
    private IEnumerator ResizeDialogueBox(float start, float end)
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

    private IEnumerator Wait(float time)
    {
        yield return new WaitForSecondsRealtime(time);
    }
    
    #endregion
    
    #region Public Methods

    /// <summary>
    /// use this function when you want to set the dialogue box to inactive.
    /// resizes the box to 0 and deactivates object. 
    /// </summary>
    public void DisableDialog()
    {
        StartCoroutine(ResizeDialogueBox(FULL_SCALE, ZERO_SCALE));
        gameObject.SetActive(false);
        InputManager.Manager.ToggleMaps(true);
        if (_onEnd != null)
        {
            _onEnd.Invoke();
            _onEnd = null;
        }
    }

    public void LoadDialogue(Dialogues d, bool enable = true, Action onEnd = null)
    {   
        LoadNewDialog((List<Sentence>) dialogues[d], enable, onEnd);
    }
    
    public void EnableDialog()
    {
        dialogueEnd = false;
        gameObject.SetActive(true);
        NextDialogue();
        StartCoroutine(ResizeDialogueBox(ZERO_SCALE, FULL_SCALE));
        InputManager.Manager.ToggleMaps(false);
        time = 1;
    }

    public void BlockDialogue()
    {
        LoadDialogue(Dialogues.BLOCK);
    }

    public void OnNext(InputAction.CallbackContext context)
    {
        if (TimelineManager.Manager.IsPlaying)
            return;
        
        if (context.phase == InputActionPhase.Started)
        {
            if (hasDialogue && (time <= 0))
            {   
               
                AudioManager.SharedAudioManager.PlayUiSounds((int) AudioManager.UiSounds.NextDialogue);
                NextDialogue();
                time = timer;
            }

            else if (!hasDialogue && !dialogueEnd && (time <= 0))
            {
                dialogueEnd = true;
                DisableDialog();
            }    
            
            
        }
    }

    /// <summary>
    /// loads a new block of dialogue to the dialogue manager. 
    /// </summary>
    /// <param name="dialogues">
    /// a list of dialogue lines
    /// </param>
    /// <param name="speakersId">
    /// a list that has the index of the speaker image of the current line in the sprites list. 
    /// </param>
    public void LoadNewDialog(List<Sentence> sentences, bool enable = false, Action onEnd = null)
    {
        _dialogueLines = new Queue<Sentence>();
        for (int i = 0; i < sentences.Count; i++)
        {
            _dialogueLines.Enqueue(sentences[i]);
        }

        if (_dialogueLines.Count > NO_LINES)
            hasDialogue = true;
        
        _onEnd = onEnd;
        
        if(enable)
            EnableDialog();
    }
    
    #endregion
}

