using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyGame.HouseSystem
{
    public class CustormTransition : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            HouseManager.Instance.OutHuose();
        }
    }
}

