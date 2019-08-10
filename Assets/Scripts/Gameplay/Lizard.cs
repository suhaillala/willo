using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for enemy: lizard
/// </summary>
public class Lizard : MonoBehaviour
{
    #region Fields

    [SerializeField]
    Animator animator;

    // Movement support
    Rigidbody2D rb2D;
    int speed = 2;
    int movementDirection = -1;
    SpriteRenderer spriteRenderer;

    float radius;

    #endregion

    #region Properties

    /// <summary>
    /// Fetches the collider's radius
    /// </summary>
    public float Radius
    {
        get
        {
            return radius;
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        // Saved for efficiency
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        radius = gameObject.GetComponent<CircleCollider2D>().radius;
    }

    /// <summary>
    /// Fixed Update is called fixed number of times per second
    /// </summary>
    void FixedUpdate()
    {
        // Moves the lizard in a particular direction
        rb2D.MovePosition(new Vector2(
            transform.position.x + speed * movementDirection * Time.fixedDeltaTime, transform.position.y));

        // Animates the lizard
        animator.SetFloat("Speed", Mathf.Abs(movementDirection));
    }

    /// <summary>
    /// Sent where a collider on another object enters this object's collider
    /// </summary>
    /// <param name="coll">Collision details</param>
    void OnCollisionEnter2D(Collision2D coll)
    {
        GameObject collGameObject = coll.gameObject;

        // Turns enemy around if it collides with another object
        if (collGameObject.tag == "Lizard" || collGameObject.tag == "Mace" ||
            collGameObject.tag == "WorldGenerator")
        {
            TurnAround();
        }
        else if (collGameObject.tag == "Player")
        {
            // Kills enemy if player jumps on it
            if (collGameObject.transform.position.y > transform.position.y + radius * 1.2f &&
            collGameObject.GetComponent<Rigidbody2D>().velocity.y < 0)
            {
                Destroy(gameObject);
            }
            else
            {
                // Kills the player otherwise
                collGameObject.GetComponent<Player>().InvokePlayerDeadEvent();
            }
        } 
            
    }

    /// <summary>
    /// Sent where a trigger on another object enters this object's collider
    /// </summary>
    /// <param name="coll">Collider details</param>
    void OnTriggerEnter2D(Collider2D coll)
    {
        // Turn enemy around if it collides with a trigger
        if (coll.tag == "WorldGenerator")
        {
            TurnAround();
        }
    }

    /// <summary>
    /// Turns the lizard around
    /// </summary>
    void TurnAround()
    {
        movementDirection *= -1;

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

    #endregion
}
