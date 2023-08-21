using System.Collections;
using UnityEngine;
using MyGame.Data;
namespace MyGame.Item
{
    public class ItemOnMap : MonoBehaviour
    {
        MapItem item;
        [SerializeField] BoxCollider2D boxCollider2D;
        [SerializeField] SpriteRenderer spriteRenderer;
        public void Init(MapItem item, SerializableVector2 pos)
        {
            GetComponent<BoxCollider2D>().enabled = true;
            transform.position = new Vector2(pos.x, pos.y);
            this.item = item;
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = DataManager.Instance.FindItemDetails(item.item.itemID).itemOnWorldSprite;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (GameManager.Instance.playerCanPick)
            {
                if (other.CompareTag("Player"))
                {
                    if (DataManager.Instance.AddItemOnBag(item.item))
                    {
                        ItemManager.Instance.currentSceneMapItem.Remove(item);
                        //AUDIO:拾取音效
                        Destroy(gameObject);
                    }
                }
            }
        }
        public void ItemThrown(MapItem item, SerializableVector2 pos)
        {
            this.item = item;
            transform.position = new Vector2(pos.x, pos.y + 0.5f);
            var temp = DataManager.Instance.FindInventory(item.item.itemID);
            if (temp == null)
                spriteRenderer.sprite = DataManager.Instance.FindToolDetails(item.item.itemID).toolOnWorldSprite;
            else spriteRenderer.sprite = temp.itemOnWorldSprite;
            StartCoroutine(RealItemThrown());
        }
        IEnumerator RealItemThrown()
        {
            int num;
            for (int i = 0; i < 5; i++)
            {
                num = i + 1;
                for (int j = 0; j < 5; j++)
                {
                    transform.position += new Vector3(0.04f, -0.008f * i, 0);
                    yield return Tools.seconds;
                }
            }
            yield return new WaitForSeconds(0.5f);
            boxCollider2D.enabled = true;
        }
        public void ItemCreat(MapItem item, SerializableVector2 pos, Vector3 targetPos)
        {
            this.item = item;
            transform.position = new Vector2(pos.x, pos.y);
            var temp = DataManager.Instance.FindInventory(item.item.itemID);
            if (temp == null)
                spriteRenderer.sprite = DataManager.Instance.FindToolDetails(item.item.itemID).toolOnWorldSprite;
            else spriteRenderer.sprite = temp.itemOnWorldSprite;
            StartCoroutine(RealItemCreat(targetPos));
        }
        IEnumerator RealItemCreat(Vector3 targetPos)
        {
            Vector3 movepos = (targetPos - transform.position).normalized * 0.04f;
            while (Vector3.Distance(transform.position, targetPos) > 0.04f)
            {
                transform.position += movepos;
                yield return Tools.seconds;
            }
            yield return new WaitForSeconds(0.5f);
            boxCollider2D.enabled = true;
        }

    }
}