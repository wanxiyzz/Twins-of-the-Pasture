using UnityEngine;
using MyGame.Slot;
using MyGame.UI;
namespace MyGame.Data
{
    /// <summary>
    /// 存储游戏的固有数据
    /// </summary>
    public class DataManager : Singleton<DataManager>
    {
        public ItemDataList itemDataList;

        //2000以上为工具
        public ToolDataList toolDataList;

        //3000以上为蓝图
        public ItemDataList buleprintDataList;

        //STORED:角色背包数据
        public InventoryDataList playerBag;

        //STORED：角色其他数据
        private int playerMoney = 2000;
        private int playerHouseLevel = 1;

        [SerializeField] GameObject[] playerHuoses;
        private void OnEnable()
        {
            EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadEvent;
        }
        private void OnDisable()
        {
            EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadEvent;
        }
        private void Start()
        {
            SlotManager.Instance.UpdateBagSlots(playerBag.inventoryItems);
            //TEST
            UIManager.Instance.playerMoney.text = playerMoney.ToString();
        }
        private void OnAfterSceneLoadEvent(SceneType type, string sceneName)
        {
            if (sceneName == "01.Field") LoadPlayerHuose();
        }
        /// <summary>
        /// 除去工具类的物品的全部检索
        /// </summary>
        public ItemDetails FindInventory(int ID)
        {
            if (ID < 2000) return itemDataList.itemDataList.Find(a => a.itemID == ID);
            else return buleprintDataList.itemDataList.Find(a => a.itemID == ID);
        }
        /// <summary>
        /// 检索ID小于1000的物品 
        /// </summary>
        public ItemDetails FindItemDetails(int ID)
        {
            return itemDataList.itemDataList.Find(a => a.itemID == ID);
        }
        /// <summary>
        /// 检索工具
        /// </summary>
        public ToolDetails FindToolDetails(int ID)
        {
            return toolDataList.toolDataList.Find(a => a.toolID == ID);
        }
        /// <summary>
        /// 物品数据的交换
        /// </summary>
        public void SwapItem(int currenIndex, int targetIndex)
        {
            (playerBag.inventoryItems[targetIndex], playerBag.inventoryItems[currenIndex]) =
             (playerBag.inventoryItems[currenIndex], playerBag.inventoryItems[targetIndex]);
        }
        /// <summary>
        /// 物品使用极其减少
        /// </summary>
        public void UseItem()
        {
            int currentIndex = SlotManager.Instance.currenIndex;
            if (currentIndex > 5 || currentIndex < 0) return;
            playerBag.inventoryItems[currentIndex].itemAmount -= 1;
            if (playerBag.inventoryItems[currentIndex].itemAmount == 0)
            {
                playerBag.inventoryItems[currentIndex].itemID = 0;
                SlotManager.Instance.bagSlot[currentIndex].UpdateSlotEmpty();
            }
            else
            {
                var slot = SlotManager.Instance.bagSlot[currentIndex];
                slot.UpdateAmount(playerBag.inventoryItems[currentIndex].itemAmount);
            }
        }
        public void UseItem(int currentIndex, int amount)
        {
            playerBag.inventoryItems[currentIndex].itemAmount -= amount;
            if (playerBag.inventoryItems[currentIndex].itemAmount == 0)
            {
                playerBag.inventoryItems[currentIndex].itemID = 0;
                SlotManager.Instance.bagSlot[currentIndex].UpdateSlotEmpty();
            }
            else
            {
                var slot = SlotManager.Instance.bagSlot[currentIndex];
                slot.UpdateAmount(playerBag.inventoryItems[currentIndex].itemAmount);
            }
        }
        /// <summary>
        /// 丢弃东西的判断 极其数据改动
        /// </summary>
        public void TrownItem(int currenIndex, SlotType slotType)
        {
            if (slotType == SlotType.Bag || slotType == SlotType.Handheld)
            {
                if (currenIndex < 0)
                {
                    playerBag.inventoryItems[5].itemID = 0;
                    playerBag.inventoryItems[5].itemAmount = 0;
                }
                else
                {
                    playerBag.inventoryItems[currenIndex].itemID = 0;
                    playerBag.inventoryItems[currenIndex].itemAmount = 0;
                }
            }
            else if (slotType == SlotType.BigBox)
            {
                if (currenIndex < 0)
                {
                    return;
                }
                else
                {
                    BoxManager.Instance.currentOpenBigBox[currenIndex] = new InventoryItem();

                }
            }
            else if (slotType == SlotType.SmallBox)
            {
                if (currenIndex < 0)
                {
                    return;
                }
                else
                {
                    BoxManager.Instance.currentPickSmallBox[currenIndex] = new InventoryItem();
                }
            }
        }
        /// <summary>
        /// 捡起或者购买东西
        /// </summary>
        /// <returns>是否捡起  物品消失</returns>
        public bool AddItemOnBag(InventoryItem item)
        {
            //TODO:堆叠上限判定
            if (item.itemID < 3000)
            {
                int emptyPos = -1;
                for (int i = playerBag.inventoryItems.Length - 1; i >= 0; i--)
                {
                    if (playerBag.inventoryItems[i].itemID == item.itemID)
                    {
                        playerBag.inventoryItems[i].itemAmount += item.itemAmount;
                        SlotManager.Instance.UpdateBagSlot(playerBag.inventoryItems[i], i);
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
                    SlotManager.Instance.UpdateBagSlot(playerBag.inventoryItems[emptyPos], emptyPos);
                    return true;

                }
            }
            else
            {
                if (playerBag.inventoryItems[5].itemID == 0)
                {
                    playerBag.inventoryItems[5] = item;
                    var tool = FindToolDetails(item.itemID);
                    SlotManager.Instance.UpdateHandSlot(item);
                    if (tool == null)
                    {
                        Debug.LogError("出现未定义物品ID为 " + item.itemID);
                    }
                }
                return true;
            }
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
        public bool SpendMoney(int num)
        {
            if (playerMoney >= num)
            {
                playerMoney -= num;
                UIManager.Instance.playerMoney.text = playerMoney.ToString();
                return true;
            }
            UIManager.Instance.PromptBox("您的钱不够了哦,努力工作去赚点钱吧");
            return false;
        }
        public void MakeMoney(int num)
        {
            playerMoney += num;
            UIManager.Instance.playerMoney.text = playerMoney.ToString();

        }
        public void LoadPlayerHuose()
        {
            GameObject.Instantiate(playerHuoses[playerHouseLevel - 1], new Vector3(0, 0, 0), Quaternion.identity);
        }
        public void PlayerBagToOther(int index, int targetIndex, SlotType slotType)
        {
            InventoryItem temp = new InventoryItem();
            if (slotType == SlotType.BigBox)
                BoxManager.Instance.MoveToCurrentBigBox(playerBag.inventoryItems[index], targetIndex, ref temp);
            else if (slotType == SlotType.SmallBox)
                BoxManager.Instance.MoveToCurrentSmallBox(playerBag.inventoryItems[index], targetIndex, ref temp);
            playerBag.inventoryItems[index] = temp;
            SlotManager.Instance.UpdateBagSlot(playerBag.inventoryItems[index], index);
        }
        public void BigBoxToPlayerBag(int index, int targetIndex)
        {
            InventoryItem temp = new InventoryItem();
            BoxManager.Instance.MoveToCurrentBigBox(playerBag.inventoryItems[index], targetIndex, ref temp);
            playerBag.inventoryItems[index] = temp;
            SlotManager.Instance.UpdateBagSlot(playerBag.inventoryItems[index], index);
        }
        public void MoveToPlayerBag(InventoryItem item, int index, ref InventoryItem myInventory)
        {
            if (playerBag.inventoryItems[index].itemAmount == 0)
            {
                playerBag.inventoryItems[index] = item;
            }
            else if (playerBag.inventoryItems[index].itemID == item.itemID)
            {
                playerBag.inventoryItems[index].itemAmount += item.itemAmount;
            }
            else
            {
                myInventory = playerBag.inventoryItems[index];
                playerBag.inventoryItems[index] = item;
            }
            SlotManager.Instance.UpdateBagSlot(playerBag.inventoryItems[index], index);
        }
        public void MoveToTool(InventoryItem item, int index, ref InventoryItem myInventory)
        {
            if (item.itemID < 3000)
            {
                myInventory = item;
                return;
            }
            else
            {
                if (playerBag.inventoryItems[5].itemAmount != 0)
                {
                    myInventory = playerBag.inventoryItems[5];
                }
                playerBag.inventoryItems[5] = item;
                SlotManager.Instance.UpdateHandSlot(item);
            }
        }
        public void ToolToOther(int targetIndex, SlotType slotType)
        {
            InventoryItem temp = new InventoryItem();
            if (slotType == SlotType.BigBox)
                BoxManager.Instance.MoveToCurrentBigBox(playerBag.inventoryItems[5], targetIndex, ref temp);
            else if (slotType == SlotType.SmallBox)
                BoxManager.Instance.MoveToCurrentSmallBox(playerBag.inventoryItems[5], targetIndex, ref temp);
            playerBag.inventoryItems[5] = temp;
            SlotManager.Instance.UpdateHandSlot(playerBag.inventoryItems[5]);
        }
    }
}