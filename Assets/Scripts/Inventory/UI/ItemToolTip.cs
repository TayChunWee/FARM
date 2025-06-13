using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;

    [SerializeField] private TextMeshProUGUI typeText;

    [SerializeField] private TextMeshProUGUI descriptionText;

    [SerializeField] private Text valueText;

    [SerializeField] private GameObject bottomPart;

    public void SetupToolTip(ItemDetails itemDetails , SlotType slotType)
    {
        nameText.text = itemDetails.itemName;

        typeText.text = GetItemType(itemDetails.itemType);

        descriptionText.text = itemDetails.itemDescription;

        if(itemDetails.itemType == ItemType.Seed || itemDetails.itemType == ItemType.Commodity || itemDetails.itemType == ItemType.Furniture)
        {
            bottomPart .SetActive (true);

            var price = itemDetails.itemPrice;

            if(slotType == SlotType.Bag)
            {
                price = (int)(price * itemDetails.sellPercentage);
            }

            valueText .text = price.ToString ();
        }
        else
        {
            bottomPart.SetActive(true); // 仍然显示底部
            valueText.text = "販売不可"; // 或者使用英文 "Not Sellable"
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    private string GetItemType(ItemType itemType)
    {
        return itemType switch
        {
            ItemType.Seed => "種",
            ItemType.Commodity => "商品",
            ItemType.Furniture => "家具",
            ItemType.BreakTool => "ツール",
            ItemType.ChopTool => "ツール",
            ItemType.CollectTool => "ツール",
            ItemType.HoeTool => "ツール",
            ItemType.ReapTool => "ツール",
            ItemType.WaterTool => "ツール",
            _=>"无"
        };
    }
}
