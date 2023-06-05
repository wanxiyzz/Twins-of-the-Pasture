using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class SerializableVector3
{
    public float x;
    public float y;
    public float z;
    public SerializableVector3(Vector3 pos)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
    }
    public SerializableVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    public Vector3 ToVector3()
    {
        return new Vector3(this.x, this.y, this.z);
    }
    public Vector2Int ToVector2Int()
    {
        return new Vector2Int((int)this.x, (int)this.y);
    }
}
[Serializable]
public class PlantState
{
    public int seedID = -1;
    public SerializableVector3 position;
    public int deathPrecent;
    public int goodPrecent;
    public int grouthHuornum;
    public int currentPeriod;
    public int currentHarvestPeriod;
    public int plantingTime;

    public void Init(Vector3 pos, int seedID)
    {
        position = new SerializableVector3(pos);
        this.seedID = seedID;
        currentPeriod = 1;
        plantingTime = TimeManager.Instance.minute;
    }
}
[Serializable]
public class TileDetails
{
    public bool haveTop;
    public int seedID;
    public SerializableVector3 position;
    public float NVaule;
    public float PVaule;
    public float KVaule;
    public int waterVaule;
    public int bugValue;
    public TileDetails(bool haveTop)
    {
        this.haveTop = haveTop;
    }
}
[Serializable]
public class PlantDetails
{
    public int seedId;
    public int growthPeriod;
    public int canHarvestPeriod;
    public Sprite[] growthSprite;
    public Sprite deathSpite;
    public int onceGrothHour;
    public Season[] bestSeason;
    public TileDetails bestTile;
    public ToolType harvestTool;
    public CanHarvestPlant[] canHarvestPlants;
    public bool canTwiceHavest;//1,2,3对应CanHavest的各个阶段
    public int afterHavest;//收割后的阶段
    public bool needPillar;
}
[Serializable]
public class CanHarvestPlant
{
    public Sprite[] sprites;
    public int fruitID;
    public int amountMax;
    public int amountMin;
}
[Serializable]
public class ItemDetails
{
    public int itemID;
    public string itemName;
    public ItemType itemType;
    public Sprite itemIcon;
    public Sprite itemOnWorldSprite;
    public string itemdDescription;
    public bool canDropped;
    public bool canCarried;
    public int itemPrice;
    public bool canEat;
    public int addHungry;
    public int addMental;
    public int addPhysical;
}
[Serializable]
public struct InventoryItem
{
    public int itemID;
    public int itemAmount;
}
