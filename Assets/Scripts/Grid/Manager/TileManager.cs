using System.Collections.Generic;
using UnityEngine;
using MyGame.Data;
using UnityEngine.Tilemaps;
using MyGame.Item;
using MyGame.GameTime;

namespace MyGame.Tile
{
    public class TileManager : Singleton<TileManager>
    {

        [SerializeField] TileBase emptyTile;

        //STORED:游戏中的底部（泥土）Tile存储

        public Dictionary<string, TileDetailsDataList> gameAllButtomTiles = new Dictionary<string, TileDetailsDataList>();
        public TileDetailsDataList currentSceneButtomTiles;
        //STORED:游戏中的中部（地板）Tile存储


        public Grid currentGrid;
        private GridMap currentGridmap;

        public FloorDetails[] floors;


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
                if (tile != null || !tile.haveTop)
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
            if (itemDetails == null) return;
            if (itemDetails.itemType.HaveTheType(ItemType.Seed))
            {
                var plantPos = currentGrid.WorldToCell(pos);
                EventHandler.CallPlantAPlant(plantPos, itemDetails.itemID);
                var tile = ButtomTile(plantPos);
                tile.seedID = itemDetails.itemID;
                tile.canPlant = false;
                DataManager.Instance.UseItem();
            }
            else if (itemDetails.itemType.HaveTheType(ItemType.Floor))
            {
                var usePos = currentGrid.WorldToCell(pos);
                for (int i = 4; i < floors.Length; i++)
                {
                    if (floors[i].itemID == itemDetails.itemID)
                    {
                        currentGridmap.middle.SetTile(usePos, floors[i].tileBase);
                        var tile = ButtomTile(pos);
                        tile.haveTop = true;
                        tile.canPlant = false;
                    }
                }
            }
            else if (itemDetails.itemType.HaveTheType(ItemType.Lawn))
            {
                var usePos = currentGrid.WorldToCell(pos);
                int a = (int)(TimeManager.Instance.season);
                currentGridmap.middle.SetTile(usePos, floors[a].tileBase);
                var tile = ButtomTile(pos);
                tile.haveTop = true;
                tile.canPlant = false;
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
                gameAllButtomTiles.Add(sceneName, currentGridmap.dataList);
                currentSceneButtomTiles = gameAllButtomTiles[sceneName];
            }
            GridManager.Instance.OnAfterSceneLoad(sceneType, currentSceneButtomTiles);
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
            if (width < 0 || hight < 0 || width >= currentSceneButtomTiles.gameTileDataList.Length
            || hight >= currentSceneButtomTiles.gameTileDataList[width].Length
            )
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
            if (width < 0 || hight < 0 || width >= currentSceneButtomTiles.gameTileDataList.Length
            || hight >= currentSceneButtomTiles.gameTileDataList[width].Length)
                return null;
            return currentSceneButtomTiles.gameTileDataList[width][hight];
        }
        public TileDetails ButtomTile(SerializableVector2Int pos)
        {
            int width = pos.x - currentSceneButtomTiles.startPos.x;
            int hight = pos.y - currentSceneButtomTiles.startPos.y;
            if (width < 0 || hight < 0 || width >= currentSceneButtomTiles.gameTileDataList.Length
            || hight >= currentSceneButtomTiles.gameTileDataList[width].Length)
                return null;
            return currentSceneButtomTiles.gameTileDataList[width][hight];
        }

        public TileDetails ButtomTile(Vector3 pos)
        {
            int width = (int)pos.x - currentSceneButtomTiles.startPos.x;
            int hight = (int)pos.y - currentSceneButtomTiles.startPos.y;
            if (width < 0 || hight < 0 || width >= currentSceneButtomTiles.gameTileDataList.Length
            || hight >= currentSceneButtomTiles.gameTileDataList[width].Length)
                return null;
            return currentSceneButtomTiles.gameTileDataList[width][hight];
        }
        public bool CheckCanBuild(Vector3 pos)
        {
            int width = (int)pos.x - currentSceneButtomTiles.startPos.x;
            int hight = (int)pos.y - currentSceneButtomTiles.startPos.y;
            if (width < 0 || hight < 0 || width >= currentSceneButtomTiles.gameTileDataList.Length
            || hight >= currentSceneButtomTiles.gameTileDataList[width].Length)
                return false;
            return currentSceneButtomTiles.gameTileDataList[width][hight].haveTop && currentGridmap.top.GetTile(new Vector3Int((int)pos.x, (int)pos.y, 0)) == null;
        }
        public void SetTopEmpty(Vector3 pos)
        {
            currentGridmap.top.SetTile(new Vector3Int((int)pos.x, (int)pos.y, 0), emptyTile);
        }
        public void SetTopEmpty(Vector3Int pos)
        {
            currentGridmap.top.SetTile(pos, emptyTile);
        }
        public void SetTopNull(Vector3 pos)
        {
            currentGridmap.top.SetTile(new Vector3Int((int)pos.x, (int)pos.y, 0), null);
        }
        public void BuildPlace(PlaceableDetails currentBuleprint, Vector3 buildPos)
        {
            if (currentBuleprint.evenCell)
            {
                Vector3 topPos;
                var currentbuildPos = buildPos - new Vector3(currentBuleprint.aboutCell, 0, 0);
                SetTopEmpty(currentbuildPos);
                for (int j = 0; j < currentBuleprint.aboveCell; j++)
                {
                    topPos = currentbuildPos + new Vector3(0, 1, 0);
                    SetTopEmpty(topPos);
                }
                for (int i = 0; i < 2 * currentBuleprint.aboutCell - 1; i++)
                {
                    currentbuildPos += new Vector3(1, 0, 0);
                    for (int j = 0; j < currentBuleprint.aboveCell; j++)
                    {
                        topPos = currentbuildPos + new Vector3(0, 1, 0);
                        SetTopEmpty(topPos);
                    }
                    SetTopEmpty(currentbuildPos);
                }
            }
            else
            {
                Vector3Int topPos;
                Vector3Int currentbuildPos = Tools.LocalToCell(buildPos);
                SetTopEmpty(currentbuildPos);
                for (int j = 0; j < currentBuleprint.aboveCell; j++)
                {
                    topPos = currentbuildPos + new Vector3Int(0, 1, 0);
                    SetTopEmpty(topPos);
                }
                for (int i = 1; i < currentBuleprint.aboutCell + 1; i++)
                {
                    var checkbuildPos = currentbuildPos + new Vector3Int(0, i, 0);
                    for (int j = 0; j < currentBuleprint.aboveCell; j++)
                    {
                        topPos = checkbuildPos + new Vector3Int(0, 1, 0);

                        SetTopEmpty(topPos);
                    }
                    checkbuildPos = currentbuildPos - new Vector3Int(0, i, 0);
                    for (int j = 0; j < currentBuleprint.aboveCell; j++)
                    {
                        topPos = checkbuildPos + new Vector3Int(0, 1, 0);
                        SetTopEmpty(topPos);
                    }
                }
            }
        }
        public void RemovePlace(PlaceableDetails currentBuleprint, Vector3 buildPos)
        {
            if (currentBuleprint == null) return;
            if (currentBuleprint.evenCell)
            {
                var currentbuildPos = buildPos - new Vector3(currentBuleprint.aboutCell, 0, 0);
                for (int j = 0; j < currentBuleprint.aboveCell; j++)
                {
                    var topPos = currentbuildPos + new Vector3(0, 1, 0);
                    SetTopNull(topPos);
                }
                for (int i = 0; i < 2 * currentBuleprint.aboutCell - 1; i++)
                {
                    currentbuildPos += new Vector3(1, 0, 0);
                    {
                        var topPos = currentbuildPos + new Vector3(0, 1, 0);
                        SetTopNull(topPos);
                    }
                    SetTopNull(currentbuildPos);
                }
            }
            else
            {
                Vector3Int topPos;
                Vector3Int currentbuildPos = Tools.LocalToCell(buildPos);
                SetTopNull(currentbuildPos);
                for (int j = 0; j < currentBuleprint.aboveCell; j++)
                {
                    topPos = currentbuildPos + new Vector3Int(0, 1, 0);
                    SetTopNull(topPos);
                }
                for (int i = 1; i < currentBuleprint.aboutCell + 1; i++)
                {
                    var checkbuildPos = currentbuildPos + new Vector3Int(0, i, 0);
                    for (int j = 0; j < currentBuleprint.aboveCell; j++)
                    {
                        topPos = checkbuildPos + new Vector3Int(0, 1, 0);

                        SetTopNull(topPos);
                    }
                    checkbuildPos = currentbuildPos - new Vector3Int(0, i, 0);
                    for (int j = 0; j < currentBuleprint.aboveCell; j++)
                    {
                        topPos = checkbuildPos + new Vector3Int(0, 1, 0);
                        SetTopNull(topPos);
                    }
                }
            }
        }
        public void ShovelOff(Vector3Int pos)
        {
            ButtomTile(pos).haveTop = false;
            //TODO:掉落物
            for (int i = 0; i < floors.Length; i++)
            {
                if (floors[i].tileBase == currentGridmap.middle.GetTile(pos))
                {
                    ItemManager.Instance.CreatItemInventoryItem(floors[i].itemID, new SerializableVector2(pos.x, pos.y));
                }
            }
            currentGridmap.middle.SetTile(pos, null);

        }
        public void HarvestPlant(Vector3Int pos)
        {
            TileDetails tileDetails = ButtomTile(pos);
            tileDetails.canPlant = false;
            tileDetails.seedID = -1;
            currentGridmap.middle.SetTile(pos, null);
        }

    }

}