using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TileDetailsDataList", menuName = "Tile/TileDetailsDataList")]
public class TileDetailsDataList : ScriptableObject
{
    public string sceneName;
    public SerializableVector2Int startPos;
    public SerializableVector2Int endPos;
    public TileList[] gameTileDataList;
}
[Serializable]
public class TileList
{
    public TileDetails[] tiles;
    public int Length
    {
        get
        {
            return tiles.Length;
        }
    }
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
