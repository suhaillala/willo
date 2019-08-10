using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Class for Screen Utilities
/// </summary>
public static class ScreenUtils
{
    #region Fields
    static float worldLeft;
    static float worldRight;
    static float worldTop;
    static float worldBottom;
    static float screenWidth;
    static float screenHeight;

    #endregion

    #region Properties
    /// <summary>
    /// Obtains left edge of the world
    /// </summary>
    /// <returns>X coordinates of the left edge</returns>
    public static float WorldLeft
    {
        get { return worldLeft; }
    }

    /// <summary>
    /// Obtains right edge of the world
    /// </summary>
    /// <returns>X coordinates of the right edge</returns>
    public static float WorldRight
    {
        get { return worldRight; }
    }

    /// <summary>
    /// Obtains top edge of the world
    /// </summary>
    /// <returns>Y coordinates of the top edge</returns>
    public static float WorldTop
    {
        get { return worldTop; }
    }

    /// <summary>
    /// Obtains bottom edge of the world
    /// </summary>
    /// <returns>Y coordinates of the bottom edge</returns>
    public static float WorldBottom
    {
        get { return worldBottom; }
    }

    /// <summary>
    /// Obtains screen width in world coordinates
    /// </summary>
    /// <returns>Screen width</returns>
    public static float ScreenWidth
    {
        get { return screenWidth; }
    }

    /// <summary>
    /// Obtains screen height in world coordinates
    /// </summary>
    /// <returns>Screen height</returns>
    public static float ScreenHeight
    {
        get { return screenHeight; }
    }

    #endregion

    #region Methods
    /// <summary>
    /// Initialize Screen Utilities
    /// </summary>
    public static void Initialize(Vector2 worldSize)
    {
        Vector3 topRightCornerScreen = Camera.main.ScreenToWorldPoint(new Vector3(
            Screen.width, Screen.height, 0));
        Vector3 bottomLeftCornerScreen = Camera.main.ScreenToWorldPoint(Vector3.zero);

        worldLeft = bottomLeftCornerScreen.x;
        worldRight = worldLeft + worldSize.x;
        worldTop = topRightCornerScreen.y;
        worldBottom = bottomLeftCornerScreen.y;

        screenWidth = topRightCornerScreen.x - bottomLeftCornerScreen.x;
        screenHeight = worldTop - worldBottom;
    }

    #endregion
}
