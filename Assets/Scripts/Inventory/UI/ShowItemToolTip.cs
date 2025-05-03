using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Farm.Inventory
{
    [RequireComponent(typeof(SLOTUI))]
    public class ShowItemToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private SLOTUI slotUI;

        private InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();


        private void Awake()
        {
            slotUI = GetComponent<SLOTUI>();    
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(slotUI.itemAmount != 0)
            {
                inventoryUI.itemToolTip.gameObject.SetActive(true);
                inventoryUI.itemToolTip.SetupToolTip(slotUI.itemDetails, slotUI.slotType);

                inventoryUI.itemToolTip.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0);
                inventoryUI.itemToolTip.transform.position = transform.position + Vector3.up * 60;
            }
            else
            {
                inventoryUI.itemToolTip.gameObject.SetActive(false);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            inventoryUI.itemToolTip.gameObject.SetActive(false);
        }

    }

    
}

