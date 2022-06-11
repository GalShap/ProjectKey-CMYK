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
            StartCoroutine(NewPowerSequence());
        }
    }

    private IEnumerator NewPowerSequence()
    {   
        
        GetComponent<ParticleSystem>().Play();
        OrbForceField.SetActive(true);
        yield return new WaitForSeconds(3f);
      
        if(onTake != null)
            onTake.Invoke();
        Destroy(gameObject);
       
        for (int i = 0; i < gameObjectsForScene.Count; i++)
        {
            gameObjectsForScene[i].SetActive(false);
        }
        ColorManager.AddColor(layer);
        
        AudioManager.SharedAudioManager.SetVolume((int) AudioManager.AudioSources.Music,
            AudioManager.SharedAudioManager.defaultVolume);
    }
}
