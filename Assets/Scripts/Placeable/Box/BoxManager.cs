using System.Collections.Generic;
using MyGame.Slot;
using MyGame.UI;
using UnityEngine;

namespace MyGame.Data
{
    /// <summary>
    /// 只存储Box的内容数据  不存储位置信息
    /// </summary>
    public class BoxManager : Singleton<BoxManager>
    {
        //STORED 小箱子
        private Dictionary<string, InventoryItem[]> smallBoxDict = new Dictionary<string, InventoryItem[]>();
        public InventoryItem[] currentPickSmallBox;

        //STORED 大箱子
        private Dictionary<string, InventoryItem[]> bigBoxDict = new Dictionary<string, InventoryItem[]>();
        public InventoryItem[] currentOpenBigBox;
        public int SmallBoxCount => smallBoxDict.Count;
        public int BigBoxCount => bigBoxDict.Count;
        public InventoryItem[] FindSmallBox(string key)
        {
            return smallBoxDict.ContainsKey(key) ? smallBoxDict[key] : null;
        }
        public InventoryItem[] FindBigBox(string key)
        {
            return smallBoxDict.ContainsKey(key) ? bigBoxDict[key] : null;
        }
        /// <summary>
        /// 获取大箱子数据,不存在则新建一个
        /// </summary>
        /// <param name="index">箱子编号</param>
        /// <returns></returns>
        public InventoryItem[] GetBigBoxData(ref string boxName)
        {
            if (bigBoxDict.ContainsKey(boxName))
            {
                return bigBoxDict[boxName];
            }
            else
            {
                var data = new InventoryItem[8];
                boxName = "bigBox" + bigBoxDict.Count;
                bigBoxDict.Add(boxName, data);
                return data;
            }
        }
        /// <summary>
        /// 获取小箱子数据,不存在则新建一个
        /// </summary>
        /// <param name="index">箱子编号</param>
        /// <returns></returns>
        public InventoryItem[] GetSmallBoxData(ref string boxName)
        {
            if (smallBoxDict.ContainsKey(boxName))
            {
                return smallBoxDict[boxName];
            }
            else
            {
                boxName = "smallBox" + smallBoxDict.Count;
                var data = new InventoryItem[3];
                smallBoxDict.Add(boxName, data);
                return data;
            }
        }
        public InventoryItem[] PickSmallBox(string boxName)
        {
            currentPickSmallBox = GetSmallBoxData(ref boxName);
            return currentPickSmallBox;
        }
        public void MoveToCurrentBigBox(InventoryItem item, int index, ref InventoryItem myInventory)
        {
            if (item.itemID > 3000)
            {
                if (currentOpenBigBox[index].itemAmount > 0)
                {
                    myInventory = currentOpenBigBox[index];
                    currentOpenBigBox[index] = item;
                    SlotManager.Instance.UpdateBigBox(currentOpenBigBox[index], index);
                    return;
                }
            }
            if (currentOpenBigBox[index].itemAmount == 0)
            {
                currentOpenBigBox[index] = item;
            }
            else if (currentOpenBigBox[index].itemID == item.itemID)
            {
                currentOpenBigBox[index].itemAmount += item.itemAmount;
            }
            else
            {
                myInventory = currentOpenBigBox[index];
                currentOpenBigBox[index] = item;
            }
            SlotManager.Instance.UpdateBigBox(currentOpenBigBox[index], index);
        }
        public void MoveToCurrentSmallBox(InventoryItem item, int index, ref InventoryItem myInventory)
        {
            if (item.itemID > 3000)
            {
                if (currentOpenBigBox[index].itemAmount > 0)
                {
                    myInventory = currentOpenBigBox[index];
                    currentOpenBigBox[index] = item;
                    SlotManager.Instance.UpdateBigBox(currentOpenBigBox[index], index);
                    return;
                }
            }
            if (currentPickSmallBox[index].itemAmount == 0)
            {
                currentPickSmallBox[index] = item;
            }
            else if (currentPickSmallBox[index].itemID == item.itemID)
            {
                currentPickSmallBox[index].itemAmount += item.itemAmount;
            }
            else
            {
                myInventory = currentPickSmallBox[index];
                currentPickSmallBox[index] = item;
            }
            SlotManager.Instance.UpdateSmallBox(currentPickSmallBox[index], index);
        }
        public void BigBoxToOther(int index, int targetIndex, SlotType slotType)
        {
            InventoryItem temp = new InventoryItem();
            if (slotType == SlotType.Bag)
            {
                if (currentOpenBigBox[index].itemID > 3000)
                {
                    UIManager.Instance.PromptBox("要把工具放在手上哦");
                    return;
                }
                DataManager.Instance.MoveToPlayerBag(currentOpenBigBox[index], targetIndex, ref temp);
            }
            else if (slotType == SlotType.SmallBox)
                MoveToCurrentSmallBox(currentOpenBigBox[index], targetIndex, ref temp);
            else if (slotType == SlotType.Handheld)
                DataManager.Instance.MoveToTool(currentOpenBigBox[index], targetIndex, ref temp);
            else if (slotType == SlotType.BigBox)
                MoveToCurrentBigBox(currentOpenBigBox[index], targetIndex, ref temp);
            currentOpenBigBox[index] = temp;
            SlotManager.Instance.UpdateBigBox(currentOpenBigBox[index], index);
        }
        public void SmallBoxToOther(int index, int targetIndex, SlotType slotType)
        {
            InventoryItem temp = new InventoryItem();
            if (slotType == SlotType.Bag)
            {
                if (currentOpenBigBox[index].itemID > 3000)
                {
                    UIManager.Instance.PromptBox("要把工具放在手上哦");
                    return;
                }
                DataManager.Instance.MoveToPlayerBag(currentOpenBigBox[index], targetIndex, ref temp);
            }
            else if (slotType == SlotType.BigBox)
                MoveToCurrentBigBox(currentPickSmallBox[index], targetIndex, ref temp);
            else if (slotType == SlotType.SmallBox)
                MoveToCurrentSmallBox(currentPickSmallBox[index], targetIndex, ref temp);
            else if (slotType == SlotType.Handheld)
                DataManager.Instance.MoveToTool(currentOpenBigBox[index], targetIndex, ref temp);
            currentPickSmallBox[index] = temp;
            SlotManager.Instance.UpdateSmallBox(currentPickSmallBox[index], index);
        }

    }
}