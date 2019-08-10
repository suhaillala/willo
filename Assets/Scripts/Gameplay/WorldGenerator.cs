using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Class for randomly generating worlds
/// </summary>
public class WorldGenerator : MonoBehaviour
{
    #region Fields
    [SerializeField]
    GameObject tileGameObject;

    [SerializeField]
    GameObject mace;

    [SerializeField]
    GameObject lizard;

    [SerializeField]
    GameObject door;

    [SerializeField]
    GameObject lifePack;

    [SerializeField]
    GameObject invincibility;

    [SerializeField]
    GameObject speedUp;

    [SerializeField]
    GameObject waterTile;

    // Tilemap support
    Tilemap tilemap;
    Tile tile;
    int worldTileCount;
    int maxTiles;

    // Game difficulty
    DifficultyName difficulty;

    #endregion

    #region Methods
    /// <summary>
    /// Awake is called before Start
    /// </summary>
    void Awake()
    {
        // Fetch game's difficulty
        difficulty = ConfigUtils.Difficulty;

        // Generates world background
        SpriteRenderer bgSpriteRenderer = GameObject.FindWithTag("Background").GetComponent<SpriteRenderer>();
        bgSpriteRenderer.size = new Vector2(ConfigUtils.GetWorldConfig(
            difficulty.ToString(), ConfigItemName.WorldSize.ToString()) * bgSpriteRenderer.size.x, 
            bgSpriteRenderer.size.y);

        // Initialize ScreenUtils
        ScreenUtils.Initialize(bgSpriteRenderer.size);

        // Initialize Tilemap
        InitializeTilemap();
    }

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        // Generates game world
        GenerateWorld();

        // Attach edge collider to world's end
        AttachEdgeCollider(ScreenUtils.WorldRight, ScreenUtils.WorldTop,
            ScreenUtils.WorldRight, ScreenUtils.WorldBottom, false);

        // Attach top collider
        AttachEdgeCollider(ScreenUtils.WorldLeft, ScreenUtils.WorldTop,
            ScreenUtils.WorldRight, ScreenUtils.WorldTop, false);
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        // Pause game if ESC is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MenuManager.GoToMenu(MenuName.Pause);
        }
    }

    /// <summary>
    /// Initializes the tilemap
    /// </summary>
    void InitializeTilemap()
    {
        tilemap = GameObject.FindWithTag("Tilemap").GetComponent<Tilemap>();
        maxTiles = tilemap.WorldToCell(new Vector3(ScreenUtils.WorldRight, ScreenUtils.WorldBottom, 0)).x + 1;

        tilemap.size = new Vector3Int(maxTiles, tilemap.size.y, tilemap.size.z); 
        tilemap.tileAnchor = new Vector3(tilemap.cellBounds.xMin + 0.5f,
           tilemap.cellBounds.yMin + 0.5f, tilemap.tileAnchor.z);

        tile = ScriptableObject.CreateInstance<Tile>();
        tile.gameObject = tileGameObject;
    }

    /// <summary>
    ///  Attach edge collider
    /// </summary>
    void AttachEdgeCollider(float x1, float y1, float x2, float y2, bool trigger)
    {
        EdgeCollider2D ec2D = gameObject.AddComponent<EdgeCollider2D>();
        Vector2[] points = new Vector2[2];
        points[0] = new Vector2(x1, y1);
        points[1] = new Vector2(x2, y2);
        ec2D.points = points;
        ec2D.isTrigger = trigger;
    }

    /// <summary>
    /// Populates the world
    /// </summary>
    void GenerateWorld()
    {
        float tileWidth = tile.gameObject.GetComponent<BoxCollider2D>().size.x;

        // Initialize water tile
        Tile waterTile = ScriptableObject.CreateInstance<Tile>();
        waterTile.gameObject = this.waterTile;
        bool isWaterTile = false;

        // Generates start tiles
        GenerateTiles(2, tile);

        // Generates middle tiles and enemies
        while (worldTileCount < maxTiles - 2)
        {
            float randomNumber = Random.value;
            if (randomNumber < ConfigUtils.GetWorldConfig(difficulty.ToString(), 
                ConfigItemName.WaterProb.ToString()) && !isWaterTile)
            {
                // Attach trigger at the end of normal tiles
                Vector2 pos = tilemap.GetCellCenterWorld(new Vector3Int(worldTileCount - 1, 0, 0));
                AttachEdgeCollider(pos.x + tileWidth / 2, pos.y,
                    pos.x + tileWidth / 2, pos.y + tileWidth * 1.5f, true);

                // Generates water tile
                GenerateTiles(Mathf.Max(1, Mathf.RoundToInt(randomNumber * 10)), waterTile);
                isWaterTile = true;

                // Attach trigger at the start of normal tiles
                pos = tilemap.GetCellCenterWorld(new Vector3Int(worldTileCount - 1, 0, 0));
                AttachEdgeCollider(pos.x + tileWidth / 2, pos.y,
                    pos.x + tileWidth / 2, pos.y + tileWidth * 1.5f, true);
            }
            else
            {
                isWaterTile = false;
                if (randomNumber >= ConfigUtils.GetWorldConfig(difficulty.ToString(), 
                    ConfigItemName.WaterProb.ToString()) && 
                    randomNumber < ConfigUtils.GetWorldConfig(difficulty.ToString(), 
                    ConfigItemName.WaterProb.ToString()) + ConfigUtils.GetWorldConfig(
                        difficulty.ToString(), ConfigItemName.LizardProb.ToString()))
                {
                    // Generates lizard enemy
                    GenerateTiles(2, tile);
                    InstantiateGameObjectAbovePreviousTile(lizard, 
                        1.05f * lizard.GetComponent<Lizard>().Radius + tileWidth / 2);
                }
                else if (randomNumber >= 1 - ConfigUtils.GetWorldConfig(
                    difficulty.ToString(), ConfigItemName.MaceProb.ToString()))
                {
                    // Generates mace enemy and saves tileWidth in the instantiated object
                    GenerateTiles(1, tile);
                    InstantiateGameObjectAbovePreviousTile(mace, tileWidth)
                        .GetComponent<Mace>().Initialize(tileWidth);
                }
                else
                {
                    // Generates normal tiles
                    GenerateTiles(1, tile);

                    // Generates power-ups
                    GeneratePowerUps(tileWidth / 2);
                }
            }
        }
        // Generates end tiles
        GenerateTiles(2, tile);

        // Instantiates end objective
        InstantiateGameObjectAbovePreviousTile(door, 0.01f + tileWidth / 2);

        tilemap.CompressBounds();
    }

    /// <summary>
    /// Generates Tiles
    /// </summary>
    /// <param name="number">Number of tiles to generate</param>
    void GenerateTiles(int number, Tile tile)
    {
        tilemap.BoxFill(new Vector3Int(worldTileCount, 0, 0), tile, worldTileCount, 0,
                worldTileCount + number - 1, 0);
        worldTileCount += number;
    }

    /// <summary>
    /// Generates power-ups above previous tile
    /// </summary>
    /// <param name="yOffset">Y offset above previous tile center</param>
    void GeneratePowerUps(float yOffset)
    {
        // Generates power-ups based on given probabilities
        if (Random.value < ConfigUtils.GetWorldConfig(difficulty.ToString(),
            ConfigItemName.PowerUpProb.ToString()))
        {
            float randomNumber = Random.value;

            // Generates life pack
            if (randomNumber >= 1 - ConfigUtils.GetPowerUpConfig(ConfigItemName.LifePackProb.ToString()))
            {
                InstantiateGameObjectAbovePreviousTile(lifePack, yOffset);
            }

            // Generates invincibility power-up
            else if (randomNumber < ConfigUtils.GetPowerUpConfig(ConfigItemName.InvincibilityProb.ToString()))
            {
                InstantiateGameObjectAbovePreviousTile(invincibility, yOffset);
            }

            // Generates SpeedUp power-up
            else
            {
                InstantiateGameObjectAbovePreviousTile(speedUp, yOffset);
            }
        }
    }

    /// <summary>
    /// Instantiates a GameObject above the previous tile
    /// </summary>
    /// <param name="gameObject">GameObject to instantiate</param>
    /// <param name="yOffset">Y offset from centre of the previous tile</param>
    GameObject InstantiateGameObjectAbovePreviousTile(GameObject gameObject, float yOffset)
    {
        Vector3 gameObjectWorldCoordinates = tilemap.GetCellCenterWorld(
            new Vector3Int(worldTileCount - 1, 0, 0));
        gameObjectWorldCoordinates.y += yOffset;

        return Instantiate<GameObject>(gameObject, gameObjectWorldCoordinates, Quaternion.identity);
    }

    #endregion
}
