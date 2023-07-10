using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Tile
{
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
            EventHandler.SeasonChange += OnSeasonChange;
            EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadEvent;
            EventHandler.BeforeSceneLoadEvent += OnBeforeSceneLoadEvent;
            EventHandler.HourUpdate += OnHourUpdate;
            EventHandler.UseItemEvent += OnUseItemEvent;
            EventHandler.UseTool += OnUseTool;
        }
        private void OnDisable()
        {
            EventHandler.SeasonChange -= OnSeasonChange;
            EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadEvent;
            EventHandler.HourUpdate -= OnHourUpdate;
            EventHandler.UseItemEvent -= OnUseItemEvent;
            EventHandler.UseTool -= OnUseTool;
            EventHandler.BeforeSceneLoadEvent -= OnBeforeSceneLoadEvent;
        }



        private void OnUseTool(ToolType type, Vector3 pos)
        {
            if (type == ToolType.Hoe)
            {
                Vector3Int position = currentGrid.WorldToCell(pos);
                GridManager.Instance.DigPostion(position);
                var tile = ButtomTile(position);
                if (tile != null)
                {
                    tile.canPlant = true;
                }
            }
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
                    var tile = ButtomTile(plantPos);
                    tile.seedID = itemDetails.itemID;
                    tile.canPlant = false;
                    DataManager.Instance.UseItem(SlotManager.Instance.currenIndex);
                }
            }
        }
        private void OnBeforeSceneLoadEvent()
        {
            currentSceneButtomTiles = null;
        }
        private void OnAfterSceneLoadEvent(SceneType sceneType, string sceneName)
        {
            currentGrid = FindObjectOfType<Grid>();
            currentGridmap = currentGrid.gameObject.GetComponent<GridMap>();
            var buttomTileMap = FindObjectOfType<ButtomTileMap>();
            //底部Tiles(土壤属性)的存储
            if (gameAllButtomTiles.ContainsKey(sceneName))
            {
                currentSceneButtomTiles = gameAllButtomTiles[sceneName];
            }
            else
            {
                canPlant = true;
                gameAllButtomTiles.Add(sceneName, currentGridmap.dataList);
                currentSceneButtomTiles = gameAllButtomTiles[sceneName];
            }
            //中间草坪地板的存储
            if (middleTileDict.ContainsKey(sceneName))
            {
                currentSceneMiddleTiles = middleTileDict[sceneName];
            }
            else
            {
                middleTileDict.Add(sceneName, currentGridmap.middleTileList);
                currentSceneMiddleTiles = middleTileDict[sceneName];
            }
            GridManager.Instance.OnAfterSceneLoad(sceneType, currentSceneMiddleTiles);
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
        public TileDetails ButtomTile(Vector3Int pos)
        {
            if (currentSceneButtomTiles == null) return null;
            int width = pos.x - currentSceneButtomTiles.startPos.x;
            int hight = pos.y - currentSceneButtomTiles.startPos.y;
            if (width >= currentSceneButtomTiles.gameTileDataList.Length
            || hight >= currentSceneButtomTiles.gameTileDataList[width].Length
            || width < 0 || hight < 0)
            {
                return null;
            }
            else
            {
                return currentSceneButtomTiles.gameTileDataList[width][hight];
            }
        }
        public TileDetails ButtomTile(Vector2Int pos)
        {
            int width = pos.x - currentSceneButtomTiles.startPos.x;
            int hight = pos.y - currentSceneButtomTiles.startPos.y;
            if (width >= currentSceneButtomTiles.gameTileDataList.Length
            || hight >= currentSceneButtomTiles.gameTileDataList[width].Length)
                return null;
            return currentSceneButtomTiles.gameTileDataList[width][hight];
        }
        public TileDetails ButtomTile(Vector3 pos)
        {
            int width = (int)pos.x - currentSceneButtomTiles.startPos.x;
            int hight = (int)pos.y - currentSceneButtomTiles.startPos.y;
            if (width >= currentSceneButtomTiles.gameTileDataList.Length
            || hight >= currentSceneButtomTiles.gameTileDataList[width].Length)
                return null;
            return currentSceneButtomTiles.gameTileDataList[width][hight];
        }
    }


}