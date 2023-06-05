using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    //TODO:需要存储
    public Dictionary<string, TileDetailsDataList> gameAllButtomTiles = new Dictionary<string, TileDetailsDataList>();
    private TileDetailsDataList currentSceneButtomTiles;
    //TODO:也需要存储
    public Dictionary<string, MiddleTileList> middleTileDict = new Dictionary<string, MiddleTileList>();
    private MiddleTileList currentSceneMiddleTiles;


    public TileDetails[] seasonTiles;
    public Vector2Int[] changeTilesPos;
    private Grid currentGrid;
    private GridMap currentGridmap;

    private void OnEnable()
    {
        EventHandler.seasonChange += OnSeasonChange;
        EventHandler.afterSceneLoadEvent += OnAfterSceneLoadEvent;
        EventHandler.hourUpdate += OnHourUpdate;
    }
    private void OnDisable()
    {
        EventHandler.seasonChange -= OnSeasonChange;
        EventHandler.afterSceneLoadEvent -= OnAfterSceneLoadEvent;
        EventHandler.hourUpdate -= OnHourUpdate;
    }
    private void Start()
    {
        currentGrid = CursorManager.Instance.currentGrid;
        currentGridmap = GridManager.Instance.gridMap;
    }
    private void OnAfterSceneLoadEvent()
    {
        string sceneName = TransitionManager.Instance.currentSceneName;
        //底部Tiles(土壤属性)的存储
        if (gameAllButtomTiles.ContainsKey(sceneName))
        {
            currentSceneButtomTiles = gameAllButtomTiles[sceneName];
        }
        else
        {
            gameAllButtomTiles.Add(sceneName, GameManager.FindObjectOfType<ButtomTileMap>().dataList);
            currentSceneButtomTiles = gameAllButtomTiles[sceneName];
        }
        //中间草坪地板的存储
        if (middleTileDict.ContainsKey(sceneName))
        {
            currentSceneMiddleTiles = middleTileDict[sceneName];
        }
        else
        {
            middleTileDict.Add(sceneName, GameManager.FindObjectOfType<ButtomTileMap>().middleTileList);
            currentSceneMiddleTiles = middleTileDict[sceneName];
        }
    }
    private void OnSeasonChange(Season season)
    {

    }
    private void OnHourUpdate()
    {
        foreach (var Tiles in changeTilesPos)
        {

        }
    }
    public TileDetailsDataList FindTilesByScene(string sceneName)
    {
        return gameAllButtomTiles[sceneName];
    }
    public TileDetails FindTileDetils(Vector2Int pos)
    {
        return currentSceneButtomTiles.gameTileDataList[pos.x][pos.y];
    }
    public TileDetails FindTileDetils(Vector3 pos)
    {
        return currentSceneButtomTiles.gameTileDataList[(int)pos.x][(int)pos.y];
    }

}

