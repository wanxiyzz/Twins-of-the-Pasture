using System.Collections.Generic;
using UnityEngine;
namespace MyGame.Item
{
    public class ItemManager : Singleton<ItemManager>
    {
        //STORED 地图掉落物品
        public Dictionary<string, List<MapItem>> DictMapItem = new Dictionary<string, List<MapItem>>();

        public List<MapItem> currentSceneMapItem;

        public Transform ItemParent;
        [SerializeField] private GameObject MapItemPrafab;

        private void OnEnable()
        {
            EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadEvent;
        }

        private void OnDisable()
        {
            EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadEvent;
        }

        protected override void Awake()
        {
            base.Awake();
        }

        private void OnAfterSceneLoadEvent(SceneType type, string sceneName)
        {
            ItemParent = GameObject.FindGameObjectWithTag("ItemParent").transform;
            if (DictMapItem.ContainsKey(sceneName))
            {
                currentSceneMapItem = DictMapItem[sceneName];
                for (int i = 0; i < currentSceneMapItem.Count; i++)
                {
                    var item = Instantiate(MapItemPrafab, ItemParent);
                    item.GetComponent<ItemOnMap>().Init(currentSceneMapItem[i], currentSceneMapItem[i].position);
                }
            }
            else
            {
                DictMapItem.Add(sceneName, new List<MapItem>());
                currentSceneMapItem = DictMapItem[sceneName];
            }
        }
        /// <summary>
        /// 丢弃东西出现
        /// </summary>
        public void ThrownItem(InventoryItem inventoryItem, SerializableVector2 pos)
        {
            var mapItem = Instantiate(MapItemPrafab, ItemParent);
            var itemOnMap = mapItem.GetComponent<ItemOnMap>();
            MapItem currentItem = new MapItem()
            {
                item = inventoryItem,
                position = pos + Vector2.right,
            };
            currentSceneMapItem.Add(currentItem);
            itemOnMap.ItemThrown(currentItem, pos);
        }
        /// <summary>
        /// 物品掉落物
        /// </summary>
        /// <param name="inventoryItem"></param>
        /// <param name="pos"></param>
        public void CreatItemInventoryItem(InventoryItem inventoryItem, SerializableVector2 pos)
        {
            for (int i = 0; i < inventoryItem.itemAmount; i++)
            {
                var mapItem = Instantiate(MapItemPrafab, ItemParent);
                var itemOnMap = mapItem.GetComponent<ItemOnMap>();
                MapItem currentItem = new MapItem()
                {
                    item = new InventoryItem()
                    {
                        itemAmount = 1,
                        itemID = inventoryItem.itemID,
                    },
                    position = pos + Random.insideUnitCircle,
                };
                currentSceneMapItem.Add(currentItem);
                itemOnMap.ItemCreat(currentItem, pos, currentItem.position.ToVector3());
            }
        }
        public void CreatItemInventoryItem(int itemID, SerializableVector2 pos)
        {

            var mapItem = Instantiate(MapItemPrafab, ItemParent);
            var itemOnMap = mapItem.GetComponent<ItemOnMap>();
            MapItem currentItem = new MapItem()
            {
                item = new InventoryItem()
                {
                    itemAmount = 1,
                    itemID = itemID,
                },
                position = pos + Random.insideUnitCircle,
            };
            currentSceneMapItem.Add(currentItem);
            itemOnMap.ItemCreat(currentItem, pos, currentItem.position.ToVector3());

        }
    }
}