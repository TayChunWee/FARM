using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Inventory
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        public ItemDataList_SO itemDataList_SO;

        /// <summary>
        /// 通过ID返回物品信息
        /// </summary>
        /// <param name="ID">Item ID</param>
        /// <returns></returns>

        public ItemDetails GetItemDetails (int ID)
        {
            return itemDataList_SO.itemDetailsList.Find(i => i.itemID == ID);
        }

        public void AddItem(Item item,bool toDestroy)
        {
            Debug.Log(GetItemDetails(item.itemID).itemID + "Name:" + GetItemDetails(item.itemID).itemName);
            if (toDestroy)
            {
                Destroy(item.gameObject);
            }
        }
    }
}

