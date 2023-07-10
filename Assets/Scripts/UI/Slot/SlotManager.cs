using UnityEngine.UI;
using MyGame.Slot;
using MyGame.UI;
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
    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < bagSlot.Length; i++)
        {
            bagSlot[i].slotIndex = i;
        }
    }
    private void OnEnable()
    {

    }
    private void OnDisable()
    {

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
    public void UpdateHandSlot(InventoryItem tool)
    {
        if (tool.itemAmount > 0)
        {
            handSlot.UpdateSlot(tool);
            EventHandler.CallPickUpTool(DataManager.Instance.FindToolDetails(tool.itemID).toolType);
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
    public void UpdateBoxSlots(InventoryItem items)
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
            DataManager.Instance.SwapItem(currenIndex, targetIndex, SlotType.Bag);
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
        DataManager.Instance.SwapItem(currenIndex, targetIndex, SlotType.Bag);
    }
}
