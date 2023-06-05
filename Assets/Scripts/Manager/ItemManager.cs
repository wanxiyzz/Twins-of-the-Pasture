using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public ItemDataList itemDataList;
    public ItemDetails FindItemDetails(int ID)
    {
        foreach (var item in itemDataList.itemDataList)
        {
            if (item.itemID == ID)
            {
                return item;
            }
        }
        return null;
    }
}
