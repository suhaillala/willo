using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls transitions between menus
/// </summary>
public static class MenuManager
{
    public static void GoToMenu(MenuName menuName)
    {
        switch (menuName)
        {
            case MenuName.Main:
                SceneManager.LoadScene("MainMenu");
                break;

            case MenuName.Help:
                SceneManager.LoadScene("HelpMenu");
                break;

            case MenuName.Difficulty:
                SceneManager.LoadScene("DifficultyMenu");
                break;

            case MenuName.Pause:
                Object.Instantiate(Resources.Load("PauseMenu"));
                break;

            case MenuName.GameOver:
                Object.Instantiate(Resources.Load("GameOverMenu"));
                break;

            case MenuName.GameWin:
                Object.Instantiate(Resources.Load("GameWinMenu"));
                break;

            default:
                SceneManager.LoadScene("MainMenu");
                break;
        }
    }
}
