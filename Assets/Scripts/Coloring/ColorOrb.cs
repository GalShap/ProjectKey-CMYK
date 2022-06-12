using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColorOrb : MonoBehaviour
{
    [SerializeField] private LayerMask layer;
    [SerializeField] private UnityEvent onTake = null;
 
    [SerializeField] private GameObject OrbForceField;
    [SerializeField] private List<GameObject> gameObjectsForScene;

    [SerializeField] private bool _returnToDialogue;

    
    private SpriteRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        Init();
    }

    private void OnEnable()
    {
        for (int i = 0; i < gameObjectsForScene.Count; i++)
        {
            gameObjectsForScene[i].SetActive(true);
        }
        AudioManager.SharedAudioManager.SetVolume((int) AudioManager.AudioSources.Music, 0.1f);
    }

    private void Init()
    {
        ColorManager.ColorLayer? cl = ColorManager.GetColorLayer(layer.value);
        if (cl == null)
            return;
        _renderer.color = (Color) cl?.color;
        gameObject.layer = (int) cl?.index;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {   
            onTake.Invoke();
            AudioManager.SharedAudioManager.PlayUiSounds((int) AudioManager.UiSounds.LoadNewColor);
            StartCoroutine(NewPowerSequence());
        }
    }

    private IEnumerator NewPowerSequence()
    {   
        InputManager.Manager.DisableAll();
        GetComponent<ParticleSystem>().Play();
        OrbForceField.SetActive(true);
        yield return new WaitForSeconds(3f);
      
        if(onTake != null)
            onTake.Invoke();
        
        ColorManager.AddColor(layer);
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < gameObjectsForScene.Count; i++)
        {   
            gameObjectsForScene[i].SetActive(false);
        }
        AudioManager.SharedAudioManager.SetVolume((int) AudioManager.AudioSources.Music,
            AudioManager.SharedAudioManager.defaultVolume);
        
        if (_returnToDialogue) InputManager.Manager.EnableDialogue();
        
        else InputManager.Manager.EnablePlayer();
        AudioManager.SharedAudioManager.PlayUiSounds((int) AudioManager.UiSounds.NewColorEnd);
        Destroy(gameObject);
    }
}
