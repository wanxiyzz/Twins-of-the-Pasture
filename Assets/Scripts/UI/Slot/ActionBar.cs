using UnityEngine;
[RequireComponent(typeof(SlotUI))]
public class ActionBar : MonoBehaviour
{
    [SerializeField] KeyCode key;
    private SlotUI slotUI;
    private void Awake()
    {
        slotUI = GetComponent<SlotUI>();
    }
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            slotUI.SelectThis();
        }
    }
}
