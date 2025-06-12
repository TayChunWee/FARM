using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Farm.Inventory
{
    public class ItemBounce : MonoBehaviour
    {
        private Transform spriteTrans;

        private BoxCollider2D coll;

        public float gravity


        private void Awake()
        {
            spriteTrans = transform.GetChild(0);
            coll = GetComponent<BoxCollider2D>();
            coll.enabled = false;
        }

        private void Update()
        {
            
        }
    }
}

