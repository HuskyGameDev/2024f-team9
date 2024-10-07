using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateLevel : MonoBehaviour
{
    [Tooltip("How many tiles does this generator create?"), Min(1)]
    public int tiles = 1;
    [Tooltip("How far in the X direction do you want to generate the map.")]
    public int tileSizeX = 18;
    [Tooltip("How far in the Y direction do you want to generate the map.")]
    public int tileSizeY = 10;

    [Header("Background")]
    public Tilemap backgroundMap;
    public Tile[] backgroundTiles;

    [Header("Collision Plane")]
    public Tilemap collisionPlane;
    public Tile[] collisionTiles;

    [Header("Middleground")]
    [Range(0, 1)]
    public float middlegroundFill = .063f;
    public Tilemap middlegroundMap;
    public Tile[] middlegroundTiles;

    [Header("Detail")]
    [Range(0, 1)]
    public float detailFill = .125f;
    public Tilemap detailMap;
    public Tile[] detailTiles;

    [Header("Foreground")]
    [Range(0, 1)]
    public float ForegroundFill = 0.031f;
    public Tilemap ForegroundMap;
    public Tile[] ForegroundTiles;

    private void OnValidate()
    {
        if (backgroundMap == null) Debug.LogError($"Background Map not assigned.");
        else if (backgroundTiles.Length < 1) Debug.LogError($"Background Tiles missing atleast one tile to fill with.");
        if (collisionPlane == null) Debug.LogError($"Collision Plane not assigned.");
        if (middlegroundMap == null) Debug.LogError($"Middleground Map not assigned.");
        if (detailMap == null) Debug.LogError($"Detail Map not assigned.");
        if (ForegroundMap == null) Debug.LogError($"Foreground Map not assigned.");
    }

    // Start is called before the first frame update
    void Start()
    {
        // clear garabge from tilemaps
        backgroundMap.ClearAllTiles();
        collisionPlane.ClearAllTiles();
        middlegroundMap.ClearAllTiles();
        detailMap.ClearAllTiles();
        ForegroundMap.ClearAllTiles();

        // fill background
        var backStart = new Vector3Int(-tileSizeX / 2, -tileSizeY / 2);
        var backEnd = new Vector3Int((tileSizeX * tiles) - (tileSizeX / 2) - 1, (tileSizeY - 1) / 2);
        RandFill(backgroundMap, backgroundTiles, backStart, backEnd);

        // fill top and Bottom walls
        var topStart = new Vector3Int(backStart.x, (tileSizeY - 1) / 2);
        var topEnd = new Vector3Int(backEnd.x, topStart.y);
        var bottomStart = new Vector3Int(backStart.x, -topStart.y - 1);
        var bottomEnd = new Vector3Int(backEnd.x, -topStart.y - 1);
        if (collisionTiles.Length > 0)
        {
            RandFill(collisionPlane, collisionTiles, topStart, topEnd);
            RandFill(collisionPlane, collisionTiles, bottomStart, bottomEnd);
        }


        // fill Middle ground if there are tiles
        if (middlegroundTiles.Length > 0)
            RandFill(middlegroundMap, middlegroundTiles, backStart, backEnd, false, middlegroundFill);

        // fill details if there are tiles
        if (detailTiles.Length > 0)
            RandFill(detailMap, detailTiles, backStart, backEnd, false, detailFill);

        // fill Foreground if there are tiles
        if (ForegroundTiles.Length > 0)
            RandFill(ForegroundMap, ForegroundTiles, backStart, backEnd, false, ForegroundFill);
    }

    public void BoxFill(Tilemap map, TileBase tile, Vector3Int start, Vector3Int end)
    {
        //Determine directions on X and Y axis
        var xDir = start.x < end.x ? 1 : -1;
        var yDir = start.y < end.y ? 1 : -1;
        //How many tiles on each axis?
        int xCols = 1 + Mathf.Abs(start.x - end.x);
        int yCols = 1 + Mathf.Abs(start.y - end.y);
        //Start painting
        for (var x = 0; x < xCols; x++)
        {
            for (var y = 0; y < yCols; y++)
            {
                var tilePos = start + new Vector3Int(x * xDir, y * yDir, 0);
                map.SetTile(tilePos, tile);
            }
        }
    }

    public void RandFill(Tilemap map, TileBase[] tile, Vector3Int start, Vector3Int end)
    {
        //Determine directions on X and Y axis
        var xDir = start.x < end.x ? 1 : -1;
        var yDir = start.y < end.y ? 1 : -1;
        //How many tiles on each axis?
        int xCols = 1 + Mathf.Abs(start.x - end.x);
        int yCols = 1 + Mathf.Abs(start.y - end.y);
        //Start painting
        for (var x = 0; x < xCols; x++)
        {
            for (var y = 0; y < yCols; y++)
            {
                var tilePos = start + new Vector3Int(x * xDir, y * yDir, 0);
                map.SetTile(tilePos, tile[Random.Range(0, tile.Length)]);
            }
        }
    }
    public void RandFill(Tilemap map, TileBase[] tile, Vector3Int start, Vector3Int end, bool completeFill = true, float fillChance = 1f)
    {
        //Determine directions on X and Y axis
        var xDir = start.x < end.x ? 1 : -1;
        var yDir = start.y < end.y ? 1 : -1;
        //How many tiles on each axis?
        int xCols = 1 + Mathf.Abs(start.x - end.x);
        int yCols = 1 + Mathf.Abs(start.y - end.y);
        //Start painting
        for (var x = 0; x < xCols; x++)
        {
            for (var y = 0; y < yCols; y++)
            {
                if (completeFill)
                {
                    var tilePos = start + new Vector3Int(x * xDir, y * yDir, 0);
                    map.SetTile(tilePos, tile[Random.Range(0, tile.Length)]);
                }
                else
                {
                    var randChance = Random.Range(0f, 1.0f);
                    if (randChance < fillChance)
                    {
                        var tilePos = start + new Vector3Int(x * xDir, y * yDir, 0);
                        map.SetTile(tilePos, tile[Random.Range(0, tile.Length)]);
                    }
                }

            }
        }
    }
}
