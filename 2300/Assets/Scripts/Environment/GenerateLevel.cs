using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateLevel : MonoBehaviour
{
    [System.Serializable]
    public class DecorationLayer
    {
        [Tooltip("What percentage of Tiles do you want this decoration to fill?"), Range(0f, 1f)]
        public float decorationFill = .125f;
        public Tilemap decorationMap;
        public Tile[] decorationTiles;
    }

    [Header("Portals"), Tooltip("Do you want to use Portals for the map?")]
    public bool usePortals = true;
    [Tooltip("Portal Prefab\nRequires PortalController Component to work as expected.")]
    public GameObject portal;

    [Tooltip("How many tiles does this generator create?"), Min(1)]
    public int tiles = 1;
    [Tooltip("How far in the X direction do you want to generate the map."), Min(1)]
    public int tileSizeX = 18;
    [Tooltip("How far in the Y direction do you want to generate the map."), Min(1)]
    public int tileSizeY = 10;

    [Header("Background")]
    public Tilemap backgroundMap;
    public Tile[] backgroundTiles;

    [Header("Collision Plane")]
    public Tilemap collisionPlane;
    public Tile[] collisionTiles;

    [Header("Decoration layers")]
    public DecorationLayer[] decorationLayers;

    [Header("Radiation Layer")]
    public Tilemap radiationLayer;
    public Tile[] radiationTiles;

    private void OnValidate()
    {
        if (!backgroundMap) Debug.LogError($"GenerateLevel.cs: Background Map not assigned.");
        else if (backgroundTiles.Length < 1) Debug.LogError($"GenerateLevel.cs: Background Tiles missing atleast one tile to fill with.");
        if (!collisionPlane) Debug.LogError($"GenerateLevel.cs: Collision Plane not assigned.");
        for (int i = 0; i < decorationLayers.Length; i++) // could use foreach loop but i want to be able to tell the index that has an unassigned map ;)
            if (decorationLayers[i].decorationMap == null) Debug.LogError($"GenerateLevel.cs: Decoration Layer at Index {i} has a decoration map Unassigned.");

        if(!portal && usePortals) Debug.LogError($"GenerateLevel.cs: Portal Prefab not assigned.");
    }

    // Start is called before the first frame update
    void Start()
    {
        // clear garabge from tilemaps
        backgroundMap.ClearAllTiles();
        collisionPlane.ClearAllTiles();
        foreach (DecorationLayer l in decorationLayers)
            l.decorationMap.ClearAllTiles();

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

        // Fill decoration layers with corresponding Tiles.
        foreach (DecorationLayer l in decorationLayers)
            if (l.decorationTiles.Length > 0)
                RandFill(l.decorationMap, l.decorationTiles, backStart, backEnd, false, l.decorationFill);

        // shift the map left to place the player at dead center of the level.
        transform.position -= new Vector3(tileSizeX*(tiles-1)/2,transform.position.y,transform.position.z);

        // everything past this point is related to portals so if we aren't using them then no need to execute past this point.
        if (!usePortals) return;

        // generate Portals on each end of map.
        int portalError = 0;
        var LeftPortal = Instantiate(portal, transform);
        if (LeftPortal.TryGetComponent<PortalController>(out var lpc))
        {
            lpc.LeftPortal = true;
            lpc.tileSizeX = tileSizeX;
            lpc.tileSizeY = tileSizeY;
            lpc.transform.position = new Vector3(transform.position.x + (-tileSizeX / 2),transform.position.y,transform.position.z);
        }
        else Debug.LogError($"GenerateLevel.cs: Portal Prefab ({portal.name}) does not contain PortalController Component!\t{portalError++}");
        var rightPortal = Instantiate(portal, transform);
        if (rightPortal.TryGetComponent<PortalController>(out var rpc))
        {
            rpc.LeftPortal = false;
            rpc.tileSizeX = tileSizeX;
            rpc.tileSizeY = tileSizeY;
            rpc.transform.position = new Vector3(transform.position.x + (tileSizeX * tiles) - (tileSizeX/2), transform.position.y, transform.position.z);
        }
        else Debug.LogError($"GenerateLevel.cs: Portal Prefab ({portal.name}) does not contain PortalController Component!\t{portalError++}");

        if (portalError > 0) return; // if there are too many errors(>0) with portals there is no point to continue further.

        if (rpc && lpc)
        {
            rpc.linkedPortal = lpc;
            lpc.linkedPortal = rpc;
        }
        


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

    public void RandFill(Tilemap map, TileBase[] tile, Vector3Int start, Vector3Int end, bool completeFill = true, float fillChance = .5f)
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
