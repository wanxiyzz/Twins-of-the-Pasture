using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyGame.PlantSystem
{
    [CreateAssetMenu(fileName = "PlantData", menuName = "Inventory/PlantData", order = 0)]
    public class PlantDataList : ScriptableObject
    {
        public List<PlantDetails> plantDataList = new List<PlantDetails>();
    }
}
