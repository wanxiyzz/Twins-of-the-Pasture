using UnityEngine;
using UnityEngine.UI;
using MyGame.Data;
using MyGame.Item;
using UnityEngine.EventSystems;
namespace MyGame.Slot
{
    public class SlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] Image itemImage;
        [SerializeField] Text amountText;
        [SerializeField] Button button;
        public GameObject highLight;
        public SlotType slotType;
        public ItemDetails itemDetails;
        public ToolDetails toolDetails;
        public int itemAmount;
        public bool isSelected;
        public int slotIndex = 5;

        private void Awake()
        {
            isSelected = false;
            if (itemDetails.itemID == 0 || slotType == SlotType.Bag)
            {
                UpdateSlotEmpty();
            }
            if (slotType == SlotType.Bag)
                button.onClick.AddListener(SelectThis);
        }
        /// <summary>
        /// 更新数量  0为空
        /// </summary>
        /// <param name="amount"></param>
        public void UpdateAmount(int amount)
        {
            if (amount == 0) UpdateSlotEmpty();
            else
            {
                itemAmount = amount;
                amountText.text = amount.ToString();
            }

        }
        public void UpdateSlot(ItemDetails item, int amount)
        {
            if (item == null || amount == 0) return;
            itemImage.enabled = true;
            itemImage.sprite = item.itemIcon ?? item.itemOnWorldSprite;
            itemDetails = item;
            itemAmount = amount;
            amountText.enabled = true;
            amountText.text = amount.ToString();
            if (slotType == SlotType.Handheld) return;
            button.interactable = true;
        }
        public void UpdateSlot(InventoryItem inventoryItem)
        {

            if (inventoryItem.itemAmount == 0)
            {
                UpdateSlotEmpty();
                return;
            }
            if (inventoryItem.itemID > 3000)
            {
                itemAmount = 1;
                toolDetails = DataManager.Instance.FindToolDetails(inventoryItem.itemID);
                if (toolDetails == null) return;
                itemImage.sprite = toolDetails.toolIcon ?? toolDetails.toolOnWorldSprite;
                itemImage.enabled = true;
            }
            else
            {
                var item = DataManager.Instance.FindInventory(inventoryItem.itemID);
                int amount = inventoryItem.itemAmount;
                if (item == null) return;
                itemImage.enabled = true;
                itemImage.sprite = item.itemIcon ?? item.itemOnWorldSprite;
                itemDetails = item;
                itemAmount = amount;
                amountText.enabled = true;
                amountText.text = amount.ToString();
                button.interactable = true;
            }
        }
        public void UpdateSlotEmpty()
        {
            itemImage.enabled = false;
            itemDetails = null;
            itemAmount = 0;
            amountText.enabled = false;
            button.interactable = false;
            if (isSelected)
            {
                SlotManager.Instance.UpdateSlotsHighLight(-1);
                EventHandler.CallSelectItemEvent(null, false);
                isSelected = false;
            }
        }
        public void SelectThis()
        {
            isSelected = !isSelected;
            if (isSelected)
            {
                SlotManager.Instance.UpdateSlotsHighLight(slotIndex);
                EventHandler.CallSelectItemEvent(itemDetails, isSelected);
            }
            else
            {
                SlotManager.Instance.UpdateSlotsHighLight(-1);
                EventHandler.CallSelectItemEvent(itemDetails, isSelected);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (itemAmount != 0)
            {
                SlotManager.Instance.dragItem.enabled = true;
                SlotManager.Instance.dragItem.sprite = itemImage.sprite;
                SlotManager.Instance.dragItem.SetNativeSize();
                isSelected = true;
                if (slotType == SlotType.Bag)
                    SlotManager.Instance.UpdateSlotsHighLight(slotIndex);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            SlotManager.Instance.dragItem.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            SlotManager.Instance.dragItem.enabled = false;
            if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<SlotUI>() != null)
            {
                var targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<SlotUI>();
                int targetIndex = targetSlot.slotIndex;
                if (slotType == SlotType.Bag)
                {
                    Debug.Log(targetSlot.slotType);
                    if (targetSlot.slotType == SlotType.Bag)
                        SlotManager.Instance.SwapItem(slotIndex, targetIndex);
                    if (targetSlot.slotType == SlotType.BigBox || targetSlot.slotType == SlotType.SmallBox)
                        DataManager.Instance.MoveToOther(slotIndex, targetIndex, targetSlot.slotType);
                    else TrownItem();
                }
                else if (slotType == SlotType.BigBox)
                {
                    if (targetSlot.slotType == SlotType.Bag || targetSlot.slotType == SlotType.SmallBox)
                        BoxManager.Instance.BigBoxToOther(slotIndex, targetIndex, targetSlot.slotType);
                    else TrownItem();
                }
                else if (slotType == SlotType.SmallBox)
                {
                    if (targetSlot.slotType == SlotType.Bag || targetSlot.slotType == SlotType.BigBox)
                        BoxManager.Instance.SmallBoxToOther(slotIndex, targetIndex, targetSlot.slotType);
                    else TrownItem();
                }

            }
            else
            {
                TrownItem();
            }
            SlotManager.Instance.UpdateSlotsHighLight(-1);


        }
        /// <summary>
        /// 丢东西
        /// </summary>
        public void TrownItem()
        {
            if (itemAmount == 0) return;
            EventHandler.CallSelectItemEvent(null, false);
            DataManager.Instance.TrownItem(slotIndex, slotType);
            InventoryItem inventoryItem;
            if (itemDetails != null)
            {
                inventoryItem = new InventoryItem()
                {
                    itemID = itemDetails.itemID,
                    itemAmount = itemAmount,
                };
            }
            else
            {
                EventHandler.CallPickUpTool(ToolType.Hand);
                inventoryItem = new InventoryItem()
                {
                    itemID = toolDetails.toolID,
                    itemAmount = itemAmount,
                };
            }
            ItemManager.Instance.ThrownItem(inventoryItem, GameManager.Instance.player.transform.position);
            UpdateSlotEmpty();
        }
        public void HoldItem(Sprite sprite)
        {
            itemImage.enabled = true;
            itemImage.sprite = sprite;
        }
    }
}
