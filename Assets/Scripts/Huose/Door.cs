using UnityEngine;
using MyGame.GameTime;
namespace MyGame.HuoseSystem
{
    public class Door : MonoBehaviour
    {
        [SerializeField] Animator animator;
        private void Awake()
        {
            OndayChange(TimeManager.Instance.dayShift);
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