using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataList", menuName = "Inventory/ItemDataList")]
public class ItemDataList : ScriptableObject
{
    public List<ItemDetails> itemDataList = new List<ItemDetails>();
}
