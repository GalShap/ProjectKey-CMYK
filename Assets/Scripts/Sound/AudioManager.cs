using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class AudioManager : MonoBehaviour
{
    
    [Serializable]
    class MusicAudioQueue 
    {   
        [Tooltip("drag all the game soundtrack to here")]
        [SerializeField] private List<AudioClip> tracks;

        [Tooltip("Drag the audio source that plays the game tracks to here")] [SerializeField]
        private AudioSource tracksAudioSource; 

        private static int _curTrackIndex = 0;
        
        /// <summary>
        /// plays the next audio track. Used When we want to go between worlds and sounds. 
        /// </summary>
        public void PlayNextTrack()
        {   
          
            _curTrackIndex = (_curTrackIndex == tracks.Count - 1) ? 0 : ++_curTrackIndex;
            tracksAudioSource.Stop();
            tracksAudioSource.clip = tracks[_curTrackIndex];
            tracksAudioSource.Play();
        }
        
        /// <summary>
        /// plays the previous audio track. Used When we want to go between worlds and sounds. 
        /// </summary>
        public void PlayPrevTrack()
        {
            if (_curTrackIndex != 0)
                _curTrackIndex--;
            print("index: " + _curTrackIndex); 
            tracksAudioSource.Stop();
            tracksAudioSource.clip = tracks[_curTrackIndex];
            tracksAudioSource.Play();
        }
        /// <summary>
        /// plays track of the current index
        /// </summary>
        public void PlayCurTrack()
        {
            tracksAudioSource.Stop();
            tracksAudioSource.clip = tracks[_curTrackIndex];
            tracksAudioSource.Play();
        }
        
        /// <summary>
        ///  gets an index and plays the track with that index. 
        /// </summary>
        /// <param name="index">
        /// the index to play. 
        /// </param>
        public void PlayTrackByIndex(int index)
        {
            if (index >= tracks.Count  - 1 || index < 0)
            {
                Debug.LogWarning("Invalid Index given, method didn't do nothing");
                return;
            }
            
            tracksAudioSource.Stop();
            tracksAudioSource.clip = tracks[index];
            tracksAudioSource.Play();
            _curTrackIndex = index;
        }

        public AudioSource GetMusicAudioSrc()
        {
            return tracksAudioSource;
        }
    }
    
    [Tooltip("drag all tracks in the game to here")]
    [SerializeField] private MusicAudioQueue musicAudioQueue;

    [SerializeField] private AudioSource enemyAudioSource;

    [SerializeField] private AudioSource keyAudioSource;

    [SerializeField] private AudioSource uiAudioSource;

    [SerializeField] private List<AudioClip> keyAudioClips;

    [SerializeField] private List<AudioClip> enemyAudioClips;

    [SerializeField] private List<AudioClip> uiAudioClips;

    [SerializeField] public float defaultVolume = 0.7f;
    
    private const float MaxVolume = 1f;

    public static AudioManager SharedAudioManager;
    
    public enum UiSounds
    {
        DialogueLetters, NextDialogue    
    }
    
    public enum KeySounds
    {
        Jump, Attack, Hit, ColorSwitch, Death
    }

    public enum EnemySounds
    {
        Hit, Shoot, Death
    }
    
    public enum AudioSources
    {
        Music, Key, Enemy, UI 
    }

    public void PlayPrev()
    {
        musicAudioQueue.PlayPrevTrack();
    }
    
    private void Awake()
    {   
        SharedAudioManager = this;
        if (this != SharedAudioManager)
            Destroy(this);


        enemyAudioSource.volume = defaultVolume;
        keyAudioSource.volume = defaultVolume;
        uiAudioSource.volume = defaultVolume;

    }
    public void LoadNextMusic()
    {   
        
        musicAudioQueue.PlayNextTrack();
    }

    public void StopMusic()
    {
        musicAudioQueue.GetMusicAudioSrc().Stop();
    }

    public void Play()
    {   
        
        musicAudioQueue.PlayCurTrack();
    }
    
    /// <summary>
    /// call this when an enemy is being hit by key.
    /// </summary>
    public void PlayEnemyHitSound()
    {
        enemyAudioSource.Play();
    }
    
    /// <summary>
    /// needs to be called when key is making an action. gets an action index and plays the sound accordingly. 
    /// </summary>
    /// <param name="action">
    /// 0 - Jump
    /// 1 - Attack
    /// 2 - Hit
    /// 3 - Color Switch
    /// 4 - Death
    /// </param>
    public void PlayKeyActionSound(int action)
    {
        // illegal action!
        if (action < (int) KeySounds.Jump || action > keyAudioClips.Count - 1)
            return;

        keyAudioSource.clip = keyAudioClips[action];
        keyAudioSource.PlayOneShot(keyAudioSource.clip);
    }


    /// <summary>
    /// needs to be called when enemy sound needs to be played. gets an action index and plays the sound accordingly. 
    /// </summary>
    /// <param name="action">
    /// 0 - Hit
    /// 1 - Shoot
    /// 2 - Death
    ///  </param>
    public void PlayEnemySounds(int action)
    {
        if (action < (int) EnemySounds.Hit || action > enemyAudioClips.Count - 1)
            return;

        enemyAudioSource.clip = enemyAudioClips[action];
        enemyAudioSource.PlayOneShot(enemyAudioSource.clip);
    }
    
    /// <summary>
    /// needs to be called when ui sound needs to be played. gets an action index and plays the sound accordingly. 
    /// </summary>
    /// <param name="action">
    /// 0 - dialogue letters effect
    /// 1 - next line arrow
    /// 2 - 
    ///  </param>
    public void PlayUiSounds(int action)
    {
        uiAudioSource.volume = 0.3f; 
        if (action < (int) UiSounds.DialogueLetters || action > uiAudioClips.Count - 1)
            return;

        if (action == (int) UiSounds.NextDialogue) uiAudioSource.volume = MaxVolume;
            
        uiAudioSource.clip = enemyAudioClips[action];
        //uiAudioSource.PlayOneShot(uiAudioSource.clip);
        uiAudioSource.Play();
        
    }

    public void PlayByIndex(int index)
    {   
     
        musicAudioQueue.PlayTrackByIndex(index);
    }

    public void SetVolume(int audio, float volume)
    {
        switch (audio)
        {
            case (int) AudioSources.Music:
                musicAudioQueue.GetMusicAudioSrc().volume = volume;
                return;
            
            case (int) AudioSources.Key:
                keyAudioSource.volume = volume;
                return;
            
            case (int) AudioSources.Enemy:
                enemyAudioSource.volume = volume;
                return;
            
            case (int) AudioSources.UI:
                uiAudioSource.volume = volume;
                return;
        }
    }


}
