using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An audio manager
/// </summary>
public static class AudioManager
{
    #region fields

    // Declare important fields
    static bool initialze = false;
    static bool enabled = true;
    static AudioSource audioSource;
    static AudioClip loseLife;
    static AudioClip gameOver;
    static AudioClip gameWon;
    static AudioClip powerUp;
    static AudioClip buttonClick;

    #endregion

    #region Properties

    /// <summary>
    /// Checks whether Audio has been initialized
    /// </summary>
    public static bool IsInitialized
    {
        get { return initialze; }
    }

    /// <summary>
    /// Checks whether Audio has been enabled
    /// </summary>
    public static bool IsEnabled
    {
        get { return enabled; }
    }

    /// <summary>
    /// Enables or disables the Audio
    /// </summary>
    public static bool Enabled
    {
        set { enabled = value; }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Initializes Audio
    /// </summary>
    /// <param name="source">Audio source</param>
    public static void InitializeAudio(AudioSource source)
    {
        audioSource = source;
        loseLife = Resources.Load<AudioClip>("LoseLife");
        gameOver = Resources.Load<AudioClip>("GameOver");
        gameWon = Resources.Load<AudioClip>("GameWon");
        powerUp = Resources.Load<AudioClip>("PowerUp");
        buttonClick = Resources.Load<AudioClip>("ButtonClick");
        initialze = true;
    }

    /// <summary>
    /// Play Lose Life audio
    /// </summary>
    public static void PlayLoseLifeAudio()
    {
        if (enabled)
        {
            audioSource.PlayOneShot(loseLife);
        }
    }

    /// <summary>
    /// Play Game Over audio
    /// </summary>
    public static void PlayGameOverAudio()
    {
        if (enabled)
        {
            audioSource.PlayOneShot(gameOver);
        }
    }

    /// <summary>
    /// Play Game Won audio
    /// </summary>
    public static void PlayGameWonAudio()
    {
        if (enabled)
        {
            audioSource.PlayOneShot(gameWon);
        }
    }

    /// <summary>
    /// Play Power Up audio
    /// </summary>
    public static void PlayPowerUpAudio()
    {
        if (enabled)
        {
            audioSource.PlayOneShot(powerUp);
        }
    }

    /// <summary>
    /// Play Button Click audio
    /// </summary>
    public static void PlayButtonClickAudio()
    {
        if (enabled)
        {
            audioSource.PlayOneShot(buttonClick);
        }
    }

    #endregion
}
