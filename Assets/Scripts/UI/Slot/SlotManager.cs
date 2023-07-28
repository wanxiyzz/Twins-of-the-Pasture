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
        private bool usingSmallBag;
        //大箱子相关
        [SerializeField] SlotUI[] bigSlots;
        [SerializeField] GameObject bigBoxUI;
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
            EventHandler.PickUpTool += OnPickUpTool;
            EventHandler.BeforeSceneLoadEvent += OnBeforeSceneLoadEvent;
            EventHandler.PickPlaceable += OnHoldItemSprite;
        }
        private void OnDisable()
        {
            EventHandler.PickUpTool += OnPickUpTool;
            EventHandler.BeforeSceneLoadEvent -= OnBeforeSceneLoadEvent;
            EventHandler.PickPlaceable -= OnHoldItemSprite;
        }

        private void OnBeforeSceneLoadEvent()
        {
            UpdateSlotsHighLight(-1);
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
        /// 箱子打开时 更新图片和物品
        /// </summary>
        /// <param name="items"></param>
        public void UpdateBoxSlots(InventoryItem[] items)
        {

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
            DataManager.Instance.SwapItem(currenIndex, targetIndex);
        }
        private void OnPickUpTool(ToolType type)
        {
            if (type == ToolType.SmallBox)
            {
                StartCoroutine(SmallBagValid());
            }
            else
            {
                if (smallBox.activeSelf == true) StartCoroutine(SmallBagInValid());
            }
        }
        /// <summary>
        /// 小背包出现
        /// </summary>
        /// <returns></returns>
        IEnumerator SmallBagValid()
        {
            while (usingSmallBag)
            {
                yield return new WaitForSeconds(0.01f);
            }
            usingSmallBag = true;
            smallBox.SetActive(true);
            for (int i = 0; i < 25; i++)
            {
                smallBoxTranform.localScale = new Vector3(smallBoxTranform.localScale.x + 0.04f, smallBoxTranform.localScale.y + 0.04f, 1);
                smallBoxCanvasGroup.alpha += 0.04f;
                yield return new WaitForSeconds(0.01f);
            }
            usingSmallBag = false;
        }
        /// <summary>
        /// 小背包消失
        /// </summary>
        /// <returns></returns>
        IEnumerator SmallBagInValid()
        {
            while (usingSmallBag)
            {
                yield return new WaitForSeconds(0.01f);
            }
            usingSmallBag = true;
            for (int i = 0; i < 25; i++)
            {
                smallBoxTranform.localScale = new Vector3(smallBoxTranform.localScale.x - 0.04f, smallBoxTranform.localScale.y - 0.04f, 1);
                smallBoxCanvasGroup.alpha -= 0.04f;
                yield return new WaitForSeconds(0.01f);
            }
            smallBox.SetActive(false);
            usingSmallBag = false;
        }
        private void OnHoldItemSprite(Sprite sprite)
        {
            handSlot.TrownItem();
            handSlot.HoldItem(sprite);
            EventHandler.CallPickUpTool(ToolType.HoldItem);
            UpdateSlotsHighLight(-1);
            EventHandler.CallSelectItemEvent(null, false);
        }

    }
}