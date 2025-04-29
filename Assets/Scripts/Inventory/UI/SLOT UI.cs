using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Farm.Inventory
{
    public class SLOTUI : MonoBehaviour
    {
        [Header("组件获取")]

        [SerializeField] private Image slotImage;

        [SerializeField] private TextMeshProUGUI amountText;

        [SerializeField] private Image slotHighlight;

        [SerializeField] private Button button;

        [Header("格子类型")]

        public SlotType slotType;

        public bool isSelected;

        public int slotIndex;


        //物品信息
        public ItemDetails itemDetails;

        public int itemAmount;


        private void Start()
        {
            isSelected = false;
            if(itemDetails.itemID == 0)
            {
                UpdateEmptySlot();
            }
        }

        /// <summary>
        /// 更新格子UI和信息
        /// </summary>
        /// <param name="item">ItemDetails</param>
        /// <param name="amount">持有数量</param>
        public void UpdateSlot(ItemDetails item, int amount)
        {
            itemDetails = item;
            slotImage.sprite = item.itemIcon;
            itemAmount = amount;
            amountText.text = amount.ToString();
            slotImage.enabled = true;
            button.interactable = true;
        }

        /// <summary>
        /// 将Slot更新为空
        /// </summary>
        public void UpdateEmptySlot()
        {
            if (isSelected)
            {
                isSelected = false;
            }

            slotImage.enabled = false;
            amountText.text = string.Empty;
            button.interactable = false;
        }
    }
}


