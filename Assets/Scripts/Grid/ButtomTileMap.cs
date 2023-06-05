using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
[ExecuteInEditMode]
public class ButtomTileMap : MonoBehaviour
{
    public TileDetailsDataList dataList;
    public Tilemap currentTilemap;
    public MiddleTileList middleTileList;
    private void OnEnable()
    {
        if (!Application.IsPlaying(this))
        {
            currentTilemap = GetComponent<Tilemap>();

            UpdateTileProperties();
#if UNITY_EDITOR
            if (dataList != null)
                EditorUtility.SetDirty(dataList);
#endif
        }
    }

    private void OnDisable()
    {
        if (!Application.IsPlaying(this))
        {
            currentTilemap = GetComponent<Tilemap>();

            if (dataList != null)
                dataList.gameTileDataList = null;
        }

    }

    private void UpdateTileProperties()
    {
        currentTilemap.CompressBounds();
        if (dataList == null)
        {
            return;
        }
        middleTileList.sceneName = gameObject.scene.name;
        dataList.sceneName = gameObject.scene.name;
        Vector3Int startPos = currentTilemap.cellBounds.min;
        dataList.startPos = startPos;
        Vector3Int endPos = currentTilemap.cellBounds.max;
        dataList.endPos = endPos;
        dataList.gameTileDataList = new TileList[endPos.x - startPos.x];
        middleTileList.tilePos = new List<Vector2Int>();
        for (int x = startPos.x; x < endPos.x; x++)
        {
            TileList tileList = new TileList();
            tileList.tiles = new TileDetails[endPos.y - startPos.y];
            for (int y = startPos.y; y < endPos.y; y++)
            {
                TileBase tile = currentTilemap.GetTile(new Vector3Int(x, y, 0));
                if (tile == null)
                {
                    tileList.tiles[y - startPos.y] = new TileDetails(false);
                }
                else
                {
                    middleTileList.tilePos.Add(new Vector2Int(x, y));
                    tileList.tiles[y - startPos.y] = new TileDetails(true);

                }
            }
            dataList.gameTileDataList[x - startPos.x] = tileList;
        }
    }
}
