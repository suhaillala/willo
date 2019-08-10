using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A timer
/// </summary>
public class Timer : MonoBehaviour
{
	#region Fields
	
	// timer duration
	float totalSeconds = 0;
	
	// timer execution
	float elapsedSeconds = 0;
	bool running = false;
	
	// support for Finished property
	bool started = false;

    // Initialize Timer finished event
    UnityEvent timerFinished = new UnityEvent();
	
	#endregion
	
	#region Properties
	
	/// <summary>
	/// Sets the duration of the timer
	/// </summary>
	/// <value>duration</value>
	public float Duration
    {
		set
        {
			if (!running)
            {
				totalSeconds = value;
			}
		}
	}

    /// <summary>
    /// Gets whether or not the timer has finished running
    /// </summary>
    /// <value>true if finished; otherwise, false.</value>
    public bool Finished
    {
        get { return started && !running; }
    }

    /// <summary>
    /// Gets whether or not the timer is currently running
    /// </summary>
    /// <value>true if running; otherwise, false.</value>
    public bool Running
    {
		get { return running; }
	}

    /// <summary>
    /// Gets time left on the timer
    /// </summary>
    /// <value>time left if timer is running; otherwise, 0.</value>
    public float TimeLeft
    {
        get
        {
            if (running)
            {
                return totalSeconds - elapsedSeconds;
            }
            else
            {
                return 0;
            }
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {	
		// update timer and check for finished
		if (running)
        {
			elapsedSeconds += Time.deltaTime;
			if (elapsedSeconds >= totalSeconds)
            {
				running = false;

                // Invoke timer finished event
                timerFinished.Invoke();
			}
		}
	}
	
	/// <summary>
	/// Runs the timer
	/// </summary>
	public void Run()
    {	
		// only run with valid duration
		if (totalSeconds > 0)
        {
			started = true;
			running = true;
            elapsedSeconds = 0;
		}
	}

    /// <summary>
    /// Stops the timer
    /// </summary>
    public void Stop()
    {
        if (running)
        {
            totalSeconds = 0;
            running = false;
            started = false;
        }
    }

    // Add listener for timer finished event
    public void AddTimerFinishedListener(UnityAction listener)
    {
        timerFinished.AddListener(listener);
    }


    /// <summary>
    /// Extends timer by the specified duration
    /// </summary>
    /// <param name="duration">Duration to add to the timer</param>
    public void AddDuration(float duration)
    {
        if (running)
        {
            totalSeconds += duration;
        }
    }

    #endregion
}
