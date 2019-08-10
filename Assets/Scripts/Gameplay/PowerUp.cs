using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Parent class for power-ups
/// </summary>
public class PowerUp : Invoker
{
    // Support for power-up event
    protected UnityEvent powerUpEvent;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    protected virtual void Start()
    {
        // Initializing event
        powerUpEvent = new UnityEvent();
    }

    /// <summary>
    /// Sent where a collider on another object enters this object's trigger
    /// </summary>
    /// <param name="coll">Collider details</param>
    protected void OnTriggerEnter2D(Collider2D coll)
    {
        // Triggers power-up event
        if (coll.tag == "Player")
        {
            AudioManager.PlayPowerUpAudio();
            powerUpEvent.Invoke();
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Adds listeners to the power-up events
    /// </summary>
    /// <param name="listener">Listener to add</param>
    public override void AddListener(EventName _, UnityAction listener)
    {
        powerUpEvent.AddListener(listener);
    }
}
