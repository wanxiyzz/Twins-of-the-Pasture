using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyGame.Animal
{
    [CreateAssetMenu(fileName = "AnimalDataList", menuName = "Inventory/AnimalDataList", order = 0)]
    public class AnimalDataList : ScriptableObject
    {
        public List<AnimalDetails> animalDetailsList;
    }
}
