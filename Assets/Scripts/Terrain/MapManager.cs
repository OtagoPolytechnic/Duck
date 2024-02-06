using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap map;

    [SerializeField]
    private List<TileData> tileDatas;

    private Dictionary<TileBase, TileData> dataFromTiles;
 
    private void awake()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach (var tileData in tileDatas)
        {
            foreach (var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }
    }

    public float GetTileWalkingSpeed(Vector2 worldPosition)
    {
        float modifier = 1;

        Vector3Int gridPosition = map.WorldToCell(worldPosition);

        TileBase tile = map.GetTile(gridPosition);

        /*
        This is kinda inefficient, however I was struggling to get it working by calling the tile from the dictionary.
        modifier = dataFromTiles[tile].moveSpeedModifier
        */
        foreach (var tileData in tileDatas)
        {
            if (tile.name == tileData.name)
            {
                modifier = tileData.moveSpeedModifier;
            }
        }

        return modifier;
    }
}
