using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ConfigData
{
    #region Fields

    const string worldConfigFilename = "Config-World.csv";
    const string playerConfigFilename = "Config-Player.csv";
    const string powerUpConfigFilename = "Config-PowerUp.csv";

    // Default world configuration data
    Dictionary<string, Dictionary<string, float>> worldConfig =
        new Dictionary<string, Dictionary<string, float>>()
        {
            { DifficultyName.Easy.ToString(), new Dictionary<string, float>
            {
                {ConfigItemName.WorldSize.ToString(), 6},
                {ConfigItemName.WaterProb.ToString(), 0.2f},
                {ConfigItemName.LizardProb.ToString(), 0.1f},
                {ConfigItemName.MaceProb.ToString(), 0.1f},
                {ConfigItemName.PowerUpProb.ToString(), 0.3f}
            }
            },
            { DifficultyName.Medium.ToString(), new Dictionary<string, float>
            {
                {ConfigItemName.WorldSize.ToString(), 10},
                {ConfigItemName.WaterProb.ToString(), 0.28f},
                {ConfigItemName.LizardProb.ToString(), 0.2f},
                {ConfigItemName.MaceProb.ToString(), 0.2f},
                {ConfigItemName.PowerUpProb.ToString(), 0.2f}
            }
            },
            { DifficultyName.Hard.ToString(), new Dictionary<string, float>
            {
                {ConfigItemName.WorldSize.ToString(), 14},
                {ConfigItemName.WaterProb.ToString(), 0.34f},
                {ConfigItemName.LizardProb.ToString(), 0.26f},
                {ConfigItemName.MaceProb.ToString(), 0.3f},
                {ConfigItemName.PowerUpProb.ToString(), 0.1f}
            }
            }
        };


    // Default player data
    Dictionary<string, float> playerConfig = new Dictionary<string, float>()
    {
        {ConfigItemName.AccelerationSpeed.ToString(), 47},
        {ConfigItemName.MaxSpeed.ToString(), 6.1f},
        {ConfigItemName.JumpDistance.ToString(), 6.1f},
        {ConfigItemName.Life.ToString(), 3}
    };

    // Default difficulty
    DifficultyName difficulty = DifficultyName.Easy;

    // Default power-up data
    Dictionary<string, float> powerUpConfig = new Dictionary<string, float>()
    {
        {ConfigItemName.PowerUpDuration.ToString(), 7},
        {ConfigItemName.LifePackProb.ToString(), 0.3f},
        {ConfigItemName.SpeedUpFactor.ToString(), 1.7f},
        {ConfigItemName.SpeedUpProb.ToString(), 0.35f},
        {ConfigItemName.InvincibilityProb.ToString(), 0.35f}
    };

    #endregion

    #region Properties

    /// <summary>
    /// Sets the game's difficulty
    /// </summary>
    public DifficultyName Difficulty
    {
        get { return difficulty; }
        set { difficulty = value; }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Reads configuration data from a file. If the file read fails, 
    /// the object contains default values for the configuration data
    /// </summary>
    public ConfigData()
    {
        StreamReader worldConfig = null;
        StreamReader playerConfig = null;
        StreamReader powerUpConfig = null;

        try
        {
            worldConfig = File.OpenText(Path.Combine(Application.streamingAssetsPath, worldConfigFilename));
            playerConfig = File.OpenText(Path.Combine(Application.streamingAssetsPath, playerConfigFilename));
            powerUpConfig = File.OpenText(Path.Combine(Application.streamingAssetsPath, powerUpConfigFilename));

            // Allocates configuration data to their respective fields
            PopulateWorldConfig(worldConfig);
            PopulatePlayerConfig(playerConfig);
            PopulatePowerUpConfig(powerUpConfig);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        finally
        {
            if (worldConfig != null)
            {
                worldConfig.Close();
            }

            if (playerConfig != null)
            {
                playerConfig.Close();
            }

            if (powerUpConfig != null)
            {
                powerUpConfig.Close();
            }
        }
    }

    /// <summary>
    /// Allocates world configuration data
    /// </summary>
    /// <param name="configData">Input</param>
    void PopulateWorldConfig(StreamReader configData)
    {
        string line = configData.ReadLine();
        while (line != null)
        {
            string[] values = line.Split(',');

            // Allocates values for easy difficulty
            if (worldConfig[DifficultyName.Easy.ToString()].ContainsKey(values[0]))
            {
                worldConfig[DifficultyName.Easy.ToString()][values[0]] = float.Parse(values[1]);
            }

            // Allocates values for medium difficulty
            if (worldConfig[DifficultyName.Medium.ToString()].ContainsKey(values[0]))
            {
                worldConfig[DifficultyName.Medium.ToString()][values[0]] = float.Parse(values[2]);
            }

            // Allocates values for hard difficulty
            if (worldConfig[DifficultyName.Hard.ToString()].ContainsKey(values[0]))
            {
                worldConfig[DifficultyName.Hard.ToString()][values[0]] = float.Parse(values[3]);
            }

            line = configData.ReadLine();
        }
    }

    /// <summary>
    /// Allocates player configuration data
    /// </summary>
    /// <param name="configData">Input</param>
    void PopulatePlayerConfig(StreamReader configData)
    {
        string line = configData.ReadLine();
        while (line != null)
        {
            string[] values = line.Split(',');
            if (playerConfig.ContainsKey(values[0]))
            {
                playerConfig[values[0]] = float.Parse(values[1]);
            }
            line = configData.ReadLine();
        }
    }

    /// <summary>
    /// Allocates power-up configuration data
    /// </summary>
    /// <param name="configData">Input</param>
    void PopulatePowerUpConfig(StreamReader configData)
    {
        string line = configData.ReadLine();
        while (line != null)
        {
            string[] values = line.Split(',');
            if (powerUpConfig.ContainsKey(values[0]))
            {
                powerUpConfig[values[0]] = float.Parse(values[1]);
            }
            line = configData.ReadLine();
        }
    }

    /// <summary>
    /// Fetches world configuration data
    /// </summary>
    /// <param name="difficulty">Game difficulty</param>
    /// <param name="configItem">Configuration item</param>
    /// <returns>Value associated with the configuration item</returns>
    public float GetWorldConfig(string difficulty, string configItem)
    {
        return worldConfig[difficulty][configItem];
    }

    /// <summary>
    /// Fetches player configuration data
    /// </summary>
    /// <param name="configItem">Configuration item</param>
    /// <returns>Value associated with the configuration item</returns>
    public float GetPlayerConfig(string configItem)
    {
        return playerConfig[configItem];
    }

    /// <summary>
    /// Fetches power-up configuration data
    /// </summary>
    /// <param name="configItem">Configuration item</param>
    /// <returns>Value associated with the configuration item</returns>
    public float GetPowerUpConfig(string configItem)
    {
        return powerUpConfig[configItem];
    }

    #endregion
}
