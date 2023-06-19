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
}
