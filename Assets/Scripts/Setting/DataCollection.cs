using MyGame.GameTime;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;

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
    public Vector3Int ToVector3Int()
    {
        return new Vector3Int((int)this.x, (int)this.y, (int)this.z);
    }
    public Vector2Int ToVector2Int()
    {
        return new Vector2Int((int)this.x, (int)this.y);
    }
    public Vector2 ToVector2()
    {
        return new Vector2(this.x, this.y);
    }
    public static implicit operator SerializableVector3(Vector3 pos)
    {
        return new SerializableVector3(pos.x, pos.y, pos.z);
    }
}
[Serializable]
public class SerializableVector3Int
{
    public int x;
    public int y;
    public int z;
    public SerializableVector3Int(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    public SerializableVector3Int(Vector3Int pos)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
    }
    public SerializableVector3Int(Vector3 pos)
    {
        this.x = (int)pos.x;
        this.y = (int)pos.y;
        this.z = (int)pos.z;
    }
    public Vector3Int ToVector3Int()
    {
        return new Vector3Int(x, y, z);
    }
    public static implicit operator SerializableVector3Int(Vector3Int pos)
    {
        return new SerializableVector3Int(pos.x, pos.y, pos.z);
    }
}
[Serializable]
public class SerializableVector2
{
    public float x;
    public float y;
    public SerializableVector2(Vector2 pos)
    {
        this.x = pos.x;
        this.y = pos.y;
    }
    public SerializableVector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
    public override string ToString()
    {
        return "x:" + x + "  y:" + y;
    }
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + x.GetHashCode();
            hash = hash * 29 + y.GetHashCode();
            return hash;
        }
    }
    public override bool Equals(object obj)
    {
        if (obj is SerializableVector2 other)
        {
            return x == other.x && y == other.y;
        }
        return false;
    }
    public Vector3 ToVector3()
    {
        return new Vector3(x, y, 0);
    }
    public static SerializableVector2 operator +(SerializableVector2 vector2, Vector2 pos)
    {
        return new SerializableVector2(vector2.x + pos.x, vector2.y + pos.y);
    }
    public static SerializableVector2 operator +(SerializableVector2 vector2, Vector3 pos)
    {
        return new SerializableVector2(vector2.x + pos.x, vector2.y + pos.y);
    }
    public static implicit operator SerializableVector2(Vector3 pos)
    {
        return new SerializableVector2(pos.x, pos.y);
    }

}
[Serializable]
public class SerializableVector2Int
{
    public int x;
    public int y;
    public static SerializableVector2Int ToSerializableVector2Int(Vector3Int pos)
    {
        return new SerializableVector2Int(pos);
    }
    public SerializableVector2Int(Vector3Int pos)
    {
        this.x = pos.x;
        this.y = pos.y;
    }
    public SerializableVector2Int(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public static implicit operator SerializableVector2Int(Vector2Int pos)
    {
        return new SerializableVector2Int(pos.x, pos.y);
    }
    public Vector2Int ToVector2Int()
    {
        return new Vector2Int(this.x, this.y);
    }
    public Vector3Int ToVector3Int()
    {
        return new Vector3Int(this.x, this.y, 0);
    }
    public Vector3 ToVector3()
    {
        return new Vector3(this.x, this.y, 0);
    }
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;
        SerializableVector2 other = (SerializableVector2)obj;
        return this.x == other.x && this.y == other.y;
    }
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 33;
            hash = hash * 17 + x.GetHashCode();
            hash = hash * 23 + y.GetHashCode();
            return hash;
        }
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
    public string deathCause;
    public bool isDeath;
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
    public int seedID = -1;
    public SerializableVector3Int position;
    public bool canPlant;
    public float NVaule;
    public float PVaule;
    public float KVaule;
    public int waterVaule;
    public int bugValue;
    public TileDetails(bool haveTop, int x, int y)
    {
        this.haveTop = haveTop;
        position = new SerializableVector3Int(x, y, 0);
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
    public ItemType[] itemType;
    public Sprite itemIcon;
    public Sprite itemOnWorldSprite;
    public string itemdDescription;
    public int itemPrice;
    public bool canEat;
    public int addHungry;
    public int addMental;
    public int addPhysical;
}
[Serializable]
public class ToolDetails
{
    public int toolID;
    public string toolName;
    public ToolType toolType;
    public Sprite toolIcon;
    public Sprite toolOnWorldSprite;
    public string toolDescription;
    public int toolPrice;
}
[Serializable]
public struct InventoryItem
{
    public int itemID;
    public int itemAmount;
}
[Serializable]
public class MapItem
{
    public SerializableVector2 position;
    public InventoryItem item;
}
[Serializable]
public class InventoryGrass
{
    public SerializableVector2Int position;
    public bool isBig;
}
[Serializable]
public class PlaceableDetails
{
    public int buleprintID;
    public Sprite sprite;
    public bool isBox;
    public HouseType houseType;
    public bool isHouse;
    public int aboveCell;
    public int aboutCell;
    public bool evenCell;
    public int money;
    public GameObject prefab;
}
[Serializable]
public class InventoryPlaceable
{
    public int placeableID;
    public string boxName;
    public SerializableVector2 position;
}
[Serializable]
public class InventoryBox
{
    public int ID;
    public SerializableVector2 position;
    public ToolType boxType;
    public int index;
}
[Serializable]
public class FloorDetails
{
    public TileBase tileBase;
    public int itemID;
}
[Serializable]
public class HouseDetails
{
    public HouseType houseType;
    public GameObject prefab;
}