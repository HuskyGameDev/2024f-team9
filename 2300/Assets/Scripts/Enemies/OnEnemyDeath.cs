using UnityEngine;
using UnityEngine.Tilemaps;

public class OnEnemyDeath : MonoBehaviour
{
    private Tilemap radiationLayer;

    private TileBase radiation1;
    private TileBase radiation2;
    private TileBase radiation3;
    private TileBase radiation4;
    private TileBase radiation5;

    private void OnDisable()
    {
        if (radiationLayer == null)
        {
            return;
        }

        Vector3 position = transform.position;
        Vector3Int positionInt = Vector3Int.FloorToInt(position);
        var tile = radiationLayer.GetTile(positionInt);

        //increase radiation level
        if (tile == null)
        {
            radiationLayer.SetTile(positionInt, radiation1);
        }
        else if (tile == radiation1)
        {
            radiationLayer.SetTile(positionInt, radiation2);
        }
        else if (tile == radiation2)
        {
            radiationLayer.SetTile(positionInt, radiation3);
        }
        else if (tile == radiation3)
        {
            radiationLayer.SetTile(positionInt, radiation4);
        }
        else if (tile == radiation4)
        {
            radiationLayer.SetTile(positionInt, radiation5);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        radiationLayer = GameObject.FindGameObjectWithTag("Radiation").GetComponent<Tilemap>();

        var map = radiationLayer.GetComponentInParent<GenerateLevel>();
        radiation1 = map.radiationTiles[0];
        radiation2 = map.radiationTiles[1];
        radiation3 = map.radiationTiles[2];
        radiation4 = map.radiationTiles[3];
        radiation5 = map.radiationTiles[4];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
