using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] Image itemImage;
    [SerializeField] Text amountText;
    [SerializeField] Button button;
    public GameObject highLight;
    public SlotType slotType;
    public ItemDetails itemDetails;
    public int itemAmount;
    public bool isSelected;
    public int slotIndex = -1;
    private void Awake()
    {
        isSelected = false;
        if (itemDetails.itemID == 0 || slotType == SlotType.Bag)
        {
            UpdateSlotEmpty();
        }
        button.onClick.AddListener(SelectThis);
    }
    public void UpdateSlot(ItemDetails item, int amount)
    {
        if (item == null || amount == 0) return;
        itemImage.enabled = true;
        itemImage.sprite = item.itemIcon != null ? item.itemIcon : item.itemOnWorldSprite;
        itemDetails = item;
        itemAmount = amount;
        amountText.enabled = true;
        amountText.text = amount.ToString();
        if (slotType == SlotType.Handheld) return;
        button.interactable = true;
    }
    public void UpdateSlot(InventoryItem inventoryItem)
    {
        var item = DataManager.Instance.FindItemDetails(inventoryItem.itemID);
        int amount = inventoryItem.itemAmount;
        if (item == null) return;
        itemImage.enabled = true;
        itemImage.sprite = item.itemIcon != null ? item.itemIcon : item.itemOnWorldSprite;
        itemDetails = item;
        itemAmount = amount;
        amountText.enabled = true;
        amountText.text = amount.ToString();
        if (slotType == SlotType.Handheld) return;
        button.interactable = true;
    }
    public void UpdateSlotEmpty()
    {
        itemImage.enabled = false;
        itemDetails = null;
        itemAmount = 0;
        amountText.enabled = false;
        button.interactable = false;
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
            SlotManager.Instance.UpdateSlotsHighLight(100);
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
            if (slotType == SlotType.Bag && targetSlot.slotType == SlotType.Bag)
            {
                SlotManager.Instance.SwapItem(slotIndex, targetIndex);
            }
            //TODO:补全所有的Slot转移情况
            else
            {
                TrownItem();
            }
        }
        else
        {
            TrownItem();
        }
        SlotManager.Instance.UpdateSlotsHighLight(100);


    }
    public void TrownItem()
    {
        if (itemAmount == 0) return;
        EventHandler.CallSelectItemEvent(null, false);
        DataManager.Instance.TrownItem(slotIndex, slotType);
        InventoryItem inventoryItem = new InventoryItem()
        {
            itemID = itemDetails.itemID,
            itemAmount = itemAmount,
        };
        ItemManager.Instance.ThrownItem(inventoryItem, GameManager.Instance.player.transform.position);
        UpdateSlotEmpty();
    }
}
