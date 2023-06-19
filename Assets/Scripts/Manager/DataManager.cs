using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    //2000以上为工具
    public ItemDataList itemDataList;
    public InventoryDataList playerBag;
    private void Start()
    {
        SlotManager.Instance.UpdateBagSlots(playerBag.inventoryItems);
    }
    public ItemDetails FindItemDetails(int ID)
    {
        return itemDataList.itemDataList.Find(a => a.itemID == ID);
    }
    /// <summary>
    /// 物品数据的交换
    /// </summary>
    public void SwapItem(int currenIndex, int targetIndex, SlotType slotType)
    {
        if (slotType == SlotType.Bag)
        {
            InventoryItem tempInventoryItem = playerBag.inventoryItems[currenIndex];
            playerBag.inventoryItems[currenIndex] = playerBag.inventoryItems[targetIndex];
            playerBag.inventoryItems[targetIndex] = tempInventoryItem;
        }
    }
    /// <summary>
    /// 丢弃东西的判断 极其数据改动
    /// </summary>
    public void TrownItem(int currenIndex, SlotType slotType)
    {
        if (slotType == SlotType.Bag || slotType == SlotType.Handheld)
        {
            playerBag.inventoryItems[currenIndex].itemID = 0;
            playerBag.inventoryItems[currenIndex].itemAmount = 0;
        }
    }
    /// <summary>
    /// 捡起或者购买东西
    /// </summary>
    /// <returns></returns>
    public bool AddItemOnBag(InventoryItem item)
    {
        //TODO:堆叠上限判定
        if (item.itemID < 2000)
        {
            int emptyPos = -1;
            for (int i = playerBag.inventoryItems.Length - 1; i >= 0; i--)
            {
                if (playerBag.inventoryItems[i].itemID == item.itemID)
                {
                    playerBag.inventoryItems[i].itemAmount += item.itemAmount;
                    SlotManager.Instance.UpdateBagSlots(playerBag.inventoryItems);
                    Debug.Log("增加了");
                    Debug.Log(playerBag.inventoryItems[i].itemID);
                    Debug.Log(item.itemID);
                    return true;
                }
                if (playerBag.inventoryItems[i].itemID == 0)
                {
                    emptyPos = i;
                }
            }
            if (emptyPos == -1) return false;
            else
            {
                playerBag.inventoryItems[emptyPos].itemID = item.itemID;
                playerBag.inventoryItems[emptyPos].itemAmount = item.itemAmount;
                SlotManager.Instance.UpdateBagSlots(playerBag.inventoryItems);
                return true;

            }
        }
        else return false;
    }
    /// <summary>
    /// 物品的叠加
    /// </summary>
    public void ItemOverlay(int currentIndex, int targetIndex, SlotType slotType)
    {
        if (slotType == SlotType.Bag)

        {
            playerBag.inventoryItems[targetIndex].itemAmount += playerBag.inventoryItems[currentIndex].itemAmount;
            playerBag.inventoryItems[currentIndex].itemAmount = 0;
            playerBag.inventoryItems[currentIndex].itemID = 0;
        }
    }
}
