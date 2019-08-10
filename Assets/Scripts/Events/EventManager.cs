using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An event manager
/// </summary>
public static class EventManager
{
    #region Fields

    static Dictionary<EventName, List<UnityAction>> listeners = new Dictionary<EventName, List<UnityAction>>();
    static Dictionary<EventName, List<Invoker>> invokers = new Dictionary<EventName, List<Invoker>>();

    #endregion

    #region Methods

    /// <summary>
    /// Method to add an invoker
    /// </summary>
    /// <param name="eventName">Event's name</param>
    /// <param name="invoker">Invoker to add</param>
    public static void AddInvoker(EventName eventName, Invoker invoker)
    {
        if (!invokers.ContainsKey(eventName))
        {
            invokers.Add(eventName, new List<Invoker>());
        }
        invokers[eventName].Add(invoker);

        if (listeners.ContainsKey(eventName))
        {
            foreach (UnityAction listener in listeners[eventName])
            {
                invoker.AddListener(eventName, listener);
            }
        }
    }

    /// <summary>
    /// Method to add listener
    /// </summary>
    /// <param name="eventName">Event's name</param>
    /// <param name="listener">Listener to add</param>
    public static void AddListener(EventName eventName, UnityAction listener)
    {
        if (!listeners.ContainsKey(eventName))
        {
            listeners.Add(eventName, new List<UnityAction>());
        }
        listeners[eventName].Add(listener);

        if (invokers.ContainsKey(eventName))
        {
            foreach (Invoker invoker in invokers[eventName])
            {
                invoker.AddListener(eventName, listener);
            }
        }
    }

    #endregion
}