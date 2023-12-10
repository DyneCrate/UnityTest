using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
enum TileType
{
    Plane,
    Forest
}
public class Tile
{
    public GameObject tile;
    public bool isEmpty;
    public int playerIdxOnTile;
    private TileType tileType;
    public Color origColor;
    public Color currColor;

    internal TileType TileType { get => tileType; set => tileType = value; }
}



public class TilesMap : MonoBehaviour
{

    public static TilesMap Instance { get; private set; }

    [SerializeField] private GameObject fieldPrefab = null;
    [SerializeField] private GameObject forestPrefab = null;
    public static int totalColomns = 8;
    public static int totalRows = 8;

    [SerializeField] private Tile[] tilesArr;
    private float startingX;
    private float startingY;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        tilesArr = new Tile[totalRows * totalColomns];
        CreateTileMap();
        //CreateTile();
    }
        /// <summary>
        /// Create the map by initiating all the tiles.
        /// </summary>
        private void CreateTileMap()
    {
        GameObject tilePrefab = fieldPrefab;
        Vector3 tileScale = tilePrefab.transform.localScale;
        tileScale = new Vector3(1.75f, 1.5f, 0);
        float tileX, tileY;

        Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        startingX =  (screenBounds.x - (tileScale.x * totalColomns)) / 2.0f;
        startingY = 0;// (screenBounds.y - (tileScale.z * totalRows)) / 2.0f;

        ColorUtility.TryParseHtmlString("#A6A6A", out Color color);

        for (int row = 0; row < totalRows; row++)
        {
            for (int col = 0; col < totalColomns; col++)
            {
                tileX = startingX + (col* tileScale.x) + (row%2)*0.875f;
                tileY = startingY + (row* tileScale.y);

                int currentTileIdx = (row * totalRows) + col;

                if ( row > 3 && row < 6 )
                {
                    if ( col < 4 )
                    {
                        tilePrefab = forestPrefab;
                    }
                    else
                    {
                        tilePrefab = fieldPrefab;
                    }
                }

                tilesArr[currentTileIdx] = new Tile
                {
                    tile = Instantiate(tilePrefab, new Vector3(tileX, tileY, 0), Quaternion.identity)
                };
                tilesArr[currentTileIdx].tile.transform.SetParent(this.transform);
                tilesArr[currentTileIdx].tile.name = currentTileIdx.ToString();
                tilesArr[currentTileIdx].isEmpty = true;
                //tilesArr[currentTileIdx].tile.AddComponent<MeshCollider>();

                if ((col + row) % 2 == 0)
                {
                    SetTileColor(currentTileIdx, color);
                }
                tilesArr[currentTileIdx].origColor = tilesArr[currentTileIdx].tile.GetComponent<MeshRenderer>().material.color;                
            }
        }
    }

    /// <summary>
    /// Update the given tile index to hold the given player's index.
    /// </summary>
    public void UpdateTile(int tileIdx, int playerIdx)
    {
        tilesArr[tileIdx].playerIdxOnTile = playerIdx;
    }

    /// <summary>
    /// Update the given tile index's empty status.
    /// </summary>
    public void UpdateTile(int tileIdx, bool isEmpty)
    {
        tilesArr[tileIdx].isEmpty = isEmpty;
    }

    /// <summary>
    /// Set the current color of the tile at the given tile index with the given color.
    /// </summary>
    public void SetTileColor(int tileIdx, Color colorToSet)
    {
       // tilesArr[tileIdx].currColor = colorToSet;
        tilesArr[tileIdx].tile.GetComponent<MeshRenderer>().material.color = colorToSet;
    }

    /// <summary>
    /// Set the current color of the tile at the given tile index with the original color of that tile.
    /// </summary>
    public void SetTileOrigColor(int tileIdx)
    {
        SetTileColor(tileIdx, tilesArr[tileIdx].origColor);
    }

    /// <summary>
    /// Getter for the tile at the given tile index.
    /// </summary>
    public Tile GetTile(int tileIdx)
    {
        return tilesArr[tileIdx];
    }


    private void CreateTile()
    {
        GameObject tilePrefab = forestPrefab;
        Vector3 tileScale = tilePrefab.transform.localScale;
        float tileX, tileY;

        Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        startingX = 0;
        startingY = 0;
        ColorUtility.TryParseHtmlString("#A6A6A", out Color color);

        int row = 0;
        int col = 0;

        tileX = 4;
        tileY = 4;

        int currentTileIdx = (row * totalRows) + col;
        tilesArr[currentTileIdx] = new Tile
        {
            tile = Instantiate(tilePrefab, new Vector3(tileX, tileY, 0), Quaternion.identity)
        };

        tilesArr[currentTileIdx].tile.transform.SetParent(this.transform);
        tilesArr[currentTileIdx].tile.name = currentTileIdx.ToString();
        tilesArr[currentTileIdx].isEmpty = true;
        tilesArr[currentTileIdx].tile.AddComponent<MeshCollider>();
    }
}
