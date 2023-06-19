using System.Collections;
using UnityEngine;
public class ItemOnMap : MonoBehaviour
{
    MapItem item;
    public int indexInCurrentScene;
    public void Init(MapItem item, SerializableVector2 pos)
    {
        GetComponent<BoxCollider2D>().enabled = true;
        transform.position = new Vector2(pos.x, pos.y);
        this.item = item;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = DataManager.Instance.FindItemDetails(item.item.itemID).itemOnWorldSprite;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (DataManager.Instance.AddItemOnBag(item.item))
            {
                ItemManager.Instance.currentSceneMapItem.Remove(item);
                //TODO:音效
                Destroy(gameObject);
            }
        }
    }
    public void ItemThrown(MapItem item, SerializableVector2 pos)
    {
        this.item = item;
        transform.position = new Vector2(pos.x, pos.y + 0.5f);
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = DataManager.Instance.FindItemDetails(item.item.itemID).itemOnWorldSprite;
        StartCoroutine(RealItemThrown());
    }
    IEnumerator RealItemThrown()
    {
        int num;
        var second = new WaitForSeconds(0.01f);
        for (int i = 0; i < 5; i++)
        {
            num = i + 1;
            for (int j = 0; j < 5; j++)
            {
                transform.position += new Vector3(0.04f, -0.008f * i, 0);
                yield return second;
            }
        }
        GetComponent<BoxCollider2D>().enabled = true;
    }

}
