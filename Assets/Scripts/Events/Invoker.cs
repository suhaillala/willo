using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Abstract class for invokers
/// </summary>
public abstract class Invoker : MonoBehaviour
{
    public abstract void AddListener(EventName eventName, UnityAction listener);
}
