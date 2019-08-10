using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls behaviour of MainMenu buttons
/// </summary>
public class MainMenu : MonoBehaviour
{
    #region Fields

    // Support for sound button
    [SerializeField]
    GameObject soundButton;
    [SerializeField]
    Sprite soundEnabledButton;
    [SerializeField]
    Sprite soundEnabledButtonHighlighted;
    [SerializeField]
    Sprite soundDisabledButton;
    [SerializeField]
    Sprite soundDisabledButtonHighlighted;

    #endregion

    #region Methods

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        // Changes sprite for the SoundButton
        if (AudioManager.IsEnabled)
        {
            ChangeSoundButtonSprite(soundEnabledButton, soundEnabledButtonHighlighted);
        }
        else
        {
            ChangeSoundButtonSprite(soundDisabledButton, soundDisabledButtonHighlighted);
        }
    }

    /// <summary>
    /// Starts the game
    /// </summary>
    public void HandlePlayButtonOnClickEvent()
    {
        PlayButtonClickSound();
        MenuManager.GoToMenu(MenuName.Difficulty);
    }

    /// <summary>
    /// Goes to the HelpMenu
    /// </summary>
    public void HandleHelpButtonOnClickEvent()
    {
        PlayButtonClickSound();
        MenuManager.GoToMenu(MenuName.Help);
    }

    /// <summary>
    /// Enables/Disables the sound
    /// </summary>
    public void HandleSoundButtonOnClickEvent()
    {
        if (AudioManager.IsEnabled)
        {
            AudioManager.Enabled = false;

            // Changes target graphic and highlighted sprite
            ChangeSoundButtonSprite(soundDisabledButton, soundDisabledButtonHighlighted);
        }
        else
        {
            AudioManager.Enabled = true;
            PlayButtonClickSound();

            // Changes target graphic and highlighted sprite
            ChangeSoundButtonSprite(soundEnabledButton, soundEnabledButtonHighlighted);
        }
    }

    /// <summary>
    /// Exits the application
    /// </summary>
    public void HandleQuitButtonOnClickEvent()
    {
        PlayButtonClickSound();
        Application.Quit();
    }

    /// <summary>
    /// Plays sound when a button is clicked
    /// </summary>
    void PlayButtonClickSound()
    {
        AudioManager.PlayButtonClickAudio();
    }

    /// <summary>
    /// Changes sprite of the sound button
    /// </summary>
    /// <param name="sprite">New sprite</param>
    /// <param name="hSprite">Highlighted version of the new sprite</param>
    void ChangeSoundButtonSprite(Sprite sprite, Sprite hSprite)
    { 
        soundButton.GetComponent<Image>().sprite = sprite;

        // Changes highlighted sprite
        SpriteState highlightedSprite = new SpriteState();
        highlightedSprite.highlightedSprite = hSprite;
        soundButton.GetComponent<Button>().spriteState = highlightedSprite;

        // Deselects the button
        GameObject.FindWithTag("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem>()
            .SetSelectedGameObject(null);
    }

    #endregion
}
