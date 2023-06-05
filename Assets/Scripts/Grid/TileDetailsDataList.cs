using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TileDetailsDataList", menuName = "Tile/TileDetailsDataList")]
public class TileDetailsDataList : ScriptableObject
{
    public string sceneName;
    public Vector3Int startPos;
    public Vector3Int endPos;
    public TileList[] gameTileDataList;
}
[Serializable]
public class TileList
{
    public TileDetails[] tiles;
    public TileDetails this[int index]
    {
        get
        {
            return tiles[index];
        }
        set
        {
            tiles[index] = value;
        }
    }
}
