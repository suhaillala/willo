using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class for handling GameOverMenu interactions
/// </summary>
public class GameOverMenu : PauseMenu
{
    /// <summary>
    /// Restarts the game
    /// </summary>
    public void HandleRestartButtonOnClickEvent()
    {
        AudioManager.PlayButtonClickAudio();
        SceneManager.LoadScene("Scene0");
        Destroy(gameObject);
    }
}
