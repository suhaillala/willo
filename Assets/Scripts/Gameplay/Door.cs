using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Invokes GameWin event
/// </summary>
public class Door : Invoker
{
    UnityEvent gameWinEvent;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        // Initializes GameWin event
        gameWinEvent = new UnityEvent();
        EventManager.AddInvoker(EventName.GameWin, this);
    }

    /// <summary>
    /// Sent where a collider on another object enters this object's trigger
    /// </summary>
    /// <param name="coll">Collider details</param>
    void OnTriggerEnter2D(Collider2D coll)
    {
        // Triggers GameWin event if player reaches the door
        if (coll.tag == "Player")
        {
            AudioManager.PlayGameWonAudio();
            gameWinEvent.Invoke();
        }
    }

    /// <summary>
    /// Adds listener for the GameWin event
    /// </summary>
    /// <param name="listener">Listener to add</param>
    public override void AddListener(EventName _, UnityAction listener)
    {
        gameWinEvent.AddListener(listener);
    }
}
