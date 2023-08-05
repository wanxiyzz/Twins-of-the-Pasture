using UnityEngine;
using MyGame.Item;
namespace MyGame.PlantSystem
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Plant : MonoBehaviour
    {
        public PlantState plantState;
        private PlantDetails plantDetails;
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] BoxCollider2D box;
        public bool CanHarvest => plantState.currentPeriod >= plantDetails.canHarvestPeriod && !plantState.isDeath;
        public void Init(PlantState plantState, PlantDetails plantDetails)
        {
            this.plantDetails = plantDetails;
            this.plantState = plantState;
            transform.position = plantState.position.ToVector3();
            spriteRenderer.sprite = plantDetails.growthSprite[0];
            UpdateSprite();
        }

        public void UpdateSprite()
        {

            plantState.currentPeriod = plantState.grouthHuornum / plantDetails.onceGrothHour + 1;
            plantState.isDeath = plantState.currentPeriod - plantDetails.canHarvestPeriod >= 2;
            box.enabled = plantState.currentPeriod > 1;
            if (plantState.isDeath)
            {
                spriteRenderer.sprite = plantDetails.deathSpite;
            }
            if (plantDetails.growthSprite.Length < plantState.currentPeriod)
            {
                if (plantState.currentHarvestPeriod <= 0)
                {
                    plantState.currentHarvestPeriod = Random.Range(0, 3);
                }
                if (plantState.currentPeriod > plantDetails.growthPeriod) return;
                spriteRenderer.sprite = plantDetails.canHarvestPlants[plantState.currentHarvestPeriod].sprites[plantState.currentPeriod - plantDetails.growthSprite.Length - 1];
                Debug.Log(plantState.currentPeriod - plantDetails.growthSprite.Length - 1);
            }
            else
            {
                spriteRenderer.sprite = plantDetails.growthSprite[plantState.currentPeriod - 1];
            }
        }
        public void Havest()
        {
            if (!CanHarvest) return;
            var canHarvestPlant = plantDetails.canHarvestPlants[plantState.currentHarvestPeriod];
            InventoryItem item = new()
            {
                itemID = canHarvestPlant.fruitID,
                itemAmount = Random.Range(canHarvestPlant.amountMin, canHarvestPlant.amountMax),
            };
            ItemManager.Instance.ThrownItem(item, transform.position);
            if (plantDetails.canTwiceHavest)
            {
                plantState.currentPeriod = plantDetails.afterHavest;
                plantState.grouthHuornum = (plantDetails.afterHavest - 1) * plantDetails.onceGrothHour;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

}
