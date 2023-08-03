using UnityEngine;
using MyGame.Slot;
using MyGame.Data;
namespace MyGame.Buleprint
{
    public class Box : PlaceableItem
    {
        [SerializeField] Animator animator;
        public InventoryItem[] boxInventory;
        private bool isOpen;
        public ToolType toolType;
        /// <summary>
        /// 初始化箱子
        /// </summary>
        /// <param name="toolType">箱子的类型</param>
        /// <param name="index">为-1则新建一个箱子</param>
        public override void Init(InventoryPlaceable placeable)
        {
            inventoryPlaceable = placeable;
            if (toolType == ToolType.BigBox)
            {
                boxInventory = BoxManager.Instance.GetBigBoxData(ref inventoryPlaceable.boxName);
            }
            if (toolType == ToolType.SmallBox)
                boxInventory = BoxManager.Instance.GetSmallBoxData(ref inventoryPlaceable.boxName);
        }
        public void OpenOrClose()
        {
            if (toolType == ToolType.SmallBox) return;
            if (isOpen)
            {
                isOpen = false;
                SlotManager.Instance.OnCloseBigBox();
                animator.Play("CloseBox");
            }
            else
            {
                isOpen = true;
                SlotManager.Instance.OnOpenBigBox(boxInventory);
                BoxManager.Instance.currentOpenBigBox = boxInventory;
                animator.Play("OpenBox");
            }
        }
        public void CloseBox()
        {
            InSelectThis();
            if (isOpen)
            {
                SlotManager.Instance.OnCloseBigBox();
                isOpen = false;
                animator?.Play("CloseBox");
            }
        }

    }

}