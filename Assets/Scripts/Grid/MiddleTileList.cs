using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MiddleTileList", menuName = "Tile/MiddleTileList")]
public class MiddleTileList : ScriptableObject
{
    public string sceneName;
    public List<SerializableVector2Int> tilePos;
}
