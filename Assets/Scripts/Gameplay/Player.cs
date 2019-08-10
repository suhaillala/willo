using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Controls player behaviour
/// </summary>
public class Player : Invoker
{
    #region Fields
    // Animation support
    [SerializeField]
    Animator animator;
    [SerializeField]
    Sprite jumpUp;
    [SerializeField]
    Sprite jumpDown;

    // Movement support
    Rigidbody2D rb2D;
    float accelerationSpeed = ConfigUtils.GetPlayerConfig(ConfigItemName.AccelerationSpeed.ToString());
    float maxSpeed = ConfigUtils.GetPlayerConfig(ConfigItemName.MaxSpeed.ToString());
    float jumpDistance = ConfigUtils.GetPlayerConfig(ConfigItemName.JumpDistance.ToString());
    bool platform = false;

    // Gameplay support
    Vector3 previousPlatformPosition;

    // PlayerDead event support
    UnityEvent playerDeadEvent;

    // Power-up support
    Timer invincibilityTimer;
    Timer speedUpTimer;
    SpriteRenderer spriteRenderer;
    float powerUpDuration = ConfigUtils.GetPowerUpConfig(ConfigItemName.PowerUpDuration.ToString());
    float speedUpFactor = ConfigUtils.GetPowerUpConfig(ConfigItemName.SpeedUpFactor.ToString());

    #endregion

    #region Methods

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        // Movement support
        rb2D = gameObject.GetComponent<Rigidbody2D>();

        // Saved for efficiency
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        // Initializing PlayerDead event
        playerDeadEvent = new UnityEvent();
        EventManager.AddInvoker(EventName.PlayerDead, this);

        // Adds listener for RespawnPlayer event
        EventManager.AddListener(EventName.RespawnPlayer, RespawnPlayer);

        // Adds listener for GameOver event
        EventManager.AddListener(EventName.GameOver, GameOver);

        // Adds listener for GameWin event
        EventManager.AddListener(EventName.GameWin, GameWin);

        // Adds listener for SpeedUp power-up
        EventManager.AddListener(EventName.SpeedUp, SpeedUp);

        // Adds listener for Invincibility power-up
        EventManager.AddListener(EventName.Invincibility, Invincibility);

        // Power-up timer support
        invincibilityTimer = gameObject.AddComponent<Timer>();
        speedUpTimer = gameObject.AddComponent<Timer>();
        invincibilityTimer.AddTimerFinishedListener(StopInvincibility);
        speedUpTimer.AddTimerFinishedListener(StopSpeedUp);
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        // Main camera follows the player
        Camera.main.transform.position = new Vector3(
            CalculateCameraXPosition(transform.position.x, ScreenUtils.ScreenWidth / 2),
            Camera.main.transform.position.y, Camera.main.transform.position.z);
    }

    /// <summary>
    /// Fixed Update is called fixed number of times per second
    /// </summary>
    void FixedUpdate()
    {
        // Moves the player in a particular direction
        float movementDirection = Input.GetAxis("Horizontal");
        MovePlayer(movementDirection);

        // Makes the Player jump if standing on a platform
        if (Input.GetAxis("Jump") != 0 && platform)
        {
            rb2D.AddForce(new Vector2(0, jumpDistance),
                ForceMode2D.Impulse);
            platform = false;
            CapVelocity();
        }

        // Switches between player movement animations
        SwitchAnimation(movementDirection);
    }

    /// <summary>
    /// Moves the player in a particular direction
    /// </summary>
    /// <param name="movementDirection">direction of player movement:
    /// Positive for right, Negative for left</param>
    void MovePlayer(float movementDirection)
    {
        if (movementDirection != 0 && platform && Mathf.Abs(rb2D.velocity.x) < maxSpeed)
        {
            rb2D.AddForce(new Vector2(movementDirection * accelerationSpeed, 0),
                     ForceMode2D.Force);

            // Flips sprite based on movement direction
            if (movementDirection < 0 && !spriteRenderer.flipX)
            {
                spriteRenderer.flipX = true;
            }
            else if (movementDirection > 0 && spriteRenderer.flipX)
            {
                spriteRenderer.flipX = false;
            }
        }
    }

    /// <summary>
    /// Switches between the player movement animations
    /// </summary>
    /// <param name="movementDirection">direction of player movement:
    /// Positive for right, Negative for left</param>
    void SwitchAnimation(float movementDirection)
    {
        if (rb2D.velocity.y > 0 && !platform)
        {
            // Disables animator and changes sprite
            if (animator.enabled)
            {
                animator.enabled = false;
            }
            spriteRenderer.sprite = jumpUp;
        }
        else if (rb2D.velocity.y < 0 && !platform)
        {
            if (animator.enabled)
            {
                animator.enabled = false;
            }
            spriteRenderer.sprite = jumpDown;
        }
        else
        {
            // Re-enables animator for idle and run animation
            if (!animator.enabled)
            {
                animator.enabled = true;
            }
            animator.SetFloat("Speed", Mathf.Abs(movementDirection));
        }
    }

    /// <summary>
    /// Sent where a collider on another object enters this object's collider
    /// </summary>
    /// <param name="coll">Collision details</param>
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Platform")
        {
            // Player is standing or moving on the platform
            platform = true;

            // Saves previous platform position info if player is not jumping or falling
            if (rb2D.velocity.y == 0)
            {
                previousPlatformPosition = coll.gameObject.transform.position;
            }
        }
        // Kills player if he collides with the mace enemy
        else if (coll.gameObject.tag == "Mace")
        {
            InvokePlayerDeadEvent();
        }
    }

    /// <summary>
    /// Prevents velocity from spiking during diagonal jumps
    /// </summary>
    void CapVelocity()
    {
        Vector2 cappedVelocity = rb2D.velocity;
        if (cappedVelocity.x > maxSpeed)
        {
            cappedVelocity.x = maxSpeed;
        }
        if (cappedVelocity.y > jumpDistance)
        {
            cappedVelocity.y = jumpDistance;
        }
        rb2D.velocity = cappedVelocity;
    }

    /// <summary>
    /// Calculates the camera's x-coordinates to stay within the game world
    /// </summary>
    /// <param name="playerXPosition">Player's X coordinate</param>
    /// <param name="halfWidth">Screen's half width</param>
    /// <returns></returns>
    float CalculateCameraXPosition(float playerXPosition, float halfWidth)
    {
        // Fixes camera to the right of the player
        playerXPosition += halfWidth / 2;

        // Clamp camera to stay within the game world
        if (playerXPosition + halfWidth > ScreenUtils.WorldRight)
        {
            playerXPosition = ScreenUtils.WorldRight - halfWidth;
        }
        else if (playerXPosition - halfWidth < ScreenUtils.WorldLeft)
        {
            playerXPosition = ScreenUtils.WorldLeft + halfWidth;
        }

        return playerXPosition;
    }

    /// <summary>
    /// OnBecameInvisible is called when the player is off-screen
    /// </summary>
    void OnBecameInvisible()
    {
        // Invokes PlayerDead event if not on a platform
        if (transform.position.y < ScreenUtils.WorldBottom)
        {
            platform = false;
            InvokePlayerDeadEvent();
        }
    }

    /// <summary>
    /// Respawns player on the previous platform
    /// </summary>
    void RespawnPlayer()
    {
        StopPowerUp();
        rb2D.velocity = Vector2.zero;
        transform.position = new Vector3(previousPlatformPosition.x, 0, transform.position.z);
        Invincibility();
    }

    /// <summary>
    /// Destroys player and instantiates GameOverMenu
    /// </summary>
    void GameOver()
    {
        Destroy(gameObject);
        MenuManager.GoToMenu(MenuName.GameOver);
    }

    /// <summary>
    /// Destroys player and instantiates GameWinMenu
    /// </summary>
    void GameWin()
    {
        
        Destroy(gameObject);
        MenuManager.GoToMenu(MenuName.GameWin);
    }

    /// <summary>
    /// OnDestroy is called when the player is destroyed
    /// </summary>
    void OnDestroy()
    {
        StopPowerUp();
    }

    /// <summary>
    /// Speeds up the player
    /// </summary>
    void SpeedUp()
    {
        if (!speedUpTimer.Running)
        {
            maxSpeed *= speedUpFactor;
            speedUpTimer.Duration = powerUpDuration;
            speedUpTimer.Run();

            // Makes the player sprite turn red
            Color color = spriteRenderer.color;
            color.g = 0;
            color.b = 0;
            spriteRenderer.color = color;
        }
        else
        {
            // Adds duration to the existing timer
            speedUpTimer.AddDuration(powerUpDuration);
        }
    }

    /// <summary>
    /// Makes the player invincible
    /// </summary>
    void Invincibility()
    {
        if (!invincibilityTimer.Running)
        {
            Physics2D.IgnoreLayerCollision(9, 10, true);
            invincibilityTimer.Duration = powerUpDuration; 
            invincibilityTimer.Run();

            // Makes the player sprite semi-transparent
            Color color = spriteRenderer.color;
            color.a = 0.5f;
            spriteRenderer.color = color;
        }
        else
        {
            // Adds duration to the existing timer
            invincibilityTimer.AddDuration(powerUpDuration);
        }
    }

    /// <summary>
    /// Stops speeding up the player
    /// </summary>
    void StopSpeedUp()
    {
        maxSpeed = maxSpeed / speedUpFactor;

        // Makes the player sprite turn normal
        Color color = spriteRenderer.color;
        color.g = 1;
        color.b = 1;
        spriteRenderer.color = color;
    }

    /// <summary>
    /// Turns off invincibility
    /// </summary>
    void StopInvincibility()
    {
        Physics2D.IgnoreLayerCollision(9, 10, false);

        // Makes the player sprite opaque
        Color color = spriteRenderer.color;
        color.a = 1;
        spriteRenderer.color = color;
    }

    /// <summary>
    /// Turns off power-up
    /// </summary>
    void StopPowerUp()
    {
        if (speedUpTimer.Running)
        {
            speedUpTimer.Stop();
            StopSpeedUp();
        }

        if (invincibilityTimer.Running)
        {
            invincibilityTimer.Stop();
            StopInvincibility();
        }
    }

    /// <summary>
    /// Invokes PlayerDead event
    /// </summary>
    public void InvokePlayerDeadEvent()
    {
        playerDeadEvent.Invoke();
    }

    /// <summary>
    /// Adds listener for the PlayerDead event
    /// </summary>
    /// <param name="listener">Listener to add</param>
    public override void AddListener(EventName _ , UnityAction listener)
    {
        playerDeadEvent.AddListener(listener);
    }

    #endregion
}
