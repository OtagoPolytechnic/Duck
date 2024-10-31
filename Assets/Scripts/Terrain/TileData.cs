using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TileData : ScriptableObject
{
    public TileBase[] tiles;

    public string tileType;
    public float enemyMoveSpeedModifier;
    public float playerMoveSpeedModifier;
}
