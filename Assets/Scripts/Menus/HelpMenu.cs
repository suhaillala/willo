using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMenu : MonoBehaviour
{
    /// <summary>
    /// Goes to the MainMenu
    /// </summary>
    public void HandleBackButtonOnClickEvent()
    {
        AudioManager.PlayButtonClickAudio();
        MenuManager.GoToMenu(MenuName.Main);
    }
}
