using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    
    [Serializable]
    class MusicAudioQueue 
    {   
        [Tooltip("drag all the game soundtrack to here")]
        [SerializeField] private List<AudioClip> tracks;
        
        [Tooltip("Drag the audio source that plays the game tracks to here")]
        [SerializeField] private AudioSource tracksAudioSource;

        private static int _curTrackIndex = 0;
        
        /// <summary>
        /// plays the next audio track. Used When we want to go between worlds and sounds. 
        /// </summary>
        public void PlayNextTrack()
        {
            _curTrackIndex = (_curTrackIndex == tracks.Count - 1) ? 0 : _curTrackIndex++;
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
            if (index >= tracks.Count  - 1|| index < 0)
            {
                Debug.LogWarning("Invalid Index given, method didn't do nothing");
                return;
            }
            
            tracksAudioSource.Stop();
            tracksAudioSource.clip = tracks[_curTrackIndex];
            tracksAudioSource.Play();
            _curTrackIndex = index;
        }
    }
    
    [Tooltip("drag all tracks in the game to here")]
    [SerializeField] private MusicAudioQueue _musicAudioQueue;

    [SerializeField] private AudioSource _enemyAudioSource;

    [SerializeField] private AudioSource _keyAudioSource;
    
    public static AudioManager SharedAudioManager;
    // Start is called before the first frame update

    private void Awake()
    {
        SharedAudioManager = this;
    }


   
}
