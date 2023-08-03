using System.Collections;
using UnityEngine.UI;
using MyGame.Data;
using MyGame.UI;
using UnityEngine;
namespace MyGame.Slot
{
    /// <summary>
    /// 只处理UI的表现 不处理任何数据 数据在DataManager那里
    /// </summary>
    public class SlotManager : Singleton<SlotManager>
    {
        public ItemToolTip itemToolTip;
        public SlotUI handSlot;
        public SlotUI[] bagSlot;
        public int currenIndex = -1;
        public Image dragItem;

        [SerializeField] GameObject smallBox;
        [SerializeField] SlotUI[] smallBoxSlots;
        private CanvasGroup smallBoxCanvasGroup;
        private RectTransform smallBoxTranform;
        //大箱子相关
        [SerializeField] SlotUI[] bigBoxSlots;
        [SerializeField] GameObject bigBoxUI;
        [SerializeField] CanvasGroup bigBoxCanvasGroup;
        protected override void Awake()
        {
            base.Awake();
            for (int i = 0; i < bagSlot.Length; i++)
            {
                bagSlot[i].slotIndex = i;
            }

            smallBoxTranform = smallBox.GetComponent<RectTransform>();
            smallBoxCanvasGroup = smallBox.GetComponent<CanvasGroup>();

        }
        private void OnEnable()
        {
            EventHandler.BeforeSceneLoadEvent += OnBeforeSceneLoadEvent;
            EventHandler.PickPlaceable += OnHoldItemSprite;
        }
        private void OnDisable()
        {
            EventHandler.BeforeSceneLoadEvent -= OnBeforeSceneLoadEvent;
            EventHandler.PickPlaceable -= OnHoldItemSprite;
        }

        private void OnBeforeSceneLoadEvent()
        {
            EventHandler.CallSelectItemEvent(null, false);
        }


        /// <summary>
        /// 更新背包物品 最后一个是手持
        /// </summary>
        /// <param name="item"></param>
        public void UpdateBagSlots(InventoryItem[] item)
        {
            for (int i = 0; i < bagSlot.Length; i++)
            {
                bagSlot[i].UpdateSlot(item[i]);
            }
            UpdateHandSlot(item[5]);
        }
        public void UpdateBagSlot(InventoryItem item, int index)
        {
            bagSlot[index].UpdateSlot(item);
        }
        public void UpdateHandSlot(InventoryItem tool)
        {
            if (tool.itemAmount > 0)
            {
                handSlot.UpdateSlot(tool);
                EventHandler.CallPickUpTool(DataManager.Instance.FindToolDetails(tool.itemID).toolType);
            }
            else
            {
                EventHandler.CallPickUpTool(ToolType.Hand);
            }
        }
        public void UpdateSlotsHighLight(int index)
        {
            if (currenIndex >= 0)
            {
                bagSlot[currenIndex].highLight.gameObject.SetActive(false);
                bagSlot[currenIndex].isSelected = false;
            }
            if (index < 0)
            {
                currenIndex = index;
                return;
            }
            //选中的是工具一栏
            if (index >= 5)
            {
                EventHandler.CallSelectItemEvent(null, false);
                currenIndex = index;
                return;
            }
            if (currenIndex == index) return;
            if (currenIndex < 5)
            {
                bagSlot[index].highLight.gameObject.SetActive(true);
                currenIndex = index;
            }


        }

        /// <summary>
        /// 背包间的转换
        /// </summary>
        public void SwapItem(int currenIndex, int targetIndex)
        {
            if (currenIndex == targetIndex) return;
            if (bagSlot[targetIndex].itemAmount == 0)
            {
                bagSlot[targetIndex].UpdateSlot(bagSlot[currenIndex].itemDetails, bagSlot[currenIndex].itemAmount);
                bagSlot[currenIndex].UpdateSlotEmpty();
                DataManager.Instance.SwapItem(currenIndex, targetIndex);
                return;
            }
            if (bagSlot[currenIndex].itemDetails == bagSlot[targetIndex].itemDetails)
            {
                bagSlot[targetIndex].UpdateSlot(bagSlot[currenIndex].itemDetails, bagSlot[currenIndex].itemAmount + bagSlot[targetIndex].itemAmount);
                bagSlot[currenIndex].UpdateSlotEmpty();
                DataManager.Instance.ItemOverlay(currenIndex, targetIndex, SlotType.Bag);
                return;
            }
            ItemDetails tempItemDetils = bagSlot[currenIndex].itemDetails;
            int tempAmount = bagSlot[currenIndex].itemAmount;
            bagSlot[currenIndex].UpdateSlot(bagSlot[targetIndex].itemDetails, bagSlot[targetIndex].itemAmount);
            bagSlot[targetIndex].UpdateSlot(tempItemDetils, tempAmount);
            EventHandler.CallSelectItemEvent(null, false);
            DataManager.Instance.SwapItem(currenIndex, targetIndex);
        }
        public void LoadSmallBox(InventoryItem[] items)
        {
            BoxManager.Instance.currentPickSmallBox = items;
            StartCoroutine(Tools.PopUI(smallBoxCanvasGroup, smallBoxTranform));
            for (int i = 0; i < 3; i++)
            {
                smallBoxSlots[i].UpdateSlot(items[i]);
            }
        }
        public void UnLoadSmallBox()
        {
            StartCoroutine(Tools.RecycleUI(smallBoxCanvasGroup, smallBoxTranform));
        }
        private void OnHoldItemSprite(Sprite sprite)
        {
            handSlot.TrownItem();
            handSlot.HoldItem(sprite);
            EventHandler.CallPickUpTool(ToolType.HoldItem);
            UpdateSlotsHighLight(-1);
            EventHandler.CallSelectItemEvent(null, false);
        }
        /// <summary>
        /// 箱子打开时 更新图片和物品
        /// </summary>
        /// <param name="items"></param>
        public void UpdateBigBox(InventoryItem item, int index)
        {
            bigBoxSlots[index].UpdateSlotEmpty();
            if (item.itemAmount != 0)
                bigBoxSlots[index].UpdateSlot(item);
        }
        public void OnOpenBigBox(InventoryItem[] item)
        {
            for (int i = 0; i < 8; i++)
            {
                bigBoxSlots[i].UpdateSlot(item[i]);
            }
            StartCoroutine(Tools.PopUI(bigBoxCanvasGroup, bigBoxUI.transform));

        }
        public void OnCloseBigBox()
        {
            StartCoroutine(Tools.RecycleUI(bigBoxCanvasGroup, bigBoxUI.transform));
        }
        public void UpdateSmallBox(InventoryItem item, int index)
        {
            smallBoxSlots[index].UpdateSlotEmpty();
            if (item.itemAmount != 0)
                smallBoxSlots[index].UpdateSlot(item);
        }
    }
}