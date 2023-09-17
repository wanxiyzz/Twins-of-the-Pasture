using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyGame.Animal
{
    public class EggInMap : MonoBehaviour
    {
        [SerializeField] SpriteRenderer[] eggsSprite;
        private int count = 0;
        public void LoadEggNumber(int num)
        {
            for (int i = 0; i < num; i++)
            {
                eggsSprite[i].enabled = true;
            }
            count = num;
        }
        public void DecreaseEgg()
        {
            if (count <= 0) return;
            count--;
            eggsSprite[count].enabled = false;
        }
        public void AddEgg()
        {
            if (count >= 6) return;
            count++;
            eggsSprite[count].enabled = true;
        }
    }
}