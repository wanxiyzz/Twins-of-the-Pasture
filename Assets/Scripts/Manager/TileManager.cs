using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : Singleton<TileManager>
{
    //STORED:游戏中的底部（泥土）Tile存储

    public Dictionary<string, TileDetailsDataList> gameAllButtomTiles = new Dictionary<string, TileDetailsDataList>();
    private TileDetailsDataList currentSceneButtomTiles;
    //STORED:游戏中的中部（地板）Tile存储

    public Dictionary<string, MiddleTileList> middleTileDict = new Dictionary<string, MiddleTileList>();
    public MiddleTileList currentSceneMiddleTiles;


    public TileDetails[] seasonTiles;
    public Grid currentGrid;
    private GridMap currentGridmap;

    private bool canPlant;

    private void OnEnable()
    {
        EventHandler.seasonChange += OnSeasonChange;
        EventHandler.afterSceneLoadEvent += OnAfterSceneLoadEvent;
        EventHandler.hourUpdate += OnHourUpdate;
        EventHandler.useItemEvent += OnUseItemEvent;

    }
    private void OnDisable()
    {
        EventHandler.seasonChange -= OnSeasonChange;
        EventHandler.afterSceneLoadEvent -= OnAfterSceneLoadEvent;
        EventHandler.hourUpdate -= OnHourUpdate;
        EventHandler.useItemEvent -= OnUseItemEvent;

    }
    private void Start()
    {
        currentGridmap = GridManager.Instance.gridMap;
    }

    private void OnUseItemEvent(Vector3 pos, ItemDetails itemDetails)
    {
        if (itemDetails.itemType.HaveTheType(ItemType.Seed))
        {
            if (canPlant)
            {
                var plantPos = currentGrid.WorldToCell(pos);
                EventHandler.CallPlantAPlant(plantPos, itemDetails.itemID);
                ButtomTile(plantPos).seedID = itemDetails.itemID;
            }
        }
    }

    private void OnAfterSceneLoadEvent(SceneType sceneType, string sceneName)
    {
        currentGrid = FindObjectOfType<Grid>();
        var buttomTileMap = GameManager.FindObjectOfType<ButtomTileMap>();
        //底部Tiles(土壤属性)的存储
        if (gameAllButtomTiles.ContainsKey(sceneName))
        {
            currentSceneButtomTiles = gameAllButtomTiles[sceneName];
        }
        else
        {
            canPlant = true;
            gameAllButtomTiles.Add(sceneName, buttomTileMap.dataList);
            currentSceneButtomTiles = gameAllButtomTiles[sceneName];
        }
        //中间草坪地板的存储
        if (middleTileDict.ContainsKey(sceneName))
        {
            currentSceneMiddleTiles = middleTileDict[sceneName];
        }
        else
        {
            middleTileDict.Add(sceneName, buttomTileMap.middleTileList);
            currentSceneMiddleTiles = middleTileDict[sceneName];
        }
        GridManager.Instance.ChangeSceneMiddleTiles(sceneType, currentSceneMiddleTiles);
    }
    private void OnSeasonChange(Season season)
    {

    }
    //水分蒸发 虫子等逻辑
    private void OnHourUpdate()
    {

    }
    public TileDetailsDataList FindBottomTilesByScene(string sceneName)
    {
        return gameAllButtomTiles[sceneName];
    }
    public TileDetails FindBottomTileDetils(Vector2Int pos)
    {
        return currentSceneButtomTiles.gameTileDataList[pos.x][pos.y];
    }
    public TileDetails FindBottomTileDetils(Vector3 pos)
    {
        return currentSceneButtomTiles.gameTileDataList[(int)pos.x][(int)pos.y];
    }
    public bool CheckCanPlant(Vector3Int pos)
    {
        var tileDetails = ButtomTile(pos);
        if (tileDetails == null)
            return false;
        return !tileDetails.haveTop && tileDetails.seedID < 1;
    }
    public TileDetails ButtomTile(Vector3Int pos)
    {
        if (pos.x - currentSceneButtomTiles.startPos.x >= currentSceneButtomTiles.gameTileDataList.Length
        ||
         pos.y - currentSceneButtomTiles.startPos.y >=
         currentSceneButtomTiles.gameTileDataList[pos.x - currentSceneButtomTiles.startPos.x].Length)
            return null;
        return currentSceneButtomTiles.gameTileDataList[pos.x - currentSceneButtomTiles.startPos.x][pos.y - currentSceneButtomTiles.startPos.y];
    }
}

