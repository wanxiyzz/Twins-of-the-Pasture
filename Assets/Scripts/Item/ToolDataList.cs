using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ToolDataList", menuName = "Inventory/ToolDataList")]
public class ToolDataList : ScriptableObject
{
    public List<ToolDetails> toolDataList;
}