using System.Collections.Generic;
using UnityEngine;
using MyGame.Data;
using MyGame.Tile;
using MyGame.Slot;
using MyGame.HouseSystem;

namespace MyGame.Buleprint
{
    public class PlaceableManager : Singleton<PlaceableManager>
    {
        private Transform placeableParent;


        [SerializeField] PlaceableitemList placeableData;


        //STORED:家具和场景的信息
        private Dictionary<string, List<InventoryPlaceable>> placeableDataDict = new Dictionary<string, List<InventoryPlaceable>>();
        private List<InventoryPlaceable> currentScenePlaceable = new List<InventoryPlaceable>();



        //STORED:已放置地块


        private bool currentInMyHome;

        private void OnEnable()
        {
            EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadEvent;
            EventHandler.UseTool += OnUseTool;
            //TEST
            if (currentInMyHome) return;
        }
        private void OnDisable()
        {
            EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadEvent;
            EventHandler.UseTool += OnUseTool;
        }
        public PlaceableDetails FindPlaceableDetails(int ID)
        {
            return placeableData.placeableitemsList.Find(a => a.buleprintID == ID);
        }
        public void OnAfterSceneLoadEvent(SceneType type, string sceneName)
        {
            if (type == SceneType.AnimalHuose || type == SceneType.PlantHuose) return;
            if (type == SceneType.MyHuose) currentInMyHome = true;
            else currentInMyHome = false;
            if (type != SceneType.PeopleHome)
            {
                placeableParent = GameObject.FindGameObjectWithTag("PlaceableParent").transform;
            }

            //放置物品处理
            if (placeableDataDict.ContainsKey(sceneName))
            {
                currentScenePlaceable = placeableDataDict[sceneName];
            }
            else
            {
                currentScenePlaceable = new List<InventoryPlaceable>();
                placeableDataDict.Add(sceneName, currentScenePlaceable);
            }
            //可放置物品初始化
            for (int i = 0; i < currentScenePlaceable.Count; i++)
            {
                InitPlaceable(currentScenePlaceable[i]);
            }
        }
        public bool BuildPlace(PlaceableDetails placeable, Vector3 pos)
        {
            if (placeable.isHouse)
            {
                HouseManager.Instance.BuildHouse(placeable.houseType, pos);
            }
            if (DataManager.Instance.SpendMoney(placeable.money))
            {
                var item = GameObject.Instantiate(placeable.prefab, pos, Quaternion.identity, placeableParent);
                InventoryPlaceable inventoryPlaceable = new InventoryPlaceable()
                {
                    placeableID = placeable.buleprintID,
                    position = pos,
                    boxName = string.Empty,
                };
                item.GetComponent<PlaceableItem>().Init(inventoryPlaceable);
                Debug.Log(inventoryPlaceable.boxName);
                currentScenePlaceable.Add(inventoryPlaceable);
                DataManager.Instance.UseItem();
                TileManager.Instance.BuildPlace(placeable, pos);
                return true;
            }
            return false;
        }
        private void InitPlaceable(InventoryPlaceable placeable)
        {

            var placeableDetails = FindPlaceableDetails(placeable.placeableID);
            var item = GameObject.Instantiate(placeableDetails.prefab, placeable.position.ToVector3(), Quaternion.identity, placeableParent);
            TileManager.Instance.BuildPlace(placeableDetails, placeable.position.ToVector3());
            var boxdata = item.GetComponent<PlaceableItem>();
            boxdata.Init(placeable);
        }
        public void RemovePlaceable(InventoryPlaceable placeable)
        {
            if (currentScenePlaceable.Contains(placeable))
            {
                currentScenePlaceable.Remove(placeable);
            }
            TileManager.Instance.RemovePlace(FindPlaceableDetails(placeable.placeableID), placeable.position.ToVector3());
        }
        private void OnUseTool(ToolType type, Vector3 vector)
        {
            if (type == ToolType.HoldItem)
            {
                GameManager.Instance.currentPickItem.position = vector;
                InitPlaceable(GameManager.Instance.currentPickItem);
                currentScenePlaceable.Add(GameManager.Instance.currentPickItem);
                GameManager.Instance.currentPickItem = null;
                SlotManager.Instance.handSlot.UpdateSlotEmpty();
                EventHandler.CallPickUpTool(ToolType.Hand);
            }

        }
    }
}
