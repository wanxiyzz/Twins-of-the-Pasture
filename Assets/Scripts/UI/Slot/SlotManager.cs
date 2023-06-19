using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 只处理UI的表现 不处理任何数据 数据在DataManager那里
/// </summary>
public class SlotManager : Singleton<SlotManager>
{
    public ItemToolTip itemToolTip;

    public SlotUI handSlot;
    public SlotUI[] bagSlot;
    private int currenIndex = 100;
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
        handSlot.UpdateSlot(item[item.Length - 1]);
    }
    public void UpdateSlotsHighLight(int index)
    {
        if (index > 10)
        {
            if (currenIndex > 10) return;
            bagSlot[currenIndex].highLight.gameObject.SetActive(false);
            bagSlot[currenIndex].isSelected = false;
            currenIndex = index;
            return;
        }
        if (index < 0)
        {
            bagSlot[currenIndex].isSelected = false;
            currenIndex = index;
            return;
        }
        if (currenIndex == index) return;
        if (currenIndex < 10)
        {
            bagSlot[currenIndex].highLight.gameObject.SetActive(false);
            bagSlot[currenIndex].isSelected = false;
        }
        bagSlot[index].highLight.gameObject.SetActive(true);
        currenIndex = index;

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
