using UnityEngine;
using MyGame.Buleprint;
namespace MyGame.Player
{
    public class CheckBox : MonoBehaviour
    {
        [SerializeField] PlayerTools playerTools;
        private PlaceableItem currentItem;
        private Box currentBox;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (currentBox != null)
                    currentBox.OpenOrClose();
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                if (currentItem != null)
                {
                    if (GameManager.Instance.currentPickItem.placeableID != 0) return;
                    GameManager.Instance.currentPickItem = currentItem.inventoryPlaceable;
                    playerTools.PickUpPlaceable(GameManager.Instance.currentPickItem, currentItem.spriteRenderer.sprite);
                    PlaceableManager.Instance.RemovePlaceable(currentItem.inventoryPlaceable);
                    Destroy(currentItem.gameObject);
                    currentBox = null;
                    currentItem = null;
                }
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Holded"))
            {
                currentBox?.CloseBox();
                currentItem?.InSelectThis();
                currentItem = other.GetComponent<PlaceableItem>();
                currentItem.SelectThis();
                if (other.TryGetComponent<Box>(out currentBox))
                {
                    Debug.Log("进入了一个箱子");
                    currentItem = other.GetComponent<Box>();
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
                    currentBox = null;
                }
            }
        }

    }
}