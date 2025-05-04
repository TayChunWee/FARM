using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Farm.Inventory
{
    public class SLOTUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("组件获取")]

        [SerializeField] private Image slotImage;

        [SerializeField] private TextMeshProUGUI amountText;

        public Image slotHighlight;

        [SerializeField] private Button button;

        [Header("格子类型")]

        public SlotType slotType;

        public bool isSelected;

        public int slotIndex;


        //物品信息
        public ItemDetails itemDetails;

        public int itemAmount;

        private InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();


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

            //自行修復（無根據視頻可能之後需要更改），原因是因爲他的itemAmount沒有等於0，并且沒有清除乾净數據
            itemAmount = 0;
            itemDetails = new ItemDetails();

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (itemAmount == 0) return;
            isSelected = !isSelected;

            inventoryUI.UpdateSlotHighlight(slotIndex);

            if(slotType == SlotType.Bag)
            {
                //通知物品被選中的狀態
                EventHandler.CallItemSelectedEvent(itemDetails,isSelected);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(itemAmount != 0)
            {
                inventoryUI.dragItem.enabled = true;
                inventoryUI.dragItem.sprite = slotImage.sprite;
                inventoryUI.dragItem.SetNativeSize();

                isSelected = true;
                inventoryUI.UpdateSlotHighlight(slotIndex);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            inventoryUI.dragItem.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            inventoryUI.dragItem.enabled = false;
            Debug.Log(eventData.pointerCurrentRaycast.gameObject);

            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                if (eventData.pointerCurrentRaycast.gameObject.GetComponent<SLOTUI>() == null)
                {
                    return;
                }

                var targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<SLOTUI>();
                int targetIndex = targetSlot.slotIndex;

                //在Player自身背包范围内交换
                if(slotType == SlotType.Bag && targetSlot.slotType == SlotType.Bag)
                {
                    InventoryManager.Instance.SwapItem(slotIndex, targetIndex);
                }

                inventoryUI.UpdateSlotHighlight(-1);
            }
            //else  //测试扔在地上
            //{
            //    if (itemDetails.canDropped)
            //    {
            //        //鼠标对应世界地图坐标
            //        var pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            //
            //        EventHandler.CallInstantiateItemInScene(itemDetails.itemID, pos);
            //    }
            //    
            //}
        }
    }
}


