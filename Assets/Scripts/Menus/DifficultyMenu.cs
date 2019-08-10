using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls behaviour of DifficultyMenu buttons
/// </summary>
public class DifficultyMenu : MonoBehaviour
{
    /// <summary>
    /// Starts the game on Easy setting
    /// </summary>
    public void HandleEasyButtonOnClickEvent()
    {
        PlayButtonClickSound();
        ConfigUtils.Difficulty = DifficultyName.Easy;
        SceneManager.LoadScene("Scene0");
    }

    /// <summary>
    /// Starts the game on Medium setting
    /// </summary>
    public void HandleMediumButtonOnClickEvent()
    {
        PlayButtonClickSound();
        ConfigUtils.Difficulty = DifficultyName.Medium;
        SceneManager.LoadScene("Scene0");
    }

    /// <summary>
    /// Starts the game on Hard setting
    /// </summary>
    public void HandleHardButtonOnClickEvent()
    {
        PlayButtonClickSound();
        ConfigUtils.Difficulty = DifficultyName.Hard;
        SceneManager.LoadScene("Scene0");
    }

    /// <summary>
    /// Goes to the MainMenu
    /// </summary>
    public void HandleBackButtonOnClickEvent()
    {
        PlayButtonClickSound();
        MenuManager.GoToMenu(MenuName.Main);
    }

    /// <summary>
    /// Plays sound when a button is clicked
    /// </summary>
    void PlayButtonClickSound()
    {
        AudioManager.PlayButtonClickAudio();
    }
}
