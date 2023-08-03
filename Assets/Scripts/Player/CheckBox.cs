using UnityEngine;
using MyGame.Buleprint;
using MyGame.Slot;
namespace MyGame.Player
{
    public class CheckBox : MonoBehaviour
    {
        [SerializeField] PlayerTools playerTools;
        private PlaceableItem currentItem;
        private Box currentBox;
        public bool takeSmallBox;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (currentBox != null && currentBox.toolType == ToolType.BigBox)
                    currentBox.OpenOrClose();
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                if (currentItem != null)
                {
                    if (GameManager.Instance.currentPickItem == null || GameManager.Instance.currentPickItem.placeableID == 0)
                    {
                        if (currentBox != null && currentBox.toolType == ToolType.SmallBox)
                        {
                            takeSmallBox = true;
                            SlotManager.Instance.LoadSmallBox(currentBox.boxInventory);
                        }
                        GameManager.Instance.currentPickItem = currentItem.inventoryPlaceable;
                        playerTools.PickUpPlaceable(GameManager.Instance.currentPickItem);
                        PlaceableManager.Instance.RemovePlaceable(currentItem.inventoryPlaceable);
                        Destroy(currentItem.gameObject);
                        currentBox = null;
                        currentItem = null;
                    }
                }
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Holded"))
            {
                //取消选择前一个
                currentBox?.CloseBox();
                if(currentBox!=null&&currentBox.toolType==ToolType.SmallBox)
                {
                    SlotManager.Instance.UnLoadSmallBox();
                }
                currentItem?.InSelectThis();

//重新选择下一个
                currentItem = other.GetComponent<PlaceableItem>();
                currentItem.SelectThis();
                if (other.TryGetComponent<Box>(out currentBox))
                {
                    currentItem = other.GetComponent<Box>();
                    if (currentBox.toolType == ToolType.SmallBox)
                    {
                        SlotManager.Instance.LoadSmallBox(currentBox.boxInventory);
                    }
                }
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Holded"))
            {
                if (currentItem == null) return;
                if (other.GetComponent<PlaceableItem>().inventoryPlaceable == currentItem.inventoryPlaceable)
                {
                    currentItem.InSelectThis();
                    currentBox?.CloseBox();
                    if (currentBox.toolType == ToolType.SmallBox && !takeSmallBox)
                    {
                        SlotManager.Instance.UnLoadSmallBox();
                    }
                    currentBox = null;
                }
            }
        }

    }
}