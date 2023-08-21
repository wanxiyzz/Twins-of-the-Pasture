using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HouseDetailsList", menuName = "Huose/HouseDetailsList", order = 0)]
public class HouseDetailsList : ScriptableObject
{
    public List<HouseDetails> houseDetailsList;
}