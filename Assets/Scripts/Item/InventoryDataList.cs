using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryDataList", menuName = "Inventory /InventoryDataList", order = 0)]
public class InventoryDataList : ScriptableObject
{
    public InventoryItem[] inventoryItems;
}
