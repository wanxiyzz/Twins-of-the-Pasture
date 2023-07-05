using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        itemOnMap.indexInCurrentScene = currentSceneMapItem.Count - 1;
    }
}
