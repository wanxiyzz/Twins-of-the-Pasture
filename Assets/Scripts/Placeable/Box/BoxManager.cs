using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyGame.Buleprint
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
        public InventoryItem[] OpenBigBox(string boxName)
        {
            currentOpenBigBox = GetBigBoxData(ref boxName);
            return currentOpenBigBox;
        }
        public InventoryItem[] PickSmallBox(string boxName)
        {
            currentPickSmallBox = GetSmallBoxData(ref boxName);
            return currentPickSmallBox;
        }

    }
}