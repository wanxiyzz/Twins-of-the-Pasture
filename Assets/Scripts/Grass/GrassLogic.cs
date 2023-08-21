using System.Collections;
using UnityEngine;
using MyGame.PlantSystem;
using MyGame.Item;

namespace MyGame.GrassSystem
{
    public class GrassLogic : MonoBehaviour, IHavested
    {
        public static InventoryItem grassInvetory = new InventoryItem()
        {
            itemID = 1033,
        };
        private Animator animator;
        private SpriteRenderer spriteRenderer;
        public InventoryGrass inventoryGrass;
        public bool CanHarvest => true;

        public Vector3 Position => transform.position;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        private void OnEnable()
        {
            EventHandler.SeasonChange += OnSeasonChange;
        }
        private void OnDisable()
        {
            EventHandler.SeasonChange -= OnSeasonChange;
        }

        private void OnSeasonChange(Season season)
        {
            StartCoroutine(SpriteChenge((int)season));
        }
        IEnumerator SpriteChenge(int num)
        {
            yield return new WaitForSeconds(0.35f);
            spriteRenderer.sprite = inventoryGrass.isBig ? GrassManager.Instance.bigGarssSprite[num] : GrassManager.Instance.smallGarssSprite[num];
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            //AUDIO:草丛音效
            animator.Play("GrassShake");
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            //AUDIO:草丛音效
            animator.Play("GrassShake");
        }

        public void Harvested()
        {
            //TODO:草丛掉落特效和物品掉落

            if (inventoryGrass.isBig)
            {
                grassInvetory.itemAmount = Random.Range(2, 3);
            }
            else
            {
                grassInvetory.itemAmount = Random.Range(1, 2);
            }
            ItemManager.Instance.CreatItemInventoryItem(grassInvetory, Position);
            GrassManager.Instance.RemoveGrass(inventoryGrass);
            Destroy(gameObject);
        }

        public void Shoveled()
        {
            Harvested();
        }
    }
}