using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for handling PauseMenu interactions
/// </summary>
public class PauseMenu : MonoBehaviour
{
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        // Pauses the game
        Time.timeScale = 0;
    }

    /// <summary>
    /// Goes to MainMenu
    /// </summary>
    public void HandleQuitButtonOnClickEvent()
    {
        AudioManager.PlayButtonClickAudio();
        MenuManager.GoToMenu(MenuName.Main);
    }

    /// <summary>
    /// Resumes the game
    /// </summary>
    public void HandleResumeButtonOnClickEvent()
    {
        AudioManager.PlayButtonClickAudio();
        Destroy(gameObject);
    }

    /// <summary>
    /// OnDestroy is called when the object is destroyed
    /// </summary>
    void OnDestroy()
    {
        // Resumes the game
        Time.timeScale = 1;
    }
}
