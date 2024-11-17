using System.Collections.Generic;
using Unity.VisualScripting;
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

    [Tooltip("How many Rooms does this generator create?"), Min(1)]
    public int rooms = 1;
    [Tooltip("How far in the X direction do you want to generate the room."), Min(1)]
    public int RoomSizeX = 18;
    [Tooltip("How far in the Y direction do you want to generate the room."), Min(1)]
    public int RoomSizeY = 10;

    [Header("Portals"), Tooltip("Do you want to use Portals for the map?")]
    public bool usePortals = true;
    [Tooltip("Portal Prefab\nRequires PortalController Component to work as expected.")]
    public GameObject portal;

    [Header("Objectives")]
    public bool useObjectives = true;
    [Tooltip("How many Objectives do you want to spawn on the map?"),Min(1)]
    public int numObjectives = 2;
    [Tooltip("Prefabs referencing potential Objectives")]
    public GameObject[] Objectives;
    private List<GameObject> _activeObjectives;

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
        // editor error checking.
        if (!backgroundMap) Debug.LogError($"GenerateLevel.cs: Background Map not assigned.");
        else if (backgroundTiles.Length < 1) Debug.LogError($"GenerateLevel.cs: Background Tiles missing atleast one tile to fill with.");
        if (!collisionPlane) Debug.LogError($"GenerateLevel.cs: Collision Plane not assigned.");
        for (int i = 0; i < decorationLayers.Length; i++) // could use foreach loop but i want to be able to tell the index that has an unassigned map
            if (decorationLayers[i].decorationMap == null) Debug.LogError($"GenerateLevel.cs: Decoration Layer at Index {i} has a decoration map Unassigned.");

        if (!portal && usePortals) Debug.LogError($"GenerateLevel.cs: Portal Prefab not assigned.");
        if (Objectives.Length == 0 && useObjectives) Debug.LogError($"GenerateLevel.cs: Objective Prefab(s) not assigned.");

        if (numObjectives > rooms) numObjectives = rooms;
    }

    // Start is called before the first frame update
    void Start()
    {
        _activeObjectives = new List<GameObject>();

        // clear garabge from tilemaps
        backgroundMap.ClearAllTiles();
        collisionPlane.ClearAllTiles();
        radiationLayer.ClearAllTiles();
        foreach (DecorationLayer l in decorationLayers)
            l.decorationMap.ClearAllTiles();

        // fill background
        var backStart = new Vector3Int(-RoomSizeX / 2, -RoomSizeY / 2);
        var backEnd = new Vector3Int((RoomSizeX * rooms) - (RoomSizeX / 2) - 1, (RoomSizeY - 1) / 2);

        // shift the map left to place the player at dead center of the level.
        backStart -= new Vector3Int(RoomSizeX * (rooms - 1) / 2, 0);
        backEnd -= new Vector3Int(RoomSizeX * (rooms - 1) / 2, 0);

        RandFill(backgroundMap, backgroundTiles, backStart * 2, backEnd * 2);

        // fill top and Bottom walls
        var topStart = new Vector3Int(backStart.x * 2, backEnd.y + 1);
        var topEnd = new Vector3Int(backEnd.x * 2, topStart.y);
        var bottomStart = new Vector3Int(backStart.x * 2, -topStart.y - 1);
        var bottomEnd = new Vector3Int(backEnd.x * 2, bottomStart.y);
        if (collisionTiles.Length > 0)
        {
            RandFill(collisionPlane, collisionTiles, topStart, topEnd);
            RandFill(collisionPlane, collisionTiles, bottomStart, bottomEnd);
        }

        var decoStart = new Vector3Int(backStart.x, bottomStart.y + 1);
        var decoEnd = new Vector3Int(backEnd.x, topStart.y - 1);

        // Fill decoration layers with corresponding Tiles.
        foreach (DecorationLayer l in decorationLayers)
            if (l.decorationTiles.Length > 0)
                RandFill(l.decorationMap, l.decorationTiles, decoStart, decoEnd, l.decorationFill >= 1, l.decorationFill);

        // Now you're thinking with portals!
        if (usePortals)
        {
            // its dirty but just duplicate the decoration layers and offset them by the level so it creates the illusion of an "infinite" level.
            foreach (DecorationLayer l in decorationLayers)
            {
                var lef = Instantiate(l.decorationMap);
                var rig = Instantiate(l.decorationMap);
                lef.transform.SetParent(gameObject.transform);
                lef.transform.position = new Vector3(backStart.x * 2, 0);
                rig.transform.SetParent(gameObject.transform);
                rig.transform.position = new Vector3(backEnd.x * 2, 0);
            }

            // generate Portals on each end of map.
            int portalError = 0;
            var LeftPortal = Instantiate(portal, transform);
            if (LeftPortal.TryGetComponent<PortalController>(out var lpc))
            {
                lpc.LeftPortal = true;
                var a = new Vector3Int(0, topStart.y, 0);
                var b = new Vector3Int(0, bottomStart.y, 0);
                var height = Vector3Int.Distance(a, b);
                lpc.tileSizeY = (int)height;
                //lpc.transform.position = new Vector3(backStart.x + (-tileSizeX / 2), transform.position.y, transform.position.z);
                lpc.transform.position = new Vector3((-RoomSizeX / 2) + 0.5f - ((RoomSizeX * (rooms - 1)) / 2), transform.position.y, transform.position.z);
            }
            else Debug.LogError($"GenerateLevel.cs: Portal Prefab ({portal.name}) does not contain PortalController Component!\t{portalError++}");
            var rightPortal = Instantiate(portal, transform);
            if (rightPortal.TryGetComponent<PortalController>(out var rpc))
            {
                rpc.LeftPortal = false;
                var a = new Vector3Int(0, topStart.y, 0);
                var b = new Vector3Int(0, bottomStart.y, 0);
                var height = Vector3Int.Distance(a, b);
                rpc.tileSizeY = (int)height;
                //rpc.transform.position = new Vector3(backStart.x + (tileSizeX * tiles) - (tileSizeX / 2), transform.position.y, transform.position.z);
                rpc.transform.position = new Vector3(((RoomSizeX * rooms) - (RoomSizeX / 2) - .5f) - ((RoomSizeX * (rooms - 1)) / 2), transform.position.y, transform.position.z);
            }
            else Debug.LogError($"GenerateLevel.cs: Portal Prefab ({portal.name}) does not contain PortalController Component!\t{portalError++}");

            if (portalError > 0) return; // if there are too many errors(>0) with portals there is no point to continue further.

            if (rpc && lpc)
            {
                rpc.linkedPortal = lpc;
                lpc.linkedPortal = rpc;
            }
        }

        if (useObjectives)
        {
            List<int> takenRooms = new List<int>();
            for (int i = 0; i < numObjectives; i++)
            {
                int room = Random.Range(1, rooms + 1);
                while (takenRooms.Contains(room)) // could freeze game hopefully it doesn't 
                    room = Random.Range(1, rooms + 1);

                var roomStartX = ((-RoomSizeX / 2) + ((room - 1) * RoomSizeX)) - (RoomSizeX * (rooms-1)/2); // room start - level offset
                var roomEndX = ((RoomSizeX * room) - (RoomSizeX / 2) - 1) - (RoomSizeX * (rooms - 1) / 2); // room end - level offest

                var position = new Vector3Int((int)Mathf.Lerp(roomStartX + 3, roomEndX - 3, Random.value), // x
                                              (int)Mathf.Lerp(backStart.y, backEnd.y, Random.value)); // y (either on the top or bottom of the map)

                var prefab = Objectives[Random.Range(0, Objectives.Length)]; // Objective

                var obj = Instantiate(prefab, position, transform.rotation,transform);
                _activeObjectives.Add(obj); // object pooling maybe?
                takenRooms.Add(room); // prevents multiple objectives in a single room.
            }
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
                    if (randChance <= fillChance)
                    {
                        var tilePos = start + new Vector3Int(x * xDir, y * yDir, 0);
                        map.SetTile(tilePos, tile[Random.Range(0, tile.Length)]);
                    }
                }

            }
        }
    }
}
