using System.Security.Cryptography.X509Certificates;
using UnityEngine;
namespace MyGame.Buleprint
{
    public class Box : PlaceableItem
    {
        private Animator animator;
        public InventoryItem[] boxInventory;
        private bool isOpen;
        [SerializeField] ToolType toolType;
        protected override void Awake()
        {
            base.Awake();
            animator = GetComponentInParent<Animator>();
        }
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
            if (isOpen)
            {
                isOpen = false;
                animator.Play("CloseBox");
            }
            else
            {
                isOpen = true;
                animator.Play("OpenBox");
            }
        }
        public void CloseBox()
        {
            InSelectThis();
            if (isOpen)
            {
                isOpen = false;
                animator.Play("CloseBox");
            }
        }

    }

}