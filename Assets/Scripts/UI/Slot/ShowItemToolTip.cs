using UnityEngine;
using UnityEngine.EventSystems;
using MyGame.Slot;
namespace MyGame.UI
{
    [RequireComponent(typeof(SlotUI))]
    public class ShowItemToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        SlotUI slotUI;

        private void Awake()
        {
            slotUI = GetComponent<SlotUI>();
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (slotUI.itemAmount != 0)
            {
                SlotManager.Instance.itemToolTip.gameObject.SetActive(true);
                SlotManager.Instance.itemToolTip.transform.position = new Vector3(transform.position.x, transform.position.y + GameManager.Instance.toolTipHeight, 0);
                if (slotUI.itemDetails != null)
                    SlotManager.Instance.itemToolTip.SetupToolTip(slotUI.itemDetails, slotUI.slotType);
                else SlotManager.Instance.itemToolTip.SetupToolTip(slotUI.toolDetails, slotUI.slotType);
            }
            else
            {
                SlotManager.Instance.itemToolTip.gameObject.SetActive(false);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            SlotManager.Instance.itemToolTip.gameObject.SetActive(false);
        }
    }

}
