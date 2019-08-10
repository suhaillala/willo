using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioSource : MonoBehaviour
{
    /// <summary>
    /// Awake is called before start
    /// </summary>
    void Awake()
    {
        // Initialize audio
        if (!AudioManager.IsInitialized)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            AudioManager.InitializeAudio(audioSource);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
