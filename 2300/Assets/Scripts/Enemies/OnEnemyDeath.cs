using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OnEnemyDeath : MonoBehaviour
{
    private Tilemap radiationLayer;

    //private static readonly TileBase radiation1 = Resources.Load<TileBase>("Palettes(Tiles)/Map/Radiation_Level_1");
    //private static readonly TileBase radiation2 = Resources.Load<TileBase>("Palettes(Tiles)/Map/Radiation_Level_2");
    //private static readonly TileBase radiation3 = Resources.Load<TileBase>("Palettes(Tiles)/Map/Radiation_Level_3");
    //private static readonly TileBase radiation4 = Resources.Load<TileBase>("Palettes(Tiles)/Map/Radiation_Level_4");
    //private static readonly TileBase radiation5 = Resources.Load<TileBase>("Palettes(Tiles)/Map/Radiation_Level_5");
    //private static readonly TileBase radiation6 = Resources.Load<TileBase>("Palettes(Tiles)/Map/Radiation_Level_6");

    private void OnDisable()
    {
        Vector3 position = transform.position;
        Vector3Int positionInt = Vector3Int.FloorToInt(position);
        var tile = radiationLayer.GetTile(positionInt);

        //increase radiation level
        //if (tile == null)
        //{
        //    radiationLayer.SetTile(positionInt, radiation1);
        //}
        //else if (tile == radiation1)
        //{
        //    radiationLayer.SetTile(positionInt, radiation2);
        //}
        //else if (tile == radiation2)
        //{
        //    radiationLayer.SetTile(positionInt, radiation3);
        //}
        //else if (tile == radiation3)
        //{
        //    radiationLayer.SetTile(positionInt, radiation4);
        //}
        //else if (tile == radiation4)
        //{
        //    radiationLayer.SetTile(positionInt, radiation5);
        //}
        //else if (tile == radiation5)
        //{
        //    radiationLayer.SetTile(positionInt, radiation6);
        //}
    }

    // Start is called before the first frame update
    void Start()
    {
        radiationLayer = GameObject.FindGameObjectWithTag("Radiation").GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
