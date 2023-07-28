using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableItem : MonoBehaviour
{
    private static Color selectColor = new Color(0.5f, 1, 0.5f);
    public SpriteRenderer spriteRenderer;
    public InventoryPlaceable inventoryPlaceable;
    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public virtual void Init(InventoryPlaceable placeable)
    {
        inventoryPlaceable = placeable;
    }
    public void SelectThis()
    {
        spriteRenderer.color = selectColor;
    }
    public void InSelectThis()
    {
        spriteRenderer.color = Color.white;
    }
}
