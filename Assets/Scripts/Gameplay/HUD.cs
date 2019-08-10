using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Class for controlling HUD
/// </summary>
public class HUD : Invoker
{
    float life = ConfigUtils.GetPlayerConfig(ConfigItemName.Life.ToString());
    Text lifeText;

    // RespawnPlayer event support
    UnityEvent respawnPlayerEvent;

    // GameOver event support
    UnityEvent gameOverEvent;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        // Saved for efficiency
        lifeText = GameObject.FindWithTag("LifeText").GetComponent<Text>();

        // Add listener for PlayerDead event
        EventManager.AddListener(EventName.PlayerDead, ReducePlayerLife);

        // Add listener for LifePack event
        EventManager.AddListener(EventName.LifePack, AddPlayerLife);

        // Initializing RespawnPlayer event
        respawnPlayerEvent = new UnityEvent();
        EventManager.AddInvoker(EventName.RespawnPlayer, this);

        // Initializing GameOver event
        gameOverEvent = new UnityEvent();
        EventManager.AddInvoker(EventName.GameOver, this);

        // Displays player's life
        ShowPlayerLife(life);
    }

    /// <summary>
    /// Adds to player's life
    /// </summary>
    void AddPlayerLife()
    {
        life++;
        ShowPlayerLife(life);
    }

    /// <summary>
    /// Reduces player's life and respawns or destroys player
    /// </summary>
    void ReducePlayerLife()
    {
        life--;
        if (life > 0)
        {
            AudioManager.PlayLoseLifeAudio();
            respawnPlayerEvent.Invoke();
            ShowPlayerLife(life);
        }
        else
        {
            AudioManager.PlayGameOverAudio();
            gameOverEvent.Invoke();
        } 
    }

    /// <summary>
    /// Displays player's life in the HUD
    /// </summary>
    /// <param name="life">Life to display</param>
    void ShowPlayerLife(float life)
    {
        lifeText.text = "Lives: " + life.ToString();
    }

    /// <summary>
    /// Adds listeners for the corresponding events
    /// </summary>
    /// <param name="eventName">Event's name</param>
    /// <param name="listener">Listener to add</param>
    public override void AddListener(EventName eventName, UnityAction listener)
    {
        switch (eventName)
        {
            case EventName.RespawnPlayer:
                respawnPlayerEvent.AddListener(listener);
                break;

            case EventName.GameOver:
                gameOverEvent.AddListener(listener);
                break;

            default:
                Debug.Log(eventName);
                break;
        }
        
    }
}
