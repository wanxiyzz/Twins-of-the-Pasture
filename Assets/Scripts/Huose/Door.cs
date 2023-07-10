using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyGame.Huose
{
    public class Door : MonoBehaviour
    {
        Animator animator;
        private void Start()
        {
            animator = GetComponent<Animator>();
        }
        private void OnEnable()
        {
            EventHandler.DayChange += OndayChange;
        }
        private void OnDisable()
        {
            EventHandler.DayChange -= OndayChange;
        }
        private void OndayChange(DayShift shift)
        {
            animator.SetBool("IsNight", shift == DayShift.Night);
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                animator.SetTrigger("OpenDoor");
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                animator.SetTrigger("CloseDoor");
        }
    }
}