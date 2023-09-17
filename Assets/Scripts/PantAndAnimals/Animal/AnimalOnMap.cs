using System.Collections;
using System.Collections.Generic;
using MyGame.GameTime;
using UnityEngine;
namespace MyGame.Animal
{
    public class AnimalOnMap : MonoBehaviour, IShowState
    {
        private AnimalDetails animalDetails;
        private InventoryAnimal currentAnimal;
        [SerializeField] Animator animator;
        public bool CanHavest => !currentAnimal.isDeath && currentAnimal.havestTime > animalDetails.canHavestTime;

        public string State => currentAnimal.isDeath ? "已死亡" : currentAnimal.havestTime > animalDetails.canHavestTime ? "可收获" : "还需要" + (animalDetails.canHavestTime - currentAnimal.havestTime) + "小时";


        public void Init(InventoryAnimal animal, AnimalDetails details)
        {
            animalDetails = details;
            currentAnimal = animal;
            animator.runtimeAnimatorController = animal.isBaby ? details.babyAnimator : details.ripeAnimator;
            UpdateAnimation(TimeManager.Instance.dayShift);
        }
        public void UpdateAnimation(DayShift dayShift)
        {
            if (dayShift == DayShift.Day && !currentAnimal.isDeath) animator.SetBool("isSleep", false);
            else animator.SetBool("isSleep", true);
        }
        public void UpdateAnimator()
        {
            animator.runtimeAnimatorController = currentAnimal.isBaby ?
            animalDetails.babyAnimator : animalDetails.ripeAnimator;
            if (currentAnimal.isDeath) animator.SetBool("isSleep", true);
        }
    }

}
