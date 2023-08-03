using System.Collections.Generic;
using UnityEngine;
namespace MyGame.Player
{
    public class PlayerTools : MonoBehaviour
    {
        [SerializeField] List<HoldItemSprite> AllHoldItemSprite;
        [SerializeField] ToolAnimator[] toolAnimators;
        private Animator animator;
        [SerializeField] SpriteRenderer holdItemSprite;
        private InventoryPlaceable currentPlaceable;
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        private void OnEnable()
        {
            EventHandler.PickUpTool += SwitchToolAnimation;
        }
        private void OnDisable()
        {
            EventHandler.PickUpTool -= SwitchToolAnimation;
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                EventHandler.CallUseTool(ToolType.Axe, transform.position);
            }
        }
        public void SwitchToolAnimation(ToolType type)
        {
            for (int i = 0; i < toolAnimators.Length; i++)
            {
                if (toolAnimators[i].type == type)
                    animator.runtimeAnimatorController = toolAnimators[i].animator;
            }
            if (type != ToolType.HoldItem)
            {
                holdItemSprite.enabled = false;
            }
        }
        public void PickUpPlaceable(InventoryPlaceable placeable)
        {
            if (placeable.boxName != string.Empty)
            {
                if (placeable.boxName.Contains("smallBox"))
                {
                    holdItemSprite.enabled = true;
                    holdItemSprite.sprite = AllHoldItemSprite.Find(a => a.itemID == placeable.placeableID).sprite;
                    EventHandler.CallPickPlaceable(holdItemSprite.sprite);
                    return;
                }
            }
            holdItemSprite.sprite = AllHoldItemSprite.Find(a => a.itemID == placeable.placeableID).sprite;
            EventHandler.CallPickPlaceable(holdItemSprite.sprite);
            holdItemSprite.enabled = true;
        }
    }
    [System.Serializable]
    public class ToolAnimator
    {
        public ToolType type;
        public AnimatorOverrideController animator;
    }
    [System.Serializable]
    public class HoldItemSprite
    {
        public int itemID;
        public Sprite sprite;
    }
}