using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Child class for Invincibility power-up
/// </summary>
public class Invincibility : PowerUp
{
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    protected override void Start()
    {
        // Initializing SpeedUp event
        base.Start();
        EventManager.AddInvoker(EventName.Invincibility, this);
    }
}
