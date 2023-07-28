using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Placeableitem", menuName = "Placeable/Placeableitem", order = 0)]
public class PlaceableitemList : ScriptableObject
{
    public List<PlaceableDetails> placeableitemsList;

}
