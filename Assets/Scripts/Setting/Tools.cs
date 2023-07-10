using UnityEngine;

public static class Tools
{
    public static bool HaveTheType(this ItemType[] itemTypes, ItemType type)
    {
        for (int i = 0; i < itemTypes.Length; i++)
        {
            if (itemTypes[i] == type) return true;
        }
        return false;
    }
    public static Vector3Int LocalToCell(Vector3 localPosition)
    {
        return new Vector3Int(
             Mathf.FloorToInt(localPosition.x),
             Mathf.FloorToInt(localPosition.y),
             0
         );
    }
    public static Vector3Int LocalToCell(SerializableVector3 localPosition)
    {
        return new Vector3Int(
            Mathf.FloorToInt(localPosition.x),
            Mathf.FloorToInt(localPosition.y),
            0
        );
    }
}
