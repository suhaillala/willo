using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for Game Settings
/// </summary>
public static class ConfigUtils
{
    static ConfigData configData = new ConfigData();

    #region Properties

    /// <summary>
    /// Sets the game's difficulty
    /// </summary>
    public static DifficultyName Difficulty
    {
        get { return configData.Difficulty; }
        set { configData.Difficulty = value; }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Fetches world configuration data
    /// </summary>
    /// <param name="difficulty">Game difficulty</param>
    /// <param name="configItem">Configuration item</param>
    /// <returns>Value associated with the configuration item</returns>
    public static float GetWorldConfig(string difficulty, string configItem)
    {
        return configData.GetWorldConfig(difficulty, configItem);
    }

    /// <summary>
    /// Fetches player configuration data
    /// </summary>
    /// <param name="configItem">Configuration item</param>
    /// <returns>Value associated with the configuration item</returns>
    public static float GetPlayerConfig(string configItem)
    {
        return configData.GetPlayerConfig(configItem);
    }

    /// <summary>
    /// Fetches power-up configuration data
    /// </summary>
    /// <param name="configItem">Configuration item</param>
    /// <returns>Value associated with the configuration item</returns>
    public static float GetPowerUpConfig(string configItem)
    {
        return configData.GetPowerUpConfig(configItem);
    }

    #endregion
}
