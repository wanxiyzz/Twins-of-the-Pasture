using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Plant : MonoBehaviour
{
    [SerializeField] GameObject shadow;
    public PlantState plantState;
    public PlantDetails plantDetails;
    public SpriteRenderer spriteRenderer;
    public BoxCollider2D box;
    public void Init(PlantState plantState, PlantDetails plantDetails)
    {
        this.plantDetails = plantDetails;
        this.plantState = plantState;
        spriteRenderer = GetComponent<SpriteRenderer>();
        box = GetComponent<BoxCollider2D>();
        transform.position = plantState.position.ToVector3();
        spriteRenderer.sprite = plantDetails.growthSprite[0];
        UpdateSprite();
    }
    public void UpdateSprite()
    {

        plantState.currentPeriod = plantState.grouthHuornum / plantDetails.onceGrothHour + 1;
        if (plantState.currentPeriod > 1)
        {
            shadow.gameObject.SetActive(true);
            box.enabled = true;
        }
        if (plantDetails.growthSprite.Length <= plantState.currentPeriod)
        {
            if (plantState.currentHarvestPeriod <= 0)
            {
                plantState.currentHarvestPeriod = Random.Range(1, 4);
            }
            if (plantState.currentPeriod >= plantDetails.growthPeriod) return;
            spriteRenderer.sprite = plantDetails.canHarvestPlants[plantState.currentHarvestPeriod - 1].sprites[plantState.currentPeriod - plantDetails.growthSprite.Length];
        }
        else
        {
            spriteRenderer.sprite = plantDetails.growthSprite[plantState.currentPeriod - 1];
        }
    }
}
