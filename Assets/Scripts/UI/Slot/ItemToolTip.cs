using UnityEngine;
using UnityEngine.UI;
namespace MyGame.UI
{
    public class ItemToolTip : MonoBehaviour
    {
        [SerializeField] Text itemName;
        [SerializeField] Text itemType;
        [SerializeField] Text description;
        [SerializeField] Text price;
        public void SetupToolTip(ItemDetails item, SlotType slotType)
        {
            if (item == null)
            {
                Debug.Log("物品不存在");
                return;
            }
            itemName.text = item.itemName;
            if (item.itemType.Length == 1)
            {
                itemType.text = item.itemType[0] switch
                {
                    ItemType.Food => "食物",
                    ItemType.Tool => "工具",
                    ItemType.Seed => "种子",
                    ItemType.Material => "材料",
                    ItemType.Buleprint => "蓝图",
                    ItemType.Floor => "地板",
                    ItemType.Lawn => "草坪",
                    ItemType.Commodidy => "商品",
                    _ => "不知道是啥"
                };
            }
            else
            {
                itemType.text = item.itemType[0] switch
                {
                    ItemType.Food => "食物",
                    ItemType.Tool => "工具",
                    ItemType.Seed => "种子",
                    ItemType.Material => "材料",
                    ItemType.Buleprint => "蓝图",
                    ItemType.Floor => "地板",
                    ItemType.Lawn => "草坪",
                    ItemType.Commodidy => "商品",
                    ItemType.Sign => "标志",
                    _ => "不知道是啥"
                };
                itemType.text = item.itemType[1] switch
                {
                    ItemType.Food => itemType.text += ",食物",
                    ItemType.Tool => itemType.text += ",工具",
                    ItemType.Seed => itemType.text += ",种子",
                    ItemType.Material => itemType.text += ",材料",
                    ItemType.Buleprint => itemType.text += ",蓝图",
                    ItemType.Floor => itemType.text += ",地板",
                    ItemType.Lawn => itemType.text += ",草坪",
                    ItemType.Commodidy => itemType.text += ",商品",
                    ItemType.Sign => itemType.text += ",标志",
                    _ => ",不知道是啥"
                };

            }
            description.text = item.itemdDescription;
            if (slotType != SlotType.Store)
            {
                price.text = item.itemPrice.ToString();
            }
            else
            {
                price.text = ((int)(item.itemPrice * 0.6f)).ToString();
            }

        }
        public void SetupToolTip(ToolDetails item, SlotType slotType)
        {
            itemName.text = item.toolName;
            itemType.text = "工具";
            description.text = item.toolDescription;
            if (slotType != SlotType.Store)
            {
                price.text = item.toolPrice.ToString();
            }
            else
            {
                price.text = ((int)(item.toolPrice * 0.6f)).ToString();
            }

        }
    }
}

