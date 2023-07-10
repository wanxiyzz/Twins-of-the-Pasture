using UnityEngine;
namespace MyGame.WeatherSystem
{
    public class RainDrop : MonoBehaviour
    {
        Animator animator;
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        private void OnEnable()
        {
            animator.Play("RainDropEffect");
        }
        public void ReleaseThis()
        {
            gameObject.SetActive(false);
        }
    }
}