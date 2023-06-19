using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
            SlotManager.Instance.itemToolTip.SetupToolTip(slotUI.itemDetails, slotUI.slotType);
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
