using UnityEngine;

public class PlaceableItem : MonoBehaviour
{
    private static Color selectColor = new Color(0.5f, 1, 0.5f);
    [SerializeField] SpriteRenderer spriteRenderer;
    public InventoryPlaceable inventoryPlaceable;
    public virtual void Init(InventoryPlaceable placeable)
    {
        inventoryPlaceable = placeable;
    }
    public void SelectThis()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = selectColor;
    }
    public void InSelectThis()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = Color.white;
    }
}
