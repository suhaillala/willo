using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for enemy: mace
/// </summary>
public class Mace : MonoBehaviour
{
    #region Fields

    // Movement support
    Rigidbody2D rb2D;

    // Overlap area support
    float tileHeight;
    Vector2 minPoint;
    Vector2 maxPoint;
    Timer timer;

    #endregion

    #region Methods

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        // Saved for efficiency
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        timer = gameObject.AddComponent<Timer>();
        timer.Duration = 1;

        // Initializes overlap area
        InitializeOverlapArea();
        timer.AddTimerFinishedListener(DetectEnemy);
    }

    /// <summary>
    /// Sent where a collider on another object enters this object's collider
    /// </summary>
    /// <param name="coll">Collision details</param>
    void OnCollisionEnter2D(Collision2D coll)
    {
        // Movement support
        if (coll.gameObject.tag == "Platform")
        {
            rb2D.gravityScale *= -1;
        }
        else if (coll.gameObject.tag == "WorldGenerator")
        {
            // Checks overlap area for enemies
            DetectEnemy();
        }
    }

    // Saves tileHeight
    public void Initialize(float tileHeight)
    {
        this.tileHeight = tileHeight;
    }

    /// <summary>
    /// Initializes Overlap area for detecting possible collisions
    /// </summary>
    void InitializeOverlapArea()
    {
        Bounds bb = gameObject.GetComponent<BoxCollider2D>().bounds;
        Vector3 bbSize = bb.size;
        minPoint = bb.min;
        maxPoint = bb.max;

        minPoint.y = ScreenUtils.WorldBottom + tileHeight;
        maxPoint.y = minPoint.y + bbSize.y;

        minPoint.x -= bbSize.x / 2;
        maxPoint.x += bbSize.x / 2;
    }

    /// <summary>
    /// Detects enemies in the overlap area to postpone gravity reversion
    /// </summary>
    void DetectEnemy()
    {
        if (Physics2D.OverlapArea(minPoint, maxPoint, LayerMask.GetMask("Enemy")) != null)
        {
            timer.Run();
        }
        else
        {
            rb2D.gravityScale *= -1;
        }
    }

    #endregion
}
